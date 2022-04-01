using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using static SimForeflightLink.FlightData.FlightDataUpdatedEventArgs;
using static SimForeflightLink.SimConnectLink.SimConnectEventArgs;
using SimForeflightLink.Foreflight;
using System.Collections.Generic;
using System.Linq;
using static SimForeflightLink.Foreflight.ForeFlightNetworkOption;
using System.Runtime.InteropServices;

namespace SimForeflightLink
{

    public partial class SimForeflightLink : Form
    {

        SimConnectLink simConnectLink;
        FlightData flightData;
        Dictionary<uint, TrafficData> trafficDataMap = new Dictionary<uint, TrafficData>();
        ForeFlightSender foreFlightSender;
        List<ForeFlightNetworkOption> foreFlightNetworkOptions= new List<ForeFlightNetworkOption>(10);
        BindingSource foreflightNetBs = new BindingSource();
        SimConnectForeflightSettings settings = new SimConnectForeflightSettings();
        IPAddress loadedNetworkAddress = null;

        public SimForeflightLink()
        {
            InitializeComponent();
            flightData = new FlightData();
            flightData.OnFlightDataUpdate += FlightData_OnFlightDataUpdate;

            // IPv6 Link Local disabled until i figure out how to get the ipv6 link local anyIp.
            //foreFlightNetworkOptions.Add(new ForeFlightNetworkOption(ForeFlightNetworkOption.NetworkTypes.IPv6LinkLocal));
            foreFlightNetworkOptions.Add(new ForeFlightNetworkOption(ForeFlightNetworkOption.NetworkTypes.DirectIPv4));

            settings.SettingsLoaded += Setttings_SettingsLoaded;

        }

        protected override void DefWndProc(ref Message m)
        {
            try {
                if (m.Msg == SimConnectLink.WM_USER_SIMCONNECT)
                {
                    simConnectLink?.ReadMessages();
                }else
                {
                   base.DefWndProc(ref m);
                }
            }
            catch (COMException)
            {
                /* do nothing */
            }
        }

        private void RefreshNetworkList()
        {
            ISet<IPAddress> networkAddresses = Networks.GetAllIPv4Addresses();
            if (null != loadedNetworkAddress)
                networkAddresses.Add(loadedNetworkAddress);
            List<ForeFlightNetworkOption> toAdd = new List<ForeFlightNetworkOption>(networkAddresses.Count);
            foreach (IPAddress a in networkAddresses)
            {
                toAdd.Add(new ForeFlightNetworkOption(ForeFlightNetworkOption.NetworkTypes.IPv4NetworkBroadcast, a));
            }

            foreFlightNetworkOptions.RemoveAll((match) => match.NetworkType == ForeFlightNetworkOption.NetworkTypes.IPv4NetworkBroadcast);
            foreFlightNetworkOptions.AddRange(toAdd);
            foreFlightNetworkOptions.Sort();
            foreflightNetBs.ResetBindings(true);

            NetworkTypes savedFFNetworkType = SimConnectForeflightSettings.ParseForeFlightNetworkTypeSetting(settings.ForeflightNetworkType);

            int selectedOption = -1;
            if (savedFFNetworkType == NetworkTypes.IPv4NetworkBroadcast)
            {
                IPAddress broadcastIp = IPAddress.Loopback;
                IPAddress.TryParse(settings.ForeFlightLastIPv4BroadcastIp, out broadcastIp);
                selectedOption = foreFlightNetworkOptions.FindIndex(
                    (ForeFlightNetworkOption ffno) => ffno.NetworkType == NetworkTypes.IPv4NetworkBroadcast && ffno.Address.Equals(broadcastIp)
                    );
            } else
            {
                selectedOption = foreFlightNetworkOptions.FindIndex(
                    (ForeFlightNetworkOption ffno) => ffno.NetworkType == savedFFNetworkType
                    );
            }
            tbForeflightIP.Enabled = savedFFNetworkType == NetworkTypes.DirectIPv4;
            tbForeflightIP.Visible = savedFFNetworkType == NetworkTypes.DirectIPv4;

            cbForeflightConnectType.SelectedIndex = selectedOption == -1 ? 0 : selectedOption;
            cbForeflightConnectType.Invalidate();

        }

