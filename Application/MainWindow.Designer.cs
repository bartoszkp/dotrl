namespace Application
{
    partial class MainWindow
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
            System.Windows.Forms.ToolStripDropDownButton newButton;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            System.Windows.Forms.ToolStripMenuItem experimentToolStripMenuItem;
            System.Windows.Forms.ToolStripMenuItem batchExperimentToolStripMenuItem;
            this.mainMenuStrip = new System.Windows.Forms.ToolStrip();
            this.connectToRlGlueButton = new System.Windows.Forms.ToolStripButton();
            this.globalSettingsButton = new System.Windows.Forms.ToolStripButton();
            newButton = new System.Windows.Forms.ToolStripDropDownButton();
            experimentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            batchExperimentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // newButton
            // 
            newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            newButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            experimentToolStripMenuItem,
            batchExperimentToolStripMenuItem});
            newButton.Image = ((System.Drawing.Image)(resources.GetObject("newButton.Image")));
            newButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            newButton.Name = "newButton";
            newButton.Size = new System.Drawing.Size(41, 22);
            newButton.Text = "New";
            // 
            // experimentToolStripMenuItem
            // 
            experimentToolStripMenuItem.Name = "experimentToolStripMenuItem";
            experimentToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            experimentToolStripMenuItem.Text = "Experiment";
            experimentToolStripMenuItem.Click += new System.EventHandler(this.NewExperimentButtonClick);
            // 
            // batchExperimentToolStripMenuItem
            // 
            batchExperimentToolStripMenuItem.Name = "batchExperimentToolStripMenuItem";
            batchExperimentToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            batchExperimentToolStripMenuItem.Text = "Batch experiment";
            batchExperimentToolStripMenuItem.Click += new System.EventHandler(this.BatchExperimentButtonClick);
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            newButton,
            this.connectToRlGlueButton,
            this.globalSettingsButton});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(772, 25);
            this.mainMenuStrip.TabIndex = 0;
            // 
            // connectToRlGlueButton
            // 
            this.connectToRlGlueButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.connectToRlGlueButton.Image = ((System.Drawing.Image)(resources.GetObject("connectToRlGlueButton.Image")));
            this.connectToRlGlueButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.connectToRlGlueButton.Name = "connectToRlGlueButton";
            this.connectToRlGlueButton.Size = new System.Drawing.Size(100, 22);
            this.connectToRlGlueButton.Text = "Connect to RLGlue";
            this.connectToRlGlueButton.Click += new System.EventHandler(this.ConnectToRlGlueButtonClick);
            // 
            // globalSettingsButton
            // 
            this.globalSettingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.globalSettingsButton.Image = ((System.Drawing.Image)(resources.GetObject("globalSettingsButton.Image")));
            this.globalSettingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.globalSettingsButton.Name = "globalSettingsButton";
            this.globalSettingsButton.Size = new System.Drawing.Size(81, 22);
            this.globalSettingsButton.Text = "Global settings";
            this.globalSettingsButton.Click += new System.EventHandler(this.GlobalSettingsButtonClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 399);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "dotRL";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripButton globalSettingsButton;
        private System.Windows.Forms.ToolStripButton connectToRlGlueButton;
    }

}