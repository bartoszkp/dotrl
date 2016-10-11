namespace Application
{
    partial class ExperimentSetupWindow
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
            System.Windows.Forms.TabPage agentEnvironmentTabPage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExperimentSetupWindow));
            this.agentPanel = new System.Windows.Forms.Panel();
            this.environmentPanel = new System.Windows.Forms.Panel();
            this.agentLabel = new System.Windows.Forms.Label();
            this.environmentLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.reportingTabPage = new System.Windows.Forms.TabPage();
            this.reportingConfigurationControl = new Application.Reporting.ReportingConfigurationControl();
            this.experimentParametersTabPage = new System.Windows.Forms.TabPage();
            this.experimentParametersApplyButton = new System.Windows.Forms.Button();
            this.experimentParametersRevertButton = new System.Windows.Forms.Button();
            this.experimentParameterControl = new Application.Parameters.ParameterControl();
            this.agentParametersRevertButton = new System.Windows.Forms.Button();
            this.agentParametersApplyButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.environmentParametersTabPage = new System.Windows.Forms.TabPage();
            this.environmentParametersApplyButton = new System.Windows.Forms.Button();
            this.environmentParametersRevertButton = new System.Windows.Forms.Button();
            this.environmentParameterControl = new Application.Parameters.ParameterControl();
            this.agentParametersTabPage = new System.Windows.Forms.TabPage();
            this.agentParameterControl = new Application.Parameters.ParameterControl();
            this.okButton = new System.Windows.Forms.Button();
            this.saveExperimentTemplateButton = new System.Windows.Forms.Button();
            this.loadExperimentTemplateButton = new System.Windows.Forms.Button();
            agentEnvironmentTabPage = new System.Windows.Forms.TabPage();
            agentEnvironmentTabPage.SuspendLayout();
            this.reportingTabPage.SuspendLayout();
            this.experimentParametersTabPage.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.environmentParametersTabPage.SuspendLayout();
            this.agentParametersTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // agentEnvironmentTabPage
            // 
            agentEnvironmentTabPage.Controls.Add(this.agentPanel);
            agentEnvironmentTabPage.Controls.Add(this.environmentPanel);
            agentEnvironmentTabPage.Controls.Add(this.agentLabel);
            agentEnvironmentTabPage.Controls.Add(this.environmentLabel);
            agentEnvironmentTabPage.Location = new System.Drawing.Point(4, 22);
            agentEnvironmentTabPage.Name = "agentEnvironmentTabPage";
            agentEnvironmentTabPage.Padding = new System.Windows.Forms.Padding(3);
            agentEnvironmentTabPage.Size = new System.Drawing.Size(1000, 619);
            agentEnvironmentTabPage.TabIndex = 0;
            agentEnvironmentTabPage.Text = "Agent and Environment";
            agentEnvironmentTabPage.UseVisualStyleBackColor = true;
            // 
            // agentPanel
            // 
            this.agentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.agentPanel.Location = new System.Drawing.Point(510, 19);
            this.agentPanel.Name = "agentPanel";
            this.agentPanel.Size = new System.Drawing.Size(484, 594);
            this.agentPanel.TabIndex = 9;
            // 
            // environmentPanel
            // 
            this.environmentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.environmentPanel.Location = new System.Drawing.Point(9, 19);
            this.environmentPanel.Name = "environmentPanel";
            this.environmentPanel.Size = new System.Drawing.Size(495, 594);
            this.environmentPanel.TabIndex = 12;
            // 
            // agentLabel
            // 
            this.agentLabel.AutoSize = true;
            this.agentLabel.Location = new System.Drawing.Point(507, 3);
            this.agentLabel.Name = "agentLabel";
            this.agentLabel.Size = new System.Drawing.Size(199, 13);
            this.agentLabel.TabIndex = 11;
            this.agentLabel.Text = "Agents available for chosen environment";
            // 
            // environmentLabel
            // 
            this.environmentLabel.AutoSize = true;
            this.environmentLabel.Location = new System.Drawing.Point(6, 3);
            this.environmentLabel.Name = "environmentLabel";
            this.environmentLabel.Size = new System.Drawing.Size(71, 13);
            this.environmentLabel.TabIndex = 10;
            this.environmentLabel.Text = "Environments";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(945, 663);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // reportingTabPage
            // 
            this.reportingTabPage.Controls.Add(this.reportingConfigurationControl);
            this.reportingTabPage.Location = new System.Drawing.Point(4, 22);
            this.reportingTabPage.Name = "reportingTabPage";
            this.reportingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.reportingTabPage.Size = new System.Drawing.Size(1000, 619);
            this.reportingTabPage.TabIndex = 1;
            this.reportingTabPage.Text = "Reporting";
            this.reportingTabPage.UseVisualStyleBackColor = true;
            // 
            // reportingConfigurationControl
            // 
            this.reportingConfigurationControl.AutoSize = true;
            this.reportingConfigurationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportingConfigurationControl.Experiment = null;
            this.reportingConfigurationControl.Location = new System.Drawing.Point(3, 3);
            this.reportingConfigurationControl.Margin = new System.Windows.Forms.Padding(4);
            this.reportingConfigurationControl.Name = "reportingConfigurationControl";
            this.reportingConfigurationControl.Size = new System.Drawing.Size(994, 613);
            this.reportingConfigurationControl.TabIndex = 0;
            // 
            // experimentParametersTabPage
            // 
            this.experimentParametersTabPage.Controls.Add(this.experimentParametersApplyButton);
            this.experimentParametersTabPage.Controls.Add(this.experimentParametersRevertButton);
            this.experimentParametersTabPage.Controls.Add(this.experimentParameterControl);
            this.experimentParametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.experimentParametersTabPage.Name = "experimentParametersTabPage";
            this.experimentParametersTabPage.Size = new System.Drawing.Size(1000, 619);
            this.experimentParametersTabPage.TabIndex = 4;
            this.experimentParametersTabPage.Text = "Experiment parameters";
            this.experimentParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // experimentParametersApplyButton
            // 
            this.experimentParametersApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentParametersApplyButton.Location = new System.Drawing.Point(797, 593);
            this.experimentParametersApplyButton.Name = "experimentParametersApplyButton";
            this.experimentParametersApplyButton.Size = new System.Drawing.Size(75, 23);
            this.experimentParametersApplyButton.TabIndex = 2;
            this.experimentParametersApplyButton.Text = "Apply";
            this.experimentParametersApplyButton.UseVisualStyleBackColor = true;
            this.experimentParametersApplyButton.Click += new System.EventHandler(this.ExperimentParametersApplyButtonClick);
            // 
            // experimentParametersRevertButton
            // 
            this.experimentParametersRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentParametersRevertButton.CausesValidation = false;
            this.experimentParametersRevertButton.Location = new System.Drawing.Point(878, 593);
            this.experimentParametersRevertButton.Name = "experimentParametersRevertButton";
            this.experimentParametersRevertButton.Size = new System.Drawing.Size(75, 23);
            this.experimentParametersRevertButton.TabIndex = 1;
            this.experimentParametersRevertButton.Text = "Revert";
            this.experimentParametersRevertButton.UseVisualStyleBackColor = true;
            this.experimentParametersRevertButton.Click += new System.EventHandler(this.ExperimentParametersRevertButtonClick);
            // 
            // experimentParameterControl
            // 
            this.experimentParameterControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.experimentParameterControl.CausesValidation = false;
            this.experimentParameterControl.Location = new System.Drawing.Point(3, 3);
            this.experimentParameterControl.Margin = new System.Windows.Forms.Padding(4);
            this.experimentParameterControl.Name = "experimentParameterControl";
            this.experimentParameterControl.OwnerComponentFieldName = null;
            this.experimentParameterControl.OwnerComponentName = null;
            this.experimentParameterControl.Size = new System.Drawing.Size(993, 583);
            this.experimentParameterControl.TabIndex = 0;
            this.experimentParameterControl.ParameterValueEdited += new System.EventHandler(this.ExperimentParameterControlParameterValueEdited);
            // 
            // agentParametersRevertButton
            // 
            this.agentParametersRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.agentParametersRevertButton.CausesValidation = false;
            this.agentParametersRevertButton.Location = new System.Drawing.Point(892, 593);
            this.agentParametersRevertButton.Name = "agentParametersRevertButton";
            this.agentParametersRevertButton.Size = new System.Drawing.Size(75, 23);
            this.agentParametersRevertButton.TabIndex = 1;
            this.agentParametersRevertButton.Text = "Revert";
            this.agentParametersRevertButton.UseVisualStyleBackColor = true;
            this.agentParametersRevertButton.Click += new System.EventHandler(this.AgentParametersRevertButtonClick);
            // 
            // agentParametersApplyButton
            // 
            this.agentParametersApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.agentParametersApplyButton.Location = new System.Drawing.Point(811, 593);
            this.agentParametersApplyButton.Name = "agentParametersApplyButton";
            this.agentParametersApplyButton.Size = new System.Drawing.Size(75, 23);
            this.agentParametersApplyButton.TabIndex = 2;
            this.agentParametersApplyButton.Text = "Apply";
            this.agentParametersApplyButton.UseVisualStyleBackColor = true;
            this.agentParametersApplyButton.Click += new System.EventHandler(this.AgentParametersApplyButtonClick);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(agentEnvironmentTabPage);
            this.tabControl.Controls.Add(this.environmentParametersTabPage);
            this.tabControl.Controls.Add(this.agentParametersTabPage);
            this.tabControl.Controls.Add(this.experimentParametersTabPage);
            this.tabControl.Controls.Add(this.reportingTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1008, 645);
            this.tabControl.TabIndex = 0;
            // 
            // environmentParametersTabPage
            // 
            this.environmentParametersTabPage.Controls.Add(this.environmentParametersApplyButton);
            this.environmentParametersTabPage.Controls.Add(this.environmentParametersRevertButton);
            this.environmentParametersTabPage.Controls.Add(this.environmentParameterControl);
            this.environmentParametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.environmentParametersTabPage.Name = "environmentParametersTabPage";
            this.environmentParametersTabPage.Size = new System.Drawing.Size(1000, 619);
            this.environmentParametersTabPage.TabIndex = 2;
            this.environmentParametersTabPage.Text = "Environment parameters";
            this.environmentParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // environmentParametersApplyButton
            // 
            this.environmentParametersApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentParametersApplyButton.Location = new System.Drawing.Point(813, 593);
            this.environmentParametersApplyButton.Name = "environmentParametersApplyButton";
            this.environmentParametersApplyButton.Size = new System.Drawing.Size(75, 23);
            this.environmentParametersApplyButton.TabIndex = 2;
            this.environmentParametersApplyButton.Text = "Apply";
            this.environmentParametersApplyButton.UseVisualStyleBackColor = true;
            this.environmentParametersApplyButton.Click += new System.EventHandler(this.EnvironmentParametersApplyButtonClick);
            // 
            // environmentParametersRevertButton
            // 
            this.environmentParametersRevertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentParametersRevertButton.CausesValidation = false;
            this.environmentParametersRevertButton.Location = new System.Drawing.Point(894, 593);
            this.environmentParametersRevertButton.Name = "environmentParametersRevertButton";
            this.environmentParametersRevertButton.Size = new System.Drawing.Size(75, 23);
            this.environmentParametersRevertButton.TabIndex = 1;
            this.environmentParametersRevertButton.Text = "Revert";
            this.environmentParametersRevertButton.UseVisualStyleBackColor = true;
            this.environmentParametersRevertButton.Click += new System.EventHandler(this.EnvironmentParametersRevertButtonClick);
            // 
            // environmentParameterControl
            // 
            this.environmentParameterControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.environmentParameterControl.CausesValidation = false;
            this.environmentParameterControl.Location = new System.Drawing.Point(3, 3);
            this.environmentParameterControl.Margin = new System.Windows.Forms.Padding(4);
            this.environmentParameterControl.Name = "environmentParameterControl";
            this.environmentParameterControl.OwnerComponentFieldName = null;
            this.environmentParameterControl.OwnerComponentName = null;
            this.environmentParameterControl.Size = new System.Drawing.Size(993, 583);
            this.environmentParameterControl.TabIndex = 0;
            this.environmentParameterControl.ParameterValueEdited += new System.EventHandler(this.EnvironmentParameterControlParameterValueEdited);
            this.environmentParameterControl.ParametersChanged += new System.EventHandler(this.EnvironmentParameterControlParametersChanged);
            // 
            // agentParametersTabPage
            // 
            this.agentParametersTabPage.Controls.Add(this.agentParametersApplyButton);
            this.agentParametersTabPage.Controls.Add(this.agentParametersRevertButton);
            this.agentParametersTabPage.Controls.Add(this.agentParameterControl);
            this.agentParametersTabPage.Location = new System.Drawing.Point(4, 22);
            this.agentParametersTabPage.Name = "agentParametersTabPage";
            this.agentParametersTabPage.Size = new System.Drawing.Size(1000, 619);
            this.agentParametersTabPage.TabIndex = 3;
            this.agentParametersTabPage.Text = "Agent parameters";
            this.agentParametersTabPage.UseVisualStyleBackColor = true;
            // 
            // agentParameterControl
            // 
            this.agentParameterControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agentParameterControl.CausesValidation = false;
            this.agentParameterControl.Location = new System.Drawing.Point(3, 3);
            this.agentParameterControl.Margin = new System.Windows.Forms.Padding(4);
            this.agentParameterControl.Name = "agentParameterControl";
            this.agentParameterControl.OwnerComponentFieldName = null;
            this.agentParameterControl.OwnerComponentName = null;
            this.agentParameterControl.Size = new System.Drawing.Size(993, 583);
            this.agentParameterControl.TabIndex = 0;
            this.agentParameterControl.ParameterValueEdited += new System.EventHandler(this.AgentParameterControlParameterValueEdited);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(864, 663);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // saveExperimentTemplateButton
            // 
            this.saveExperimentTemplateButton.Location = new System.Drawing.Point(12, 663);
            this.saveExperimentTemplateButton.Name = "saveExperimentTemplateButton";
            this.saveExperimentTemplateButton.Size = new System.Drawing.Size(153, 23);
            this.saveExperimentTemplateButton.TabIndex = 3;
            this.saveExperimentTemplateButton.Text = "Save experiment template";
            this.saveExperimentTemplateButton.UseVisualStyleBackColor = true;
            this.saveExperimentTemplateButton.Click += new System.EventHandler(this.SaveExperimentTemplateButtonClick);
            // 
            // loadExperimentTemplateButton
            // 
            this.loadExperimentTemplateButton.Location = new System.Drawing.Point(171, 663);
            this.loadExperimentTemplateButton.Name = "loadExperimentTemplateButton";
            this.loadExperimentTemplateButton.Size = new System.Drawing.Size(151, 23);
            this.loadExperimentTemplateButton.TabIndex = 4;
            this.loadExperimentTemplateButton.Text = "Load experiment template";
            this.loadExperimentTemplateButton.UseVisualStyleBackColor = true;
            this.loadExperimentTemplateButton.Click += new System.EventHandler(this.LoadExperimentTemplateButtonClick);
            // 
            // ExperimentSetupWindow
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1032, 698);
            this.Controls.Add(this.loadExperimentTemplateButton);
            this.Controls.Add(this.saveExperimentTemplateButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExperimentSetupWindow";
            this.Text = "ExperimentSetupWindow";
            agentEnvironmentTabPage.ResumeLayout(false);
            agentEnvironmentTabPage.PerformLayout();
            this.reportingTabPage.ResumeLayout(false);
            this.reportingTabPage.PerformLayout();
            this.experimentParametersTabPage.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.environmentParametersTabPage.ResumeLayout(false);
            this.agentParametersTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Panel agentPanel;
        private System.Windows.Forms.Label environmentLabel;
        private System.Windows.Forms.Label agentLabel;
        private System.Windows.Forms.TabPage environmentParametersTabPage;
        private System.Windows.Forms.TabPage agentParametersTabPage;
        private System.Windows.Forms.Button environmentParametersApplyButton;
        private System.Windows.Forms.Button environmentParametersRevertButton;
        private Parameters.ParameterControl environmentParameterControl;
        private Parameters.ParameterControl agentParameterControl;
        private System.Windows.Forms.Button agentParametersRevertButton;
        private System.Windows.Forms.Button agentParametersApplyButton;
        private System.Windows.Forms.Panel environmentPanel;
        private Parameters.ParameterControl experimentParameterControl;
        private System.Windows.Forms.Button experimentParametersRevertButton;
        private System.Windows.Forms.Button experimentParametersApplyButton;
        private Reporting.ReportingConfigurationControl reportingConfigurationControl;
        private System.Windows.Forms.TabPage reportingTabPage;
        private System.Windows.Forms.TabPage experimentParametersTabPage;
        private System.Windows.Forms.Button saveExperimentTemplateButton;
        private System.Windows.Forms.Button loadExperimentTemplateButton;
        private System.Windows.Forms.Button cancelButton;
    }
}