        protected override void OnLoad(EventArgs e)
        { 
            base.OnLoad(e);
            SetForeflightControls(ConnectorState.Disconnected);
            SetSimConnectControls(ConnectorState.Disconnected, "Disconnected from SimConnect");

            foreflightNetBs.DataSource = foreFlightNetworkOptions;
            cbForeflightConnectType.DataSource = foreflightNetBs;


            Binding bindingAutoConnectSim = new Binding("Checked", settings,
                "AutostartSimConnect", true, DataSourceUpdateMode.OnPropertyChanged);
            checkboxSimconnectAuto.DataBindings.Add(bindingAutoConnectSim);
            
            Binding bindingAutoConnectForeflight = new Binding("Checked", settings,
                "AutostartForeFlight", true, DataSourceUpdateMode.OnPropertyChanged);
            checkboxForeFlightAuto.DataBindings.Add(bindingAutoConnectForeflight);

            Binding bindingForeflightDirectIP = new Binding("Text", settings,
                "ForeFlightDirectIP", true, DataSourceUpdateMode.OnPropertyChanged);
            tbForeflightIP.DataBindings.Add(bindingForeflightDirectIP);

            settings.Reload();
            settings.PropertyChanged += (object sender, PropertyChangedEventArgs args) => settings.Save();
        }

        private void Setttings_SettingsLoaded(object sender, SettingsLoadedEventArgs e)
        {
            RefreshNetworkList();
            if (settings.AutostartSimConnect)
                buttonSimConnect_Click(sender, new EventArgs());
            if (settings.AutostartForeFlight)
                buttonForeflight_Click(sender, new EventArgs());
            if (!String.IsNullOrWhiteSpace(settings?.ForeFlightLastIPv4BroadcastIp))
                loadedNetworkAddress = IPAddress.Parse(settings.ForeFlightLastIPv4BroadcastIp);
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            flightData.ClearData();
            if (checkboxSimconnectAuto.Checked)
            {
                buttonSimConnect_Click(this, e);
            }
            if (checkboxForeFlightAuto.Checked)
            {
                buttonForeflight_Click(this, e);
            }

        }

        static private void UpdateValueTextBox(TextBox textBox, double? value, int percision, string suffix)
        {
            textBox.Invoke(new MethodInvoker(
                delegate
                {
                    textBox.Text = FormatValue(value, percision, suffix);
                    textBox.Invalidate();
                }));
        }

        private static string FormatValue(double? value, int percision, String unit)
        {
            return value == null || !value.HasValue ?
                    "--" :
                    value.Value.ToString("F" + percision.ToString()) + unit;
        }

        private void FlightData_OnFlightDataUpdate(object sender, FlightData.FlightDataUpdatedEventArgs e)
        {
            if (0 != (e.Field | FlightDataField.Altitude))
            {
                UpdateValueTextBox(tbAltitude, flightData.AltitudeFt, 2, " ft");
            }
            if (0 != (e.Field | FlightDataField.GroundSpeed))
            {
                UpdateValueTextBox(tbGroundSpeed, flightData.GroundSpeedKt, 1 , " knots");
            }
            if (0 != (e.Field | FlightDataField.GroundTrack))
            {
                UpdateValueTextBox(tbGroundTrack, flightData.GroundTrackDegrees, 1, "°");
            }
            if (0 != (e.Field | FlightDataField.TrueHeading))
            {
                UpdateValueTextBox(tbHeading, flightData.TrueHeadingDegrees, 1, "°");
            }
            if (0 != (e.Field | FlightDataField.Latitude))
            {
                UpdateValueTextBox(tbLatitude, flightData.Latitude, 4, "°");
            }
            if (0 != (e.Field | FlightDataField.Longitude))
            {
                UpdateValueTextBox(tbLongitude, flightData.Longitude, 4, "°");
            }
            if (0 != (e.Field | FlightDataField.Pitch))
            {
                UpdateValueTextBox(tbPitch, flightData.PitchDegrees, 2, "°");
            }
            if (0 != (e.Field | FlightDataField.Roll))
            {
                UpdateValueTextBox(tbRoll, flightData.RollDegrees, 2, "°");
            }

            labelIncompleteData.Visible = !ForeFlightSender.VerifyCompleteFlightData(flightData) && null != foreFlightSender;
            labelIncompleteData.Invalidate();
        }

