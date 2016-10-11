namespace Application
{
    partial class ExperimentWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExperimentWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.realtimeLearningButton = new System.Windows.Forms.ToolStripButton();
            this.backgroundLearningButton = new System.Windows.Forms.ToolStripButton();
            this.pauseButton = new System.Windows.Forms.ToolStripButton();
            this.experimentParametersButton = new System.Windows.Forms.ToolStripButton();
            this.presentCurrentPolicyButton = new System.Windows.Forms.ToolStripButton();
            this.reportingParametersButton = new System.Windows.Forms.ToolStripButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.realtimeLearningButton,
            this.backgroundLearningButton,
            this.pauseButton,
            this.experimentParametersButton,
            this.presentCurrentPolicyButton,
            this.reportingParametersButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(781, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // realtimeLearningButton
            // 
            this.realtimeLearningButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.realtimeLearningButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.realtimeLearningButton.Name = "realtimeLearningButton";
            this.realtimeLearningButton.Size = new System.Drawing.Size(96, 22);
            this.realtimeLearningButton.Text = "Real time learning";
            this.realtimeLearningButton.Click += new System.EventHandler(this.RealtimeLearningButtonClick);
            // 
            // backgroundLearningButton
            // 
            this.backgroundLearningButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.backgroundLearningButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backgroundLearningButton.Name = "backgroundLearningButton";
            this.backgroundLearningButton.Size = new System.Drawing.Size(108, 22);
            this.backgroundLearningButton.Text = "Background learning";
            this.backgroundLearningButton.Click += new System.EventHandler(this.BackgroundLearningButtonClick);
            // 
            // pauseButton
            // 
            this.pauseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.pauseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(40, 22);
            this.pauseButton.Text = "Pause";
            this.pauseButton.Click += new System.EventHandler(this.PauseButtonClick);
            // 
            // experimentParametersButton
            // 
            this.experimentParametersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.experimentParametersButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.experimentParametersButton.Name = "experimentParametersButton";
            this.experimentParametersButton.Size = new System.Drawing.Size(176, 22);
            this.experimentParametersButton.Text = "Change parameters of experiment";
            this.experimentParametersButton.Click += new System.EventHandler(this.ExperimentParametersButtonClick);
            // 
            // presentCurrentPolicyButton
            // 
            this.presentCurrentPolicyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.presentCurrentPolicyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.presentCurrentPolicyButton.Name = "presentCurrentPolicyButton";
            this.presentCurrentPolicyButton.Size = new System.Drawing.Size(116, 22);
            this.presentCurrentPolicyButton.Text = "Present current policy";
            this.presentCurrentPolicyButton.Click += new System.EventHandler(this.PresentCurrentPolicyButtonClick);
            // 
            // reportingParametersButton
            // 
            this.reportingParametersButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.reportingParametersButton.Name = "reportingParametersButton";
            this.reportingParametersButton.Size = new System.Drawing.Size(161, 22);
            this.reportingParametersButton.Text = "Change reporting configuration";
            this.reportingParametersButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.reportingParametersButton.Click += new System.EventHandler(this.ReportingParametersButtonClick);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 150;
            this.timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 308);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(757, 22);
            this.progressBar1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(13, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(756, 273);
            this.panel1.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 333);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(781, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(25, 17);
            this.toolStripStatusLabel1.Text = "Idle";
            // 
            // ExperimentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 355);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExperimentWindow";
            this.Text = "Experiment Window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExperimentWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExperimentWindow_FormClosed);
            this.SizeChanged += new System.EventHandler(this.OnSizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ExperimentWindow_Paint);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton realtimeLearningButton;
        private System.Windows.Forms.ToolStripButton backgroundLearningButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripButton pauseButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripButton experimentParametersButton;
        private System.Windows.Forms.ToolStripButton presentCurrentPolicyButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton reportingParametersButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}