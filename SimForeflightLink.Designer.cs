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
            this.buttonSimConnect = new System.Windows.Forms.Button();
            this.buttonForeflight = new System.Windows.Forms.Button();
            this.lblSimStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSimConnect
            // 
            this.buttonSimConnect.Location = new System.Drawing.Point(40, 36);
            this.buttonSimConnect.Name = "buttonSimConnect";
            this.buttonSimConnect.Size = new System.Drawing.Size(162, 23);
            this.buttonSimConnect.TabIndex = 0;
            this.buttonSimConnect.Text = "Connect to SimConnect";
            this.buttonSimConnect.UseVisualStyleBackColor = true;
            // 
            // buttonForeflight
            // 
            this.buttonForeflight.Location = new System.Drawing.Point(671, 36);
            this.buttonForeflight.Name = "buttonForeflight";
            this.buttonForeflight.Size = new System.Drawing.Size(169, 23);
            this.buttonForeflight.TabIndex = 1;
            this.buttonForeflight.Text = "Connect to ForeFlight";
            this.buttonForeflight.UseVisualStyleBackColor = true;
            // 
            // lblSimStatus
            // 
            this.lblSimStatus.AutoSize = true;
            this.lblSimStatus.Location = new System.Drawing.Point(37, 62);
            this.lblSimStatus.Name = "lblSimStatus";
            this.lblSimStatus.Size = new System.Drawing.Size(57, 13);
            this.lblSimStatus.TabIndex = 2;
            this.lblSimStatus.Text = "Sim Status";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(754, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "ForeFlight Status";
            // 
            // SimForeflightLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 536);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSimStatus);
            this.Controls.Add(this.buttonForeflight);
            this.Controls.Add(this.buttonSimConnect);
            this.Name = "SimForeflightLink";
            this.Text = "SimForeflgihtLink";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSimConnect;
        private System.Windows.Forms.Button buttonForeflight;
        private System.Windows.Forms.Label lblSimStatus;
        private System.Windows.Forms.Label label1;
    }
}

