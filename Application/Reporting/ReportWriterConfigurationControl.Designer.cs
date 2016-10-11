namespace Application.Reporting
{
    partial class ReportWriterConfigurationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label reportPreviewInfoLabel;
            System.Windows.Forms.Label reportTriggerInfoLabel;
            System.Windows.Forms.Button configureReportTriggerButton;
            System.Windows.Forms.Button saveTemplateButton;
            System.Windows.Forms.Button loadTemplateButton;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.configureMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteColumnMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureHeaderTextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.reportGridView = new System.Windows.Forms.DataGridView();
            this.addNewColumnColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.previewTextBox = new System.Windows.Forms.TextBox();
            this.reportTriggerComboBox = new System.Windows.Forms.ComboBox();
            this.episodeCountCheckBox = new System.Windows.Forms.CheckBox();
            this.episodeStepCountCheckBox = new System.Windows.Forms.CheckBox();
            this.totalStepCountCheckBox = new System.Windows.Forms.CheckBox();
            this.averageReinforcementSinceCheckBox = new System.Windows.Forms.CheckBox();
            this.simpleConfigurationLabel = new System.Windows.Forms.Label();
            this.simpleConfigurationLine = new System.Windows.Forms.Label();
            this.advancedConfigurationLabel = new System.Windows.Forms.Label();
            this.advancedConfigurationLine = new System.Windows.Forms.Label();
            reportPreviewInfoLabel = new System.Windows.Forms.Label();
            reportTriggerInfoLabel = new System.Windows.Forms.Label();
            configureReportTriggerButton = new System.Windows.Forms.Button();
            saveTemplateButton = new System.Windows.Forms.Button();
            loadTemplateButton = new System.Windows.Forms.Button();
            this.gridViewMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reportGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // reportPreviewInfoLabel
            // 
            reportPreviewInfoLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            reportPreviewInfoLabel.AutoSize = true;
            reportPreviewInfoLabel.Location = new System.Drawing.Point(4, 370);
            reportPreviewInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            reportPreviewInfoLabel.Name = "reportPreviewInfoLabel";
            reportPreviewInfoLabel.Size = new System.Drawing.Size(201, 17);
            reportPreviewInfoLabel.TabIndex = 4;
            reportPreviewInfoLabel.Text = "Report preview (random data):";
            // 
            // reportTriggerInfoLabel
            // 
            reportTriggerInfoLabel.AutoSize = true;
            reportTriggerInfoLabel.Location = new System.Drawing.Point(4, 187);
            reportTriggerInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            reportTriggerInfoLabel.Name = "reportTriggerInfoLabel";
            reportTriggerInfoLabel.Size = new System.Drawing.Size(100, 17);
            reportTriggerInfoLabel.TabIndex = 5;
            reportTriggerInfoLabel.Text = "Report trigger:";
            // 
            // configureReportTriggerButton
            // 
            configureReportTriggerButton.Location = new System.Drawing.Point(357, 181);
            configureReportTriggerButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            configureReportTriggerButton.Name = "configureReportTriggerButton";
            configureReportTriggerButton.Size = new System.Drawing.Size(100, 28);
            configureReportTriggerButton.TabIndex = 7;
            configureReportTriggerButton.Text = "Configure";
            configureReportTriggerButton.UseVisualStyleBackColor = true;
            configureReportTriggerButton.Click += new System.EventHandler(this.ConfigureReportTriggerButtonClick);
            // 
            // saveTemplateButton
            // 
            saveTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            saveTemplateButton.Location = new System.Drawing.Point(8, 604);
            saveTemplateButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            saveTemplateButton.Name = "saveTemplateButton";
            saveTemplateButton.Size = new System.Drawing.Size(172, 28);
            saveTemplateButton.TabIndex = 8;
            saveTemplateButton.Text = "Save report template";
            saveTemplateButton.UseVisualStyleBackColor = true;
            saveTemplateButton.Click += new System.EventHandler(this.SaveReportTemplateButtonClick);
            // 
            // loadTemplateButton
            // 
            loadTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            loadTemplateButton.Location = new System.Drawing.Point(188, 604);
            loadTemplateButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            loadTemplateButton.Name = "loadTemplateButton";
            loadTemplateButton.Size = new System.Drawing.Size(168, 28);
            loadTemplateButton.TabIndex = 9;
            loadTemplateButton.Text = "Load report template";
            loadTemplateButton.UseVisualStyleBackColor = true;
            loadTemplateButton.Click += new System.EventHandler(this.LoadReportTemplateButtonClick);
            // 
            // configureMenuItem
            // 
            this.configureMenuItem.Name = "configureMenuItem";
            this.configureMenuItem.Size = new System.Drawing.Size(181, 22);
            this.configureMenuItem.Text = "Configure";
            this.configureMenuItem.Visible = false;
            this.configureMenuItem.Click += new System.EventHandler(this.ConfigureItemClick);
            // 
            // deleteColumnMenuItem
            // 
            this.deleteColumnMenuItem.Name = "deleteColumnMenuItem";
            this.deleteColumnMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteColumnMenuItem.Text = "Delete column";
            this.deleteColumnMenuItem.Visible = false;
            this.deleteColumnMenuItem.Click += new System.EventHandler(this.DeleteItemClick);
            // 
            // configureHeaderTextMenuItem
            // 
            this.configureHeaderTextMenuItem.Name = "configureHeaderTextMenuItem";
            this.configureHeaderTextMenuItem.Size = new System.Drawing.Size(181, 22);
            this.configureHeaderTextMenuItem.Text = "Configure header text";
            this.configureHeaderTextMenuItem.Visible = false;
            this.configureHeaderTextMenuItem.Click += new System.EventHandler(this.ConfigureHeaderTextClick);
            // 
            // gridViewMenuStrip
            // 
            this.gridViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureHeaderTextMenuItem,
            this.configureMenuItem,
            this.deleteColumnMenuItem});
            this.gridViewMenuStrip.Name = "configureMenuStrip";
            this.gridViewMenuStrip.Size = new System.Drawing.Size(182, 70);
            // 
            // reportGridView
            // 
            this.reportGridView.AllowUserToAddRows = false;
            this.reportGridView.AllowUserToDeleteRows = false;
            this.reportGridView.AllowUserToOrderColumns = true;
            this.reportGridView.AllowUserToResizeRows = false;
            this.reportGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.reportGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.reportGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.reportGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reportGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addNewColumnColumn});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.reportGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.reportGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.reportGridView.Location = new System.Drawing.Point(0, 217);
            this.reportGridView.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.reportGridView.MultiSelect = false;
            this.reportGridView.Name = "reportGridView";
            this.reportGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.reportGridView.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.reportGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.reportGridView.ShowCellErrors = false;
            this.reportGridView.ShowCellToolTips = false;
            this.reportGridView.ShowEditingIcon = false;
            this.reportGridView.ShowRowErrors = false;
            this.reportGridView.Size = new System.Drawing.Size(703, 150);
            this.reportGridView.TabIndex = 1;
            this.reportGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.GridViewCellValueChanged);
            this.reportGridView.ColumnDisplayIndexChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.GridViewColumnDisplayIndexChanged);
            this.reportGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridViewColumnHeaderMouseClick);
            this.reportGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.GridViewCurrentCellDirtyStateChanged);
            this.reportGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GridViewMouseDown);
            this.reportGridView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GridViewMouseUp);
            // 
            // addNewColumnColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.AppWorkspace;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.addNewColumnColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.addNewColumnColumn.HeaderText = "* Add new column";
            this.addNewColumnColumn.Name = "addNewColumnColumn";
            this.addNewColumnColumn.ReadOnly = true;
            this.addNewColumnColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.addNewColumnColumn.Width = 114;
            // 
            // previewTextBox
            // 
            this.previewTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewTextBox.Enabled = false;
            this.previewTextBox.Location = new System.Drawing.Point(0, 390);
            this.previewTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.previewTextBox.Multiline = true;
            this.previewTextBox.Name = "previewTextBox";
            this.previewTextBox.ReadOnly = true;
            this.previewTextBox.Size = new System.Drawing.Size(699, 206);
            this.previewTextBox.TabIndex = 2;
            this.previewTextBox.WordWrap = false;
            // 
            // reportTriggerComboBox
            // 
            this.reportTriggerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reportTriggerComboBox.FormattingEnabled = true;
            this.reportTriggerComboBox.Location = new System.Drawing.Point(103, 183);
            this.reportTriggerComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.reportTriggerComboBox.Name = "reportTriggerComboBox";
            this.reportTriggerComboBox.Size = new System.Drawing.Size(245, 24);
            this.reportTriggerComboBox.TabIndex = 6;
            this.reportTriggerComboBox.SelectedIndexChanged += new System.EventHandler(this.ReportTriggerComboBoxValueChanged);
            // 
            // episodeCountCheckBox
            // 
            this.episodeCountCheckBox.AutoSize = true;
            this.episodeCountCheckBox.Location = new System.Drawing.Point(8, 42);
            this.episodeCountCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.episodeCountCheckBox.Name = "episodeCountCheckBox";
            this.episodeCountCheckBox.Size = new System.Drawing.Size(120, 21);
            this.episodeCountCheckBox.TabIndex = 10;
            this.episodeCountCheckBox.Text = "Episode count";
            this.episodeCountCheckBox.UseVisualStyleBackColor = true;
            this.episodeCountCheckBox.CheckedChanged += new System.EventHandler(this.EpisodeCountCheckBoxCheckedChanged);
            // 
            // episodeStepCountCheckBox
            // 
            this.episodeStepCountCheckBox.AutoSize = true;
            this.episodeStepCountCheckBox.Location = new System.Drawing.Point(8, 70);
            this.episodeStepCountCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.episodeStepCountCheckBox.Name = "episodeStepCountCheckBox";
            this.episodeStepCountCheckBox.Size = new System.Drawing.Size(151, 21);
            this.episodeStepCountCheckBox.TabIndex = 11;
            this.episodeStepCountCheckBox.Text = "Episode step count";
            this.episodeStepCountCheckBox.UseVisualStyleBackColor = true;
            this.episodeStepCountCheckBox.CheckedChanged += new System.EventHandler(this.EpisodeStepCountCheckBoxCheckedChanged);
            // 
            // totalStepCountCheckBox
            // 
            this.totalStepCountCheckBox.AutoSize = true;
            this.totalStepCountCheckBox.Location = new System.Drawing.Point(8, 98);
            this.totalStepCountCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.totalStepCountCheckBox.Name = "totalStepCountCheckBox";
            this.totalStepCountCheckBox.Size = new System.Drawing.Size(132, 21);
            this.totalStepCountCheckBox.TabIndex = 12;
            this.totalStepCountCheckBox.Text = "Total step count";
            this.totalStepCountCheckBox.UseVisualStyleBackColor = true;
            this.totalStepCountCheckBox.CheckedChanged += new System.EventHandler(this.StepCountCheckBoxCheckedChanged);
            // 
            // averageReinforcementSinceCheckBox
            // 
            this.averageReinforcementSinceCheckBox.AutoSize = true;
            this.averageReinforcementSinceCheckBox.Location = new System.Drawing.Point(8, 127);
            this.averageReinforcementSinceCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.averageReinforcementSinceCheckBox.Name = "averageReinforcementSinceCheckBox";
            this.averageReinforcementSinceCheckBox.Size = new System.Drawing.Size(305, 21);
            this.averageReinforcementSinceCheckBox.TabIndex = 13;
            this.averageReinforcementSinceCheckBox.Text = "Average reinforcement since last report line";
            this.averageReinforcementSinceCheckBox.UseVisualStyleBackColor = true;
            this.averageReinforcementSinceCheckBox.CheckedChanged += new System.EventHandler(this.EpisodeAverageReinforcementCheckBoxCheckedChanged);
            // 
            // simpleConfigurationLabel
            // 
            this.simpleConfigurationLabel.AutoSize = true;
            this.simpleConfigurationLabel.Location = new System.Drawing.Point(4, 11);
            this.simpleConfigurationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.simpleConfigurationLabel.Name = "simpleConfigurationLabel";
            this.simpleConfigurationLabel.Size = new System.Drawing.Size(136, 17);
            this.simpleConfigurationLabel.TabIndex = 14;
            this.simpleConfigurationLabel.Text = "Simple configuration";
            // 
            // simpleConfigurationLine
            // 
            this.simpleConfigurationLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleConfigurationLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.simpleConfigurationLine.Location = new System.Drawing.Point(4, 30);
            this.simpleConfigurationLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.simpleConfigurationLine.Name = "simpleConfigurationLine";
            this.simpleConfigurationLine.Size = new System.Drawing.Size(693, 2);
            this.simpleConfigurationLine.TabIndex = 15;
            // 
            // advancedConfigurationLabel
            // 
            this.advancedConfigurationLabel.AutoSize = true;
            this.advancedConfigurationLabel.Location = new System.Drawing.Point(4, 156);
            this.advancedConfigurationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.advancedConfigurationLabel.Name = "advancedConfigurationLabel";
            this.advancedConfigurationLabel.Size = new System.Drawing.Size(157, 17);
            this.advancedConfigurationLabel.TabIndex = 16;
            this.advancedConfigurationLabel.Text = "Advanced configuration";
            // 
            // advancedConfigurationLine
            // 
            this.advancedConfigurationLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.advancedConfigurationLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.advancedConfigurationLine.Location = new System.Drawing.Point(7, 172);
            this.advancedConfigurationLine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.advancedConfigurationLine.Name = "advancedConfigurationLine";
            this.advancedConfigurationLine.Size = new System.Drawing.Size(693, 2);
            this.advancedConfigurationLine.TabIndex = 17;
            // 
            // ReportWriterConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.advancedConfigurationLine);
            this.Controls.Add(this.advancedConfigurationLabel);
            this.Controls.Add(this.simpleConfigurationLine);
            this.Controls.Add(this.simpleConfigurationLabel);
            this.Controls.Add(this.averageReinforcementSinceCheckBox);
            this.Controls.Add(this.totalStepCountCheckBox);
            this.Controls.Add(this.episodeStepCountCheckBox);
            this.Controls.Add(this.episodeCountCheckBox);
            this.Controls.Add(saveTemplateButton);
            this.Controls.Add(configureReportTriggerButton);
            this.Controls.Add(loadTemplateButton);
            this.Controls.Add(this.reportTriggerComboBox);
            this.Controls.Add(this.previewTextBox);
            this.Controls.Add(reportTriggerInfoLabel);
            this.Controls.Add(this.reportGridView);
            this.Controls.Add(reportPreviewInfoLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ReportWriterConfigurationControl";
            this.Size = new System.Drawing.Size(707, 636);
            this.gridViewMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reportGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView reportGridView;
        private System.Windows.Forms.TextBox previewTextBox;
        private System.Windows.Forms.ComboBox reportTriggerComboBox;
        private System.Windows.Forms.DataGridViewButtonColumn addNewColumnColumn;
        private System.Windows.Forms.ContextMenuStrip gridViewMenuStrip;
        private System.Windows.Forms.CheckBox episodeStepCountCheckBox;
        private System.Windows.Forms.CheckBox totalStepCountCheckBox;
        private System.Windows.Forms.CheckBox averageReinforcementSinceCheckBox;
        private System.Windows.Forms.Label simpleConfigurationLabel;
        private System.Windows.Forms.Label simpleConfigurationLine;
        private System.Windows.Forms.Label advancedConfigurationLabel;
        private System.Windows.Forms.Label advancedConfigurationLine;
        private System.Windows.Forms.ToolStripMenuItem configureMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteColumnMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureHeaderTextMenuItem;
        private System.Windows.Forms.CheckBox episodeCountCheckBox;
    }
}
