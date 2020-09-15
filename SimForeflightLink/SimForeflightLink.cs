using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using static SimForeflightLink.FlightData.FlightDataUpdatedEventArgs;
using static SimForeflightLink.SimConnectLink.SimConnectEventArgs;

namespace SimForeflightLink
{
    public partial class SimForeflightLink : Form
    {
        SimConnectLink simConnectLink;
        FlightData flightData;
        ForeFlightSender foreFlightSender;

        // TODO: add a new thread and make the sim connection work on there.

        public SimForeflightLink()
        {
            InitializeComponent();
            flightData = new FlightData();
            flightData.OnFlightDataUpdate += FlightData_OnFlightDataUpdate;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetForeflightControls(ConnectorState.Disconnected);
            SetSimConnectControls(ConnectorState.Disconnected, "Disconnected from SimConnect");

            //TODO: Load app preferences

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
                UpdateValueTextBox(tbGroundTrack, flightData.GroundTrackDegress, 1, "°");
            }
            if (0 != (e.Field | FlightDataField.TrueHeading))
            {
                UpdateValueTextBox(tbHeading, flightData.TrueHeadingDegrees, 1, "°");
            }
            if (0 != (e.Field | FlightDataField.Latitude))
            {
                UpdateValueTextBox(tbLatitude, flightData.Latitude, 4, "°");
            }
            if (0 != (e.Field | FlightDataField.Longitudue))
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

        private void buttonForeflight_Click(object sender, EventArgs e)
        {
            if (null == foreFlightSender)
            {
                foreFlightSender = new ForeFlightSender(ref flightData, new UdpClient());
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(tbForeflightIP.Text), ForeFlightSender.DEFAULT_PORT);
                foreFlightSender.EndPoint = endpoint;
                foreFlightSender.Start();
                SetForeflightControls(ConnectorState.Connected);
            } else
            {
                foreFlightSender.Stop();
                foreFlightSender = null;
                GC.Collect();
                SetForeflightControls(ConnectorState.Disconnected);
            }
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
                cbForeflightConnectType.Enabled = false; // TOOD: true once dropdown works.
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
    }
}
