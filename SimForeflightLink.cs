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
            switch (e.Field)
            {
                case FlightDataField.AltitudeFt:
                    UpdateValueTextBox(tbAltitude, flightData.AltitudeFt, 2, " ft");
                    break;

                case FlightDataField.GroundSpeedKt:
                    UpdateValueTextBox(tbGroundSpeed, flightData.GroundSpeedKt, 1 , " knots");
                    break;

                case FlightDataField.GroundTrackDeg:
                    UpdateValueTextBox(tbGroundTrack, flightData.GroundTrackDegress, 1, "°");
                    break;

                case FlightDataField.TrueheadingDeg:
                    UpdateValueTextBox(tbHeading, flightData.TrueHeadingDegrees, 1, "°");
                    break;

                case FlightDataField.Latitude:
                    UpdateValueTextBox(tbLatitude, flightData.Latitude, 4, "°");
                    break;

                case FlightDataField.Longitudue:
                    UpdateValueTextBox(tbLongitude, flightData.Longitude, 4, "°");
                    break;

                case FlightDataField.PitchDeg:
                    UpdateValueTextBox(tbPitch, flightData.PitchDegrees, 2, "°");
                    break;

                case FlightData.FlightDataUpdatedEventArgs.FlightDataField.RollDeg:
                    UpdateValueTextBox(tbRoll, flightData.RollDegrees, 2, "°");
                    break;
            }
        }

        private void buttonSimConnect_Click(object sender, EventArgs e)
        {
            simConnectLink = new SimConnectLink(ref flightData);
            simConnectLink.OnConnectionStatusChange += SimConnectLink_OnConnectionStatusChange;
            simConnectLink.Connect(Handle);
        }

        private void SimConnectLink_OnConnectionStatusChange(object sender, SimConnectLink.SimConnectEventArgs e)
        {
            lblSimStatus.Invoke(new MethodInvoker(
                                    delegate
                                    {
                                        lblSimStatus.Text = e.Message;
                                        switch (e.EventType)
                                        {
                                            case ConnectionEventType.Connected:
                                                lblSimStatus.ForeColor = Color.Green;
                                                break;
                                            case ConnectionEventType.Connecting:
                                                lblSimStatus.ForeColor = Color.Black;
                                                break;
                                            case ConnectionEventType.Neutral:
                                                lblSimStatus.ForeColor = Color.Black;
                                                break;
                                            case ConnectionEventType.Abnormal_Disconnect:
                                                lblSimStatus.ForeColor = Color.Red;
                                                break;
                                        }
                                        lblSimStatus.Invalidate();
                                    }));
        }

        private void buttonForeflight_Click(object sender, EventArgs e)
        {
            foreFlightSender = new ForeFlightSender(ref flightData, new UdpClient());
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(tbForeflightIP.Text), ForeFlightSender.DEFAULT_PORT);
            foreFlightSender.EndPoint = endpoint;
            foreFlightSender.Start();
        }
    }
}