        private void buttonSimConnect_Click(object sender, EventArgs e)
        {
            if (simConnectLink == null)
            {
                simConnectLink = new SimConnectLink(ref flightData);
                simConnectLink.OnConnectionStatusChange += SimConnectLink_OnConnectionStatusChange;
                simConnectLink.Connect(Handle);
            }
            else
            {
                simConnectLink.Disconnect();
                simConnectLink = null;
                GC.Collect();
                SetSimConnectControls(ConnectorState.Disconnected, "Disconnected from SimConnect");
            }
        }

        private void SimConnectLink_OnConnectionStatusChange(object sender, SimConnectLink.SimConnectEventArgs e)
        {
            this.Invoke(new MethodInvoker(
                                    delegate
                                    {
                                        switch (e.EventType)
                                        {
                                            case ConnectionEventType.Connected:
                                                SetSimConnectControls(ConnectorState.Connected, e.Message);
                                                break;
                                            case ConnectionEventType.Connecting:
                                                SetSimConnectControls(ConnectorState.Connecting, e.Message);
                                                break;
                                            case ConnectionEventType.Neutral:
                                                SetSimConnectControls(ConnectorState.Disconnected, e.Message);
                                                break;
                                            case ConnectionEventType.Abnormal_Disconnect:
                                                SetSimConnectControls(ConnectorState.Retrying, e.Message);
                                                break;
                                        }
                                    }));
        }
       
        private void cbForeflightConnectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ForeFlightNetworkOption selectedOption = cbForeflightConnectType.SelectedItem as ForeFlightNetworkOption;
            settings.ForeflightNetworkType = selectedOption.NetworkType.ToString();
            settings.ForeFlightLastIPv4BroadcastIp = selectedOption.NetworkType == NetworkTypes.IPv4NetworkBroadcast ?
                selectedOption.Address.ToString()
                : settings.ForeFlightLastIPv4BroadcastIp = "";
            tbForeflightIP.Enabled = selectedOption.NetworkType == NetworkTypes.DirectIPv4;
            tbForeflightIP.Visible = selectedOption.NetworkType == NetworkTypes.DirectIPv4;
        }

        private void buttonForeflight_Click(object sender, EventArgs e)
        {
            if (null == foreFlightSender)
            {
                IPAddress foreflightIPAddress = null;
                ForeFlightNetworkOption networkOption = cbForeflightConnectType.Items[cbForeflightConnectType.SelectedIndex] as ForeFlightNetworkOption;
                switch (networkOption.NetworkType)
                {
                    case ForeFlightNetworkOption.NetworkTypes.DirectIPv4:
                        foreflightIPAddress = IPAddress.Parse(settings.ForeFlightDirectIp);
                        break;
                    case ForeFlightNetworkOption.NetworkTypes.IPv4NetworkBroadcast:
                    case ForeFlightNetworkOption.NetworkTypes.IPv6LinkLocal:
                        foreflightIPAddress = networkOption.Address;
                        break;
                }

                foreFlightSender = new ForeFlightSender(ref flightData, ref trafficDataMap, new UdpClient(
                    networkOption.NetworkType == NetworkTypes.IPv6LinkLocal ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork
                    ));
                foreFlightSender.OnForeFlightSenderError += ForeFlightSender_OnForeFlightSenderError;
                IPEndPoint endpoint = new IPEndPoint(
                    foreflightIPAddress, ForeFlightSender.DEFAULT_PORT);
                foreFlightSender.EndPoint = endpoint;
                foreFlightSender.Start();
                SetForeflightControls(ConnectorState.Connected);
                labelIncompleteData.Visible = !ForeFlightSender.VerifyCompleteFlightData(flightData) && null != foreFlightSender;
                labelIncompleteData.Invalidate();
            }
            else
            {
                foreFlightSender.Stop();
                foreFlightSender = null;
                GC.Collect();
                SetForeflightControls(ConnectorState.Disconnected);
            }
        }

        private void ForeFlightSender_OnForeFlightSenderError(object sender, ForeFlightSender.ForeFlightErrorEventArgs e)
        {
            buttonForeflight.Invoke(new MethodInvoker( delegate 
                {
                    buttonForeflight_Click(sender, new EventArgs());
                    lblForeFlightStatus.Text = e.Message;
                    lblForeFlightStatus.ForeColor = Color.Red;
                    lblForeFlightStatus.Invalidate();
                })
            );
            
        }

        private enum ConnectorState
        {
            Disconnected,
            Connecting,
            Connected,
            Retrying
        };

        private void SetForeflightControls(ConnectorState state)
        {
            if (ConnectorState.Connected == state)
            {
                lblForeFlightStatus.Text = "ForeFlight Sender started.";
                lblForeFlightStatus.ForeColor = Color.Green;
                tbForeflightIP.Enabled = false;
                cbForeflightConnectType.Enabled = false;
                buttonForeflight.Text = "Stop Foreflight Sender";
            }
            else
            {
                buttonForeflight.Text = "Start ForeFlight Sender";
                lblForeFlightStatus.Text = "ForeFlight Sender stopped.";
                lblForeFlightStatus.ForeColor = Color.Black;
                tbForeflightIP.Enabled = true;
                cbForeflightConnectType.Enabled = true;
            }

            lblForeFlightStatus.Invalidate();
            tbForeflightIP.Invalidate();
            cbForeflightConnectType.Invalidate();
            buttonForeflight.Invalidate();
        }

        private void SetSimConnectControls(ConnectorState state, string statusMessage)
        {
            lblSimStatus.Text = statusMessage;
            switch (state)
            {
                case ConnectorState.Connected:
                    lblSimStatus.ForeColor = Color.Green;
                    buttonSimConnect.Text = "Disconnect from SimConnect";
                    break;
                case ConnectorState.Connecting:
                    lblSimStatus.ForeColor = Color.Black;
                    buttonSimConnect.Text = "Disconnect from SimConnect";
                    break;
                case ConnectorState.Disconnected:
                    lblSimStatus.ForeColor = Color.Black;
                    buttonSimConnect.Text = "Connect to SimConnect";
                    break;
                case ConnectorState.Retrying:
                    lblSimStatus.ForeColor = Color.Red;
                    buttonSimConnect.Text = "Disconnect from SimConnect";
                    lblSimStatus.Text += "... Retrying";
                    break;

            }

            lblSimStatus.Invalidate();
            buttonSimConnect.Invalidate();
        }

        #region Settings

        //Application settings wrapper class
        sealed class SimConnectForeflightSettings : ApplicationSettingsBase
        {
            [UserScopedSetting()]
            [DefaultSettingValue("False")]
            public bool AutostartSimConnect
            {
                get { return (bool)this["AutostartSimConnect"]; }
                set { this["AutostartSimConnect"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("False")]
            public bool AutostartForeFlight
            {
                get { return (bool)this["AutostartForeFlight"]; }
                set { this["AutostartForeFlight"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("127.0.0.1")]
            public string ForeFlightDirectIp
            {
                get { return (string)this["ForeFlightDirectIp"]; }
                set { this["ForeFlightDirectIp"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("192.168.0.255")]
            public string ForeFlightLastIPv4BroadcastIp
            {
                get { return (string)this["ForeFlightLastIPv4BroadcastIp"]; }
                set { this["ForeFlightLastIPv4BroadcastIp"] = value; }
            }

            [UserScopedSetting()]
            [DefaultSettingValue("IPv4NetworkBroadcast")]
            public string ForeflightNetworkType
            {
                get { return  (string)this["ForeflightNetworkType"]; }
                set { this["ForeflightNetworkType"] = value; }
            }

            static public NetworkTypes ParseForeFlightNetworkTypeSetting(string stringVal)
            {
                return (NetworkTypes)Enum.Parse(typeof(NetworkTypes), stringVal);
            }
        }

        #endregion Settings

    }
}
