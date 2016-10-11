namespace Application
{
    partial class BatchExperimentSetupWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchExperimentSetupWindow));
            this.experimentListBox = new System.Windows.Forms.ListBox();
            this.addExperimentButton = new System.Windows.Forms.Button();
            this.loadExperimentButton = new System.Windows.Forms.Button();
            this.environmentInfoLabel = new System.Windows.Forms.Label();
            this.environmentLabel = new System.Windows.Forms.Label();
            this.agentInfoLabel = new System.Windows.Forms.Label();
            this.agentLabel = new System.Windows.Forms.Label();
            this.experimentLabelInfo = new System.Windows.Forms.Label();
            this.experimentEpisodeCountLabel = new System.Windows.Forms.Label();
            this.experimentEpisodeStepCountLabel = new System.Windows.Forms.Label();
            this.experimentTotalStepCountLabel = new System.Windows.Forms.Label();
            this.cloneExperimentButton = new System.Windows.Forms.Button();
            this.cloneCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.batchModeLabel = new System.Windows.Forms.Label();
            this.batchModeComboBox = new System.Windows.Forms.ComboBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.saveBatchExperimentTemplateButton = new System.Windows.Forms.Button();
            this.loadBatchExperimentTemplateButton = new System.Windows.Forms.Button();
            this.configureExperimentButton = new System.Windows.Forms.Button();
            this.removeExperimentButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cloneCountUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // experimentListBox
            // 
            this.experimentListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentListBox.FormattingEnabled = true;
            this.experimentListBox.Items.AddRange(new object[] {
            "Experiment 1",
            "Experiment 2"});
            this.experimentListBox.Location = new System.Drawing.Point(210, 12);
            this.experimentListBox.Name = "experimentListBox";
            this.experimentListBox.Size = new System.Drawing.Size(228, 238);
            this.experimentListBox.TabIndex = 0;
            this.experimentListBox.SelectedIndexChanged += new System.EventHandler(this.ExperimentListSelectedChanged);
            // 
            // addExperimentButton
            // 
            this.addExperimentButton.Location = new System.Drawing.Point(12, 12);
            this.addExperimentButton.Name = "addExperimentButton";
            this.addExperimentButton.Size = new System.Drawing.Size(116, 23);
            this.addExperimentButton.TabIndex = 1;
            this.addExperimentButton.Text = "Add experiment";
            this.addExperimentButton.UseVisualStyleBackColor = true;
            this.addExperimentButton.Click += new System.EventHandler(this.AddExperimentButtonClick);
            // 
            // loadExperimentButton
            // 
            this.loadExperimentButton.Location = new System.Drawing.Point(12, 41);
            this.loadExperimentButton.Name = "loadExperimentButton";
            this.loadExperimentButton.Size = new System.Drawing.Size(116, 23);
            this.loadExperimentButton.TabIndex = 2;
            this.loadExperimentButton.Text = "Load experiment";
            this.loadExperimentButton.UseVisualStyleBackColor = true;
            this.loadExperimentButton.Click += new System.EventHandler(this.LoadExperimentButtonClick);
            // 
            // environmentInfoLabel
            // 
            this.environmentInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentInfoLabel.AutoSize = true;
            this.environmentInfoLabel.Location = new System.Drawing.Point(444, 12);
            this.environmentInfoLabel.Name = "environmentInfoLabel";
            this.environmentInfoLabel.Size = new System.Drawing.Size(69, 13);
            this.environmentInfoLabel.TabIndex = 3;
            this.environmentInfoLabel.Text = "Environment:";
            // 
            // environmentLabel
            // 
            this.environmentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentLabel.AutoSize = true;
            this.environmentLabel.Location = new System.Drawing.Point(444, 25);
            this.environmentLabel.Name = "environmentLabel";
            this.environmentLabel.Size = new System.Drawing.Size(102, 13);
            this.environmentLabel.TabIndex = 4;
            this.environmentLabel.Text = "GridEnvironmentFlat";
            // 
            // agentInfoLabel
            // 
            this.agentInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.agentInfoLabel.AutoSize = true;
            this.agentInfoLabel.Location = new System.Drawing.Point(444, 54);
            this.agentInfoLabel.Name = "agentInfoLabel";
            this.agentInfoLabel.Size = new System.Drawing.Size(38, 13);
            this.agentInfoLabel.TabIndex = 5;
            this.agentInfoLabel.Text = "Agent:";
            // 
            // agentLabel
            // 
            this.agentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.agentLabel.AutoSize = true;
            this.agentLabel.Location = new System.Drawing.Point(444, 67);
            this.agentLabel.Name = "agentLabel";
            this.agentLabel.Size = new System.Drawing.Size(84, 13);
            this.agentLabel.TabIndex = 6;
            this.agentLabel.Text = "QLearningAgent";
            // 
            // experimentLabelInfo
            // 
            this.experimentLabelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentLabelInfo.AutoSize = true;
            this.experimentLabelInfo.Location = new System.Drawing.Point(444, 97);
            this.experimentLabelInfo.Name = "experimentLabelInfo";
            this.experimentLabelInfo.Size = new System.Drawing.Size(62, 13);
            this.experimentLabelInfo.TabIndex = 7;
            this.experimentLabelInfo.Text = "Experiment:";
            // 
            // experimentEpisodeCountLabel
            // 
            this.experimentEpisodeCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentEpisodeCountLabel.AutoSize = true;
            this.experimentEpisodeCountLabel.Location = new System.Drawing.Point(444, 110);
            this.experimentEpisodeCountLabel.Name = "experimentEpisodeCountLabel";
            this.experimentEpisodeCountLabel.Size = new System.Drawing.Size(70, 13);
            this.experimentEpisodeCountLabel.TabIndex = 8;
            this.experimentEpisodeCountLabel.Tag = "";
            this.experimentEpisodeCountLabel.Text = "100 episodes";
            // 
            // experimentEpisodeStepCountLabel
            // 
            this.experimentEpisodeStepCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentEpisodeStepCountLabel.AutoSize = true;
            this.experimentEpisodeStepCountLabel.Location = new System.Drawing.Point(444, 123);
            this.experimentEpisodeStepCountLabel.Name = "experimentEpisodeStepCountLabel";
            this.experimentEpisodeStepCountLabel.Size = new System.Drawing.Size(98, 13);
            this.experimentEpisodeStepCountLabel.TabIndex = 9;
            this.experimentEpisodeStepCountLabel.Text = "50 steps in episode";
            // 
            // experimentTotalStepCountLabel
            // 
            this.experimentTotalStepCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentTotalStepCountLabel.AutoSize = true;
            this.experimentTotalStepCountLabel.Location = new System.Drawing.Point(444, 136);
            this.experimentTotalStepCountLabel.Name = "experimentTotalStepCountLabel";
            this.experimentTotalStepCountLabel.Size = new System.Drawing.Size(145, 13);
            this.experimentTotalStepCountLabel.TabIndex = 10;
            this.experimentTotalStepCountLabel.Text = "1000000 total step count limit";
            // 
            // cloneExperimentButton
            // 
            this.cloneExperimentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cloneExperimentButton.Location = new System.Drawing.Point(210, 256);
            this.cloneExperimentButton.Name = "cloneExperimentButton";
            this.cloneExperimentButton.Size = new System.Drawing.Size(75, 23);
            this.cloneExperimentButton.TabIndex = 11;
            this.cloneExperimentButton.Text = "Clone";
            this.cloneExperimentButton.UseVisualStyleBackColor = true;
            this.cloneExperimentButton.Click += new System.EventHandler(this.CloneButtonClick);
            // 
            // cloneCountUpDown
            // 
            this.cloneCountUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cloneCountUpDown.Location = new System.Drawing.Point(291, 259);
            this.cloneCountUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.cloneCountUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.cloneCountUpDown.Name = "cloneCountUpDown";
            this.cloneCountUpDown.Size = new System.Drawing.Size(80, 20);
            this.cloneCountUpDown.TabIndex = 13;
            this.cloneCountUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // batchModeLabel
            // 
            this.batchModeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.batchModeLabel.AutoSize = true;
            this.batchModeLabel.Location = new System.Drawing.Point(444, 213);
            this.batchModeLabel.Name = "batchModeLabel";
            this.batchModeLabel.Size = new System.Drawing.Size(67, 13);
            this.batchModeLabel.TabIndex = 14;
            this.batchModeLabel.Text = "Batch mode:";
            // 
            // batchModeComboBox
            // 
            this.batchModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.batchModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.batchModeComboBox.FormattingEnabled = true;
            this.batchModeComboBox.Items.AddRange(new object[] {
            "Parallel",
            "Sequential"});
            this.batchModeComboBox.Location = new System.Drawing.Point(444, 229);
            this.batchModeComboBox.Name = "batchModeComboBox";
            this.batchModeComboBox.Size = new System.Drawing.Size(103, 21);
            this.batchModeComboBox.TabIndex = 15;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(525, 315);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(447, 315);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 17;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // saveBatchExperimentTemplateButton
            // 
            this.saveBatchExperimentTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveBatchExperimentTemplateButton.Location = new System.Drawing.Point(12, 315);
            this.saveBatchExperimentTemplateButton.Name = "saveBatchExperimentTemplateButton";
            this.saveBatchExperimentTemplateButton.Size = new System.Drawing.Size(171, 23);
            this.saveBatchExperimentTemplateButton.TabIndex = 18;
            this.saveBatchExperimentTemplateButton.Text = "Save batch experiment template";
            this.saveBatchExperimentTemplateButton.UseVisualStyleBackColor = true;
            this.saveBatchExperimentTemplateButton.Click += new System.EventHandler(this.SaveBatchExperimentTemplateButtonClick);
            // 
            // loadBatchExperimentTemplateButton
            // 
            this.loadBatchExperimentTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadBatchExperimentTemplateButton.Location = new System.Drawing.Point(189, 315);
            this.loadBatchExperimentTemplateButton.Name = "loadBatchExperimentTemplateButton";
            this.loadBatchExperimentTemplateButton.Size = new System.Drawing.Size(182, 23);
            this.loadBatchExperimentTemplateButton.TabIndex = 19;
            this.loadBatchExperimentTemplateButton.Text = "Load batch experiment template";
            this.loadBatchExperimentTemplateButton.UseVisualStyleBackColor = true;
            this.loadBatchExperimentTemplateButton.Click += new System.EventHandler(this.LoadBatchExperimentTemplateButtonClick);
            // 
            // configureExperimentButton
            // 
            this.configureExperimentButton.Location = new System.Drawing.Point(129, 198);
            this.configureExperimentButton.Name = "configureExperimentButton";
            this.configureExperimentButton.Size = new System.Drawing.Size(75, 23);
            this.configureExperimentButton.TabIndex = 20;
            this.configureExperimentButton.Text = "Configure";
            this.configureExperimentButton.UseVisualStyleBackColor = true;
            this.configureExperimentButton.Click += new System.EventHandler(this.ConfigureButtonClick);
            // 
            // removeExperimentButton
            // 
            this.removeExperimentButton.Location = new System.Drawing.Point(129, 227);
            this.removeExperimentButton.Name = "removeExperimentButton";
            this.removeExperimentButton.Size = new System.Drawing.Size(75, 23);
            this.removeExperimentButton.TabIndex = 21;
            this.removeExperimentButton.Text = "Remove";
            this.removeExperimentButton.UseVisualStyleBackColor = true;
            this.removeExperimentButton.Click += new System.EventHandler(this.RemoveButtonClick);
            // 
            // BatchExperimentSetupWindow
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(612, 350);
            this.Controls.Add(this.removeExperimentButton);
            this.Controls.Add(this.configureExperimentButton);
            this.Controls.Add(this.loadBatchExperimentTemplateButton);
            this.Controls.Add(this.saveBatchExperimentTemplateButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.batchModeComboBox);
            this.Controls.Add(this.batchModeLabel);
            this.Controls.Add(this.cloneCountUpDown);
            this.Controls.Add(this.cloneExperimentButton);
            this.Controls.Add(this.experimentTotalStepCountLabel);
            this.Controls.Add(this.experimentEpisodeStepCountLabel);
            this.Controls.Add(this.experimentEpisodeCountLabel);
            this.Controls.Add(this.experimentLabelInfo);
            this.Controls.Add(this.agentLabel);
            this.Controls.Add(this.agentInfoLabel);
            this.Controls.Add(this.environmentLabel);
            this.Controls.Add(this.environmentInfoLabel);
            this.Controls.Add(this.loadExperimentButton);
            this.Controls.Add(this.addExperimentButton);
            this.Controls.Add(this.experimentListBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BatchExperimentSetupWindow";
            this.Text = "BatchExperimentSetupWindow";
            ((System.ComponentModel.ISupportInitialize)(this.cloneCountUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox experimentListBox;
        private System.Windows.Forms.Button addExperimentButton;
        private System.Windows.Forms.Button loadExperimentButton;
        private System.Windows.Forms.Label environmentInfoLabel;
        private System.Windows.Forms.Label environmentLabel;
        private System.Windows.Forms.Label agentInfoLabel;
        private System.Windows.Forms.Label agentLabel;
        private System.Windows.Forms.Label experimentLabelInfo;
        private System.Windows.Forms.Label experimentEpisodeCountLabel;
        private System.Windows.Forms.Label experimentEpisodeStepCountLabel;
        private System.Windows.Forms.Label experimentTotalStepCountLabel;
        private System.Windows.Forms.Button cloneExperimentButton;
        private System.Windows.Forms.NumericUpDown cloneCountUpDown;
        private System.Windows.Forms.Label batchModeLabel;
        private System.Windows.Forms.ComboBox batchModeComboBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button saveBatchExperimentTemplateButton;
        private System.Windows.Forms.Button loadBatchExperimentTemplateButton;
        private System.Windows.Forms.Button configureExperimentButton;
        private System.Windows.Forms.Button removeExperimentButton;
    }
}