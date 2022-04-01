namespace SimForeflightLink
{
    partial class SimForeflightLink
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimForeflightLink));
            this.buttonSimConnect = new System.Windows.Forms.Button();
            this.buttonForeflight = new System.Windows.Forms.Button();
            this.lblSimStatus = new System.Windows.Forms.Label();
            this.lblForeFlightStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLatitude = new System.Windows.Forms.TextBox();
            this.tbLongitude = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAltitude = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbGroundSpeed = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPitch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbRoll = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbHeading = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbGroundTrack = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cbForeflightConnectType = new System.Windows.Forms.ComboBox();
            this.tbForeflightIP = new System.Windows.Forms.TextBox();
            this.checkboxSimconnectAuto = new System.Windows.Forms.CheckBox();
            this.checkboxForeFlightAuto = new System.Windows.Forms.CheckBox();
            this.labelIncompleteData = new System.Windows.Forms.Label();
            this.cbTraffic = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonSimConnect
            // 
            this.buttonSimConnect.Location = new System.Drawing.Point(53, 44);
            this.buttonSimConnect.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSimConnect.Name = "buttonSimConnect";
            this.buttonSimConnect.Size = new System.Drawing.Size(216, 28);
            this.buttonSimConnect.TabIndex = 0;
            this.buttonSimConnect.Text = "Connect to SimConnect";
            this.buttonSimConnect.UseVisualStyleBackColor = true;
            this.buttonSimConnect.Click += new System.EventHandler(this.buttonSimConnect_Click);
            // 
            // buttonForeflight
            // 
            this.buttonForeflight.Location = new System.Drawing.Point(828, 44);
            this.buttonForeflight.Margin = new System.Windows.Forms.Padding(4);
            this.buttonForeflight.Name = "buttonForeflight";
            this.buttonForeflight.Size = new System.Drawing.Size(225, 28);
            this.buttonForeflight.TabIndex = 1;
            this.buttonForeflight.Text = "Connect to ForeFlight";
            this.buttonForeflight.UseVisualStyleBackColor = true;
            this.buttonForeflight.Click += new System.EventHandler(this.buttonForeflight_Click);
            // 
            // lblSimStatus
            // 
            this.lblSimStatus.Location = new System.Drawing.Point(49, 76);
            this.lblSimStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSimStatus.Name = "lblSimStatus";
            this.lblSimStatus.Size = new System.Drawing.Size(375, 17);
            this.lblSimStatus.TabIndex = 2;
            this.lblSimStatus.Text = "Sim Status";
            // 
            // lblForeFlightStatus
            // 
            this.lblForeFlightStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblForeFlightStatus.Location = new System.Drawing.Point(677, 76);
            this.lblForeFlightStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblForeFlightStatus.Name = "lblForeFlightStatus";
            this.lblForeFlightStatus.Size = new System.Drawing.Size(376, 17);
            this.lblForeFlightStatus.TabIndex = 3;
            this.lblForeFlightStatus.Text = "ForeFlight Status";
            this.lblForeFlightStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(59, 242);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Latitude";
            // 
            // tbLatitude
            // 
            this.tbLatitude.Location = new System.Drawing.Point(127, 239);
            this.tbLatitude.Margin = new System.Windows.Forms.Padding(4);
            this.tbLatitude.Name = "tbLatitude";
            this.tbLatitude.ReadOnly = true;
            this.tbLatitude.Size = new System.Drawing.Size(132, 22);
            this.tbLatitude.TabIndex = 5;
            // 
            // tbLongitude
            // 
            this.tbLongitude.Location = new System.Drawing.Point(392, 239);
            this.tbLongitude.Margin = new System.Windows.Forms.Padding(4);
            this.tbLongitude.Name = "tbLongitude";
            this.tbLongitude.ReadOnly = true;
            this.tbLongitude.Size = new System.Drawing.Size(132, 22);
            this.tbLongitude.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(324, 242);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Longitude";
            // 
            // tbAltitude
            // 
            this.tbAltitude.Location = new System.Drawing.Point(648, 239);
            this.tbAltitude.Margin = new System.Windows.Forms.Padding(4);
            this.tbAltitude.Name = "tbAltitude";
            this.tbAltitude.ReadOnly = true;
            this.tbAltitude.Size = new System.Drawing.Size(132, 22);
            this.tbAltitude.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(580, 242);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Altitude";
            // 
            // tbGroundSpeed
            // 
            this.tbGroundSpeed.Location = new System.Drawing.Point(943, 239);
            this.tbGroundSpeed.Margin = new System.Windows.Forms.Padding(4);
            this.tbGroundSpeed.Name = "tbGroundSpeed";
            this.tbGroundSpeed.ReadOnly = true;
            this.tbGroundSpeed.Size = new System.Drawing.Size(132, 22);
            this.tbGroundSpeed.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(833, 242);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Ground Speed";
            // 
            // tbPitch
            // 
            this.tbPitch.Location = new System.Drawing.Point(127, 297);
            this.tbPitch.Margin = new System.Windows.Forms.Padding(4);
            this.tbPitch.Name = "tbPitch";
            this.tbPitch.ReadOnly = true;
            this.tbPitch.Size = new System.Drawing.Size(132, 22);
            this.tbPitch.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 300);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 16);
            this.label6.TabIndex = 12;
            this.label6.Text = "Pitch Degrees";
            // 
            // tbRoll
            // 
            this.tbRoll.Location = new System.Drawing.Point(392, 297);
            this.tbRoll.Margin = new System.Windows.Forms.Padding(4);
            this.tbRoll.Name = "tbRoll";
            this.tbRoll.ReadOnly = true;
            this.tbRoll.Size = new System.Drawing.Size(132, 22);
            this.tbRoll.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(293, 300);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Roll Degrees";
            // 
            // tbHeading
            // 
            this.tbHeading.Location = new System.Drawing.Point(648, 297);
            this.tbHeading.Margin = new System.Windows.Forms.Padding(4);
            this.tbHeading.Name = "tbHeading";
            this.tbHeading.ReadOnly = true;
            this.tbHeading.Size = new System.Drawing.Size(132, 22);
            this.tbHeading.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(544, 300);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 16);
            this.label8.TabIndex = 16;
            this.label8.Text = "True Heading";
            // 
            // tbGroundTrack
            // 
            this.tbGroundTrack.Location = new System.Drawing.Point(943, 297);
            this.tbGroundTrack.Margin = new System.Windows.Forms.Padding(4);
            this.tbGroundTrack.Name = "tbGroundTrack";
            this.tbGroundTrack.ReadOnly = true;
            this.tbGroundTrack.Size = new System.Drawing.Size(132, 22);
            this.tbGroundTrack.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(800, 300);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 16);
            this.label9.TabIndex = 18;
            this.label9.Text = "Ground Track(True)";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Local Pipe",
            "IPV4 connection",
            "IPV6 Connection"});
            this.comboBox1.Location = new System.Drawing.Point(53, 97);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(369, 24);
            this.comboBox1.TabIndex = 20;
            this.comboBox1.Text = "Local Pipe";
            // 
            // cbForeflightConnectType
            // 
            this.cbForeflightConnectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbForeflightConnectType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbForeflightConnectType.FormattingEnabled = true;
            this.cbForeflightConnectType.Items.AddRange(new object[] {
            "Direct IP",
            "Broadcast on Interface"});
            this.cbForeflightConnectType.Location = new System.Drawing.Point(677, 96);
            this.cbForeflightConnectType.Margin = new System.Windows.Forms.Padding(4);
            this.cbForeflightConnectType.Name = "cbForeflightConnectType";
            this.cbForeflightConnectType.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbForeflightConnectType.Size = new System.Drawing.Size(373, 24);
            this.cbForeflightConnectType.TabIndex = 21;
            this.cbForeflightConnectType.SelectedIndexChanged += new System.EventHandler(this.cbForeflightConnectType_SelectedIndexChanged);
            // 
            // tbForeflightIP
            // 
            this.tbForeflightIP.Location = new System.Drawing.Point(723, 130);
            this.tbForeflightIP.Margin = new System.Windows.Forms.Padding(4);
            this.tbForeflightIP.Name = "tbForeflightIP";
            this.tbForeflightIP.Size = new System.Drawing.Size(327, 22);
            this.tbForeflightIP.TabIndex = 22;
            this.tbForeflightIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // checkboxSimconnectAuto
            // 
            this.checkboxSimconnectAuto.AutoSize = true;
            this.checkboxSimconnectAuto.Location = new System.Drawing.Point(279, 50);
            this.checkboxSimconnectAuto.Margin = new System.Windows.Forms.Padding(4);
            this.checkboxSimconnectAuto.Name = "checkboxSimconnectAuto";
            this.checkboxSimconnectAuto.Size = new System.Drawing.Size(109, 20);
            this.checkboxSimconnectAuto.TabIndex = 23;
            this.checkboxSimconnectAuto.Text = "Auto-Connect";
            this.checkboxSimconnectAuto.UseVisualStyleBackColor = true;
            // 
            // checkboxForeFlightAuto
            // 
            this.checkboxForeFlightAuto.AutoSize = true;
            this.checkboxForeFlightAuto.Location = new System.Drawing.Point(699, 52);
            this.checkboxForeFlightAuto.Margin = new System.Windows.Forms.Padding(4);
            this.checkboxForeFlightAuto.Name = "checkboxForeFlightAuto";
            this.checkboxForeFlightAuto.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkboxForeFlightAuto.Size = new System.Drawing.Size(109, 20);
            this.checkboxForeFlightAuto.TabIndex = 24;
            this.checkboxForeFlightAuto.Text = "Auto-Connect";
            this.checkboxForeFlightAuto.UseVisualStyleBackColor = true;
            // 
            // labelIncompleteData
            // 
            this.labelIncompleteData.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelIncompleteData.Location = new System.Drawing.Point(16, 194);
            this.labelIncompleteData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIncompleteData.Name = "labelIncompleteData";
            this.labelIncompleteData.Size = new System.Drawing.Size(1067, 28);
            this.labelIncompleteData.TabIndex = 25;
            this.labelIncompleteData.Text = "ForeFlight Sending Temporarily Paused - Incomplete Data";
            this.labelIncompleteData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbTraffic
            // 
            this.cbTraffic.AutoSize = true;
            this.cbTraffic.Location = new System.Drawing.Point(127, 202);
            this.cbTraffic.Name = "cbTraffic";
            this.cbTraffic.Size = new System.Drawing.Size(101, 20);
            this.cbTraffic.TabIndex = 26;
            this.cbTraffic.Text = "Send Traffic";
            this.cbTraffic.UseVisualStyleBackColor = true;
            this.cbTraffic.Visible = false;
            // 
            // SimForeflightLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 352);
            this.Controls.Add(this.cbTraffic);
            this.Controls.Add(this.labelIncompleteData);
            this.Controls.Add(this.checkboxForeFlightAuto);
            this.Controls.Add(this.checkboxSimconnectAuto);
            this.Controls.Add(this.tbForeflightIP);
            this.Controls.Add(this.cbForeflightConnectType);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.tbGroundTrack);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbHeading);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbRoll);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbPitch);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbGroundSpeed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbAltitude);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbLongitude);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLatitude);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblForeFlightStatus);
            this.Controls.Add(this.lblSimStatus);
            this.Controls.Add(this.buttonForeflight);
            this.Controls.Add(this.buttonSimConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1114, 399);
            this.MinimumSize = new System.Drawing.Size(1114, 399);
            this.Name = "SimForeflightLink";
            this.Text = "SimForeflgihtLink";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSimConnect;
        private System.Windows.Forms.Button buttonForeflight;
        private System.Windows.Forms.Label lblSimStatus;
        private System.Windows.Forms.Label lblForeFlightStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLatitude;
        private System.Windows.Forms.TextBox tbLongitude;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbAltitude;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbGroundSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPitch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbRoll;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbHeading;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbGroundTrack;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox cbForeflightConnectType;
        private System.Windows.Forms.TextBox tbForeflightIP;
        private System.Windows.Forms.CheckBox checkboxSimconnectAuto;
        private System.Windows.Forms.CheckBox checkboxForeFlightAuto;
        private System.Windows.Forms.Label labelIncompleteData;
        private System.Windows.Forms.CheckBox cbTraffic;
    }
}

