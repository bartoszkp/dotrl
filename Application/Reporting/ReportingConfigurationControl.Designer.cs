namespace Application.Reporting
{
    partial class ReportingConfigurationControl
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
            this.reportFilesTabControl = new System.Windows.Forms.TabControl();
            this.addReportFileButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reportFilesTabControl
            // 
            this.reportFilesTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportFilesTabControl.Location = new System.Drawing.Point(0, 35);
            this.reportFilesTabControl.Name = "reportFilesTabControl";
            this.reportFilesTabControl.SelectedIndex = 0;
            this.reportFilesTabControl.Size = new System.Drawing.Size(805, 602);
            this.reportFilesTabControl.TabIndex = 0;
            // 
            // addReportFileButton
            // 
            this.addReportFileButton.Location = new System.Drawing.Point(12, 6);
            this.addReportFileButton.Name = "addReportFileButton";
            this.addReportFileButton.Size = new System.Drawing.Size(114, 23);
            this.addReportFileButton.TabIndex = 1;
            this.addReportFileButton.Text = "Add report file";
            this.addReportFileButton.UseVisualStyleBackColor = true;
            this.addReportFileButton.Click += new System.EventHandler(this.AddReportFileButtonClick);
            // 
            // ReportingConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.addReportFileButton);
            this.Controls.Add(this.reportFilesTabControl);
            this.Name = "ReportingConfigurationControl";
            this.Size = new System.Drawing.Size(808, 640);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl reportFilesTabControl;
        private System.Windows.Forms.Button addReportFileButton;
    }
}