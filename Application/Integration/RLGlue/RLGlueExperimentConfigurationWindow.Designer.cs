namespace Application.Integration.RLGlue
{
    partial class RLGlueExperimentConfigurationWindow
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
            this.hostLabel = new System.Windows.Forms.Label();
            this.hostTextBox = new System.Windows.Forms.TextBox();
            this.hintLabel1 = new System.Windows.Forms.Label();
            this.portNumberLabel = new System.Windows.Forms.Label();
            this.portNumberTextBox = new System.Windows.Forms.TextBox();
            this.hintLabel2 = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.componentLabel = new System.Windows.Forms.Label();
            this.componentTabControl = new System.Windows.Forms.TabControl();
            this.agentsTabPage = new System.Windows.Forms.TabPage();
            this.agentListView = new System.Windows.Forms.ListView();
            this.environmentsTabPage = new System.Windows.Forms.TabPage();
            this.environmentListView = new System.Windows.Forms.ListView();
            this.componentTypeTabControl = new System.Windows.Forms.TabControl();
            this.discreteStateDiscreteActionTabPage = new System.Windows.Forms.TabPage();
            this.continuousStateDiscreteAction = new System.Windows.Forms.TabPage();
            this.continuousStateContinuousAction = new System.Windows.Forms.TabPage();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.componentTabControl.SuspendLayout();
            this.agentsTabPage.SuspendLayout();
            this.environmentsTabPage.SuspendLayout();
            this.componentTypeTabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // hostLabel
            // 
            this.hostLabel.AutoSize = true;
            this.hostLabel.Location = new System.Drawing.Point(67, 61);
            this.hostLabel.Name = "hostLabel";
            this.hostLabel.Size = new System.Drawing.Size(32, 13);
            this.hostLabel.TabIndex = 0;
            this.hostLabel.Text = "Host:";
            // 
            // hostTextBox
            // 
            this.hostTextBox.Location = new System.Drawing.Point(105, 58);
            this.hostTextBox.Name = "hostTextBox";
            this.hostTextBox.Size = new System.Drawing.Size(88, 20);
            this.hostTextBox.TabIndex = 1;
            this.hostTextBox.Text = "127.0.0.1";
            this.hostTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.HostTextBoxValidating);
            // 
            // hintLabel1
            // 
            this.hintLabel1.AutoSize = true;
            this.hintLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hintLabel1.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.hintLabel1.Location = new System.Drawing.Point(218, 48);
            this.hintLabel1.MaximumSize = new System.Drawing.Size(200, 0);
            this.hintLabel1.Name = "hintLabel1";
            this.hintLabel1.Size = new System.Drawing.Size(189, 26);
            this.hintLabel1.TabIndex = 2;
            this.hintLabel1.Text = "IPv4 number of the machine on which rl_glue.exe core application is running.";
            // 
            // portNumberLabel
            // 
            this.portNumberLabel.AutoSize = true;
            this.portNumberLabel.Location = new System.Drawing.Point(32, 104);
            this.portNumberLabel.Name = "portNumberLabel";
            this.portNumberLabel.Size = new System.Drawing.Size(67, 13);
            this.portNumberLabel.TabIndex = 3;
            this.portNumberLabel.Text = "Port number:";
            // 
            // portNumberTextBox
            // 
            this.portNumberTextBox.Location = new System.Drawing.Point(105, 101);
            this.portNumberTextBox.Name = "portNumberTextBox";
            this.portNumberTextBox.Size = new System.Drawing.Size(59, 20);
            this.portNumberTextBox.TabIndex = 4;
            this.portNumberTextBox.Text = "4096";
            this.portNumberTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.PortNumberTextBoxValidating);
            // 
            // hintLabel2
            // 
            this.hintLabel2.AutoSize = true;
            this.hintLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.hintLabel2.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.hintLabel2.Location = new System.Drawing.Point(218, 92);
            this.hintLabel2.MaximumSize = new System.Drawing.Size(200, 0);
            this.hintLabel2.Name = "hintLabel2";
            this.hintLabel2.Size = new System.Drawing.Size(199, 39);
            this.hintLabel2.TabIndex = 5;
            this.hintLabel2.Text = "Number of port on which rl_glue.exe core application is configured to listen for " +
    "incoming connections. Usually 4096.";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(405, 13);
            this.titleLabel.TabIndex = 6;
            this.titleLabel.Text = "Choose connection parameters and a component to expose to running RLGlue core:";
            // 
            // componentLabel
            // 
            this.componentLabel.AutoSize = true;
            this.componentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.componentLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.componentLabel.Location = new System.Drawing.Point(35, 146);
            this.componentLabel.MaximumSize = new System.Drawing.Size(200, 0);
            this.componentLabel.Name = "componentLabel";
            this.componentLabel.Size = new System.Drawing.Size(64, 13);
            this.componentLabel.TabIndex = 9;
            this.componentLabel.Text = "Component:";
            // 
            // componentTabControl
            // 
            this.componentTabControl.Controls.Add(this.agentsTabPage);
            this.componentTabControl.Controls.Add(this.environmentsTabPage);
            this.componentTabControl.Location = new System.Drawing.Point(105, 168);
            this.componentTabControl.Name = "componentTabControl";
            this.componentTabControl.SelectedIndex = 0;
            this.componentTabControl.Size = new System.Drawing.Size(496, 161);
            this.componentTabControl.TabIndex = 10;
            this.componentTabControl.Validating += new System.ComponentModel.CancelEventHandler(this.ComponentTabControlValidating);
            // 
            // agentsTabPage
            // 
            this.agentsTabPage.Controls.Add(this.agentListView);
            this.agentsTabPage.Location = new System.Drawing.Point(4, 22);
            this.agentsTabPage.Name = "agentsTabPage";
            this.agentsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.agentsTabPage.Size = new System.Drawing.Size(488, 135);
            this.agentsTabPage.TabIndex = 0;
            this.agentsTabPage.Text = "Agent";
            this.agentsTabPage.UseVisualStyleBackColor = true;
            // 
            // agentListView
            // 
            this.agentListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.agentListView.LabelWrap = false;
            this.agentListView.Location = new System.Drawing.Point(3, 3);
            this.agentListView.MultiSelect = false;
            this.agentListView.Name = "agentListView";
            this.agentListView.ShowGroups = false;
            this.agentListView.Size = new System.Drawing.Size(482, 129);
            this.agentListView.TabIndex = 0;
            this.agentListView.UseCompatibleStateImageBehavior = false;
            this.agentListView.View = System.Windows.Forms.View.Tile;
            // 
            // environmentsTabPage
            // 
            this.environmentsTabPage.Controls.Add(this.environmentListView);
            this.environmentsTabPage.Location = new System.Drawing.Point(4, 22);
            this.environmentsTabPage.Name = "environmentsTabPage";
            this.environmentsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.environmentsTabPage.Size = new System.Drawing.Size(488, 135);
            this.environmentsTabPage.TabIndex = 1;
            this.environmentsTabPage.Text = "Environment";
            this.environmentsTabPage.UseVisualStyleBackColor = true;
            // 
            // environmentListView
            // 
            this.environmentListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.environmentListView.LabelWrap = false;
            this.environmentListView.Location = new System.Drawing.Point(3, 3);
            this.environmentListView.MultiSelect = false;
            this.environmentListView.Name = "environmentListView";
            this.environmentListView.ShowGroups = false;
            this.environmentListView.Size = new System.Drawing.Size(482, 129);
            this.environmentListView.TabIndex = 0;
            this.environmentListView.UseCompatibleStateImageBehavior = false;
            this.environmentListView.View = System.Windows.Forms.View.Tile;
            // 
            // componentTypeTabControl
            // 
            this.componentTypeTabControl.Controls.Add(this.discreteStateDiscreteActionTabPage);
            this.componentTypeTabControl.Controls.Add(this.continuousStateDiscreteAction);
            this.componentTypeTabControl.Controls.Add(this.continuousStateContinuousAction);
            this.componentTypeTabControl.Location = new System.Drawing.Point(105, 146);
            this.componentTypeTabControl.Name = "componentTypeTabControl";
            this.componentTypeTabControl.SelectedIndex = 0;
            this.componentTypeTabControl.Size = new System.Drawing.Size(496, 24);
            this.componentTypeTabControl.TabIndex = 0;
            this.componentTypeTabControl.SelectedIndexChanged += new System.EventHandler(this.ComponentTypeTabControlSelectedIndexChanged);
            // 
            // discreteStateDiscreteActionTabPage
            // 
            this.discreteStateDiscreteActionTabPage.Location = new System.Drawing.Point(4, 22);
            this.discreteStateDiscreteActionTabPage.Name = "discreteStateDiscreteActionTabPage";
            this.discreteStateDiscreteActionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.discreteStateDiscreteActionTabPage.Size = new System.Drawing.Size(488, 0);
            this.discreteStateDiscreteActionTabPage.TabIndex = 0;
            this.discreteStateDiscreteActionTabPage.Text = "Discrete state, discrete action";
            this.discreteStateDiscreteActionTabPage.UseVisualStyleBackColor = true;
            // 
            // continuousStateDiscreteAction
            // 
            this.continuousStateDiscreteAction.Location = new System.Drawing.Point(4, 22);
            this.continuousStateDiscreteAction.Name = "continuousStateDiscreteAction";
            this.continuousStateDiscreteAction.Padding = new System.Windows.Forms.Padding(3);
            this.continuousStateDiscreteAction.Size = new System.Drawing.Size(488, 0);
            this.continuousStateDiscreteAction.TabIndex = 1;
            this.continuousStateDiscreteAction.Text = "Continuous state, discrete action";
            this.continuousStateDiscreteAction.UseVisualStyleBackColor = true;
            // 
            // continuousStateContinuousAction
            // 
            this.continuousStateContinuousAction.Location = new System.Drawing.Point(4, 22);
            this.continuousStateContinuousAction.Name = "continuousStateContinuousAction";
            this.continuousStateContinuousAction.Size = new System.Drawing.Size(488, 0);
            this.continuousStateContinuousAction.TabIndex = 2;
            this.continuousStateContinuousAction.Text = "Continous state, continuous action";
            this.continuousStateContinuousAction.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(526, 336);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 11;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.CausesValidation = false;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(12, 336);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 12;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // RLGlueExperimentConfigurationWindow
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(623, 371);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.componentTabControl);
            this.Controls.Add(this.componentTypeTabControl);
            this.Controls.Add(this.componentLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.hintLabel2);
            this.Controls.Add(this.portNumberTextBox);
            this.Controls.Add(this.portNumberLabel);
            this.Controls.Add(this.hintLabel1);
            this.Controls.Add(this.hostTextBox);
            this.Controls.Add(this.hostLabel);
            this.Name = "RLGlueExperimentConfigurationWindow";
            this.Text = "Configure connection to RLGlue experiment";
            this.componentTabControl.ResumeLayout(false);
            this.agentsTabPage.ResumeLayout(false);
            this.environmentsTabPage.ResumeLayout(false);
            this.componentTypeTabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label hostLabel;
        private System.Windows.Forms.TextBox hostTextBox;
        private System.Windows.Forms.Label hintLabel1;
        private System.Windows.Forms.Label portNumberLabel;
        private System.Windows.Forms.TextBox portNumberTextBox;
        private System.Windows.Forms.Label hintLabel2;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label componentLabel;
        private System.Windows.Forms.TabControl componentTabControl;
        private System.Windows.Forms.TabPage agentsTabPage;
        private System.Windows.Forms.TabPage environmentsTabPage;
        private System.Windows.Forms.TabControl componentTypeTabControl;
        private System.Windows.Forms.TabPage discreteStateDiscreteActionTabPage;
        private System.Windows.Forms.TabPage continuousStateDiscreteAction;
        private System.Windows.Forms.TabPage continuousStateContinuousAction;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListView agentListView;
        private System.Windows.Forms.ListView environmentListView;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}