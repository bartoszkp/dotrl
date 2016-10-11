namespace Application
{
    partial class BatchExperimentWindow
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
            System.Windows.Forms.Label batchModeLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchExperimentWindow));
            this.cancelButton = new System.Windows.Forms.Button();
            this.startAllButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.experimentListView = new System.Windows.Forms.ListView();
            this.progressColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.environmentColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.agentColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.experimentEpisodeCountColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.experimentEpisodeStepCountColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.experimentTotalStepCountLimitColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.experimentImageList = new System.Windows.Forms.ImageList(this.components);
            this.totalProgressBar = new System.Windows.Forms.ProgressBar();
            this.batchModeComboBox = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.pauseAllButton = new System.Windows.Forms.Button();
            this.startSelectedButton = new System.Windows.Forms.Button();
            this.pauseSelectedButton = new System.Windows.Forms.Button();
            this.viewSelectedButton = new System.Windows.Forms.Button();
            batchModeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // batchModeLabel
            // 
            batchModeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            batchModeLabel.AutoSize = true;
            batchModeLabel.Location = new System.Drawing.Point(-2, 466);
            batchModeLabel.Name = "batchModeLabel";
            batchModeLabel.Size = new System.Drawing.Size(67, 13);
            batchModeLabel.TabIndex = 5;
            batchModeLabel.Text = "Batch mode:";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(781, 453);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // startAllButton
            // 
            this.startAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.startAllButton.Location = new System.Drawing.Point(1, 417);
            this.startAllButton.Name = "startAllButton";
            this.startAllButton.Size = new System.Drawing.Size(75, 23);
            this.startAllButton.TabIndex = 7;
            this.startAllButton.Text = "Start all";
            this.startAllButton.UseVisualStyleBackColor = true;
            this.startAllButton.Click += new System.EventHandler(this.StartButtonClick);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(700, 453);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // experimentListView
            // 
            this.experimentListView.AutoArrange = false;
            this.experimentListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.progressColumn,
            this.environmentColumn,
            this.agentColumn,
            this.experimentEpisodeCountColumn,
            this.experimentEpisodeStepCountColumn,
            this.experimentTotalStepCountLimitColumn,
            this.statusColumn});
            this.experimentListView.Dock = System.Windows.Forms.DockStyle.Top;
            this.experimentListView.FullRowSelect = true;
            this.experimentListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.experimentListView.Location = new System.Drawing.Point(0, 0);
            this.experimentListView.Name = "experimentListView";
            this.experimentListView.Size = new System.Drawing.Size(868, 382);
            this.experimentListView.SmallImageList = this.experimentImageList;
            this.experimentListView.TabIndex = 2;
            this.experimentListView.TileSize = new System.Drawing.Size(400, 200);
            this.experimentListView.UseCompatibleStateImageBehavior = false;
            this.experimentListView.View = System.Windows.Forms.View.Details;
            this.experimentListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ExperimentListViewItemSelectionChanged);
            // 
            // progressColumn
            // 
            this.progressColumn.Text = "Progress";
            this.progressColumn.Width = 64;
            // 
            // environmentColumn
            // 
            this.environmentColumn.Text = "Environment";
            this.environmentColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.environmentColumn.Width = 113;
            // 
            // agentColumn
            // 
            this.agentColumn.Text = "Agent";
            this.agentColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.agentColumn.Width = 105;
            // 
            // experimentEpisodeCountColumn
            // 
            this.experimentEpisodeCountColumn.Text = "Episode count limit";
            this.experimentEpisodeCountColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.experimentEpisodeCountColumn.Width = 104;
            // 
            // experimentEpisodeStepCountColumn
            // 
            this.experimentEpisodeStepCountColumn.Text = "Episode step count limit";
            this.experimentEpisodeStepCountColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.experimentEpisodeStepCountColumn.Width = 128;
            // 
            // experimentTotalStepCountLimitColumn
            // 
            this.experimentTotalStepCountLimitColumn.Text = "Total step count limit";
            this.experimentTotalStepCountLimitColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.experimentTotalStepCountLimitColumn.Width = 114;
            // 
            // statusColumn
            // 
            this.statusColumn.Text = "Status";
            this.statusColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // experimentImageList
            // 
            this.experimentImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.experimentImageList.ImageSize = new System.Drawing.Size(150, 20);
            this.experimentImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // totalProgressBar
            // 
            this.totalProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.totalProgressBar.Location = new System.Drawing.Point(0, 388);
            this.totalProgressBar.Name = "totalProgressBar";
            this.totalProgressBar.Size = new System.Drawing.Size(868, 23);
            this.totalProgressBar.TabIndex = 3;
            // 
            // batchModeComboBox
            // 
            this.batchModeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.batchModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.batchModeComboBox.FormattingEnabled = true;
            this.batchModeComboBox.Location = new System.Drawing.Point(71, 463);
            this.batchModeComboBox.Name = "batchModeComboBox";
            this.batchModeComboBox.Size = new System.Drawing.Size(121, 21);
            this.batchModeComboBox.TabIndex = 4;
            this.batchModeComboBox.SelectedIndexChanged += new System.EventHandler(this.BatchModeComboBoxSelectedIndexChanged);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 200;
            this.timer.Tick += new System.EventHandler(this.TimerTick);
            // 
            // pauseAllButton
            // 
            this.pauseAllButton.Location = new System.Drawing.Point(82, 417);
            this.pauseAllButton.Name = "pauseAllButton";
            this.pauseAllButton.Size = new System.Drawing.Size(75, 23);
            this.pauseAllButton.TabIndex = 8;
            this.pauseAllButton.Text = "Pause all";
            this.pauseAllButton.UseVisualStyleBackColor = true;
            this.pauseAllButton.Click += new System.EventHandler(this.PauseButtonClick);
            // 
            // startSelectedButton
            // 
            this.startSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startSelectedButton.Location = new System.Drawing.Point(278, 417);
            this.startSelectedButton.Name = "startSelectedButton";
            this.startSelectedButton.Size = new System.Drawing.Size(105, 23);
            this.startSelectedButton.TabIndex = 9;
            this.startSelectedButton.Text = "Start selected";
            this.startSelectedButton.UseVisualStyleBackColor = true;
            this.startSelectedButton.Click += new System.EventHandler(this.StartSelectedButtonClick);
            // 
            // pauseSelectedButton
            // 
            this.pauseSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pauseSelectedButton.Location = new System.Drawing.Point(389, 417);
            this.pauseSelectedButton.Name = "pauseSelectedButton";
            this.pauseSelectedButton.Size = new System.Drawing.Size(99, 23);
            this.pauseSelectedButton.TabIndex = 10;
            this.pauseSelectedButton.Text = "Pause selected";
            this.pauseSelectedButton.UseVisualStyleBackColor = true;
            this.pauseSelectedButton.Click += new System.EventHandler(this.PauseSelectedButtonClick);
            // 
            // viewSelectedButton
            // 
            this.viewSelectedButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewSelectedButton.Location = new System.Drawing.Point(494, 417);
            this.viewSelectedButton.Name = "viewSelectedButton";
            this.viewSelectedButton.Size = new System.Drawing.Size(97, 23);
            this.viewSelectedButton.TabIndex = 11;
            this.viewSelectedButton.Text = "View selected";
            this.viewSelectedButton.UseVisualStyleBackColor = true;
            this.viewSelectedButton.Click += new System.EventHandler(this.ViewSelectedButtonClick);
            // 
            // BatchExperimentWindow
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 488);
            this.Controls.Add(this.viewSelectedButton);
            this.Controls.Add(this.pauseSelectedButton);
            this.Controls.Add(this.startSelectedButton);
            this.Controls.Add(this.pauseAllButton);
            this.Controls.Add(this.startAllButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(batchModeLabel);
            this.Controls.Add(this.batchModeComboBox);
            this.Controls.Add(this.totalProgressBar);
            this.Controls.Add(this.experimentListView);
            this.Controls.Add(this.okButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BatchExperimentWindow";
            this.Text = "BatchExperimentWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView experimentListView;
        private System.Windows.Forms.ImageList experimentImageList;
        private System.Windows.Forms.ColumnHeader environmentColumn;
        private System.Windows.Forms.ColumnHeader agentColumn;
        private System.Windows.Forms.ColumnHeader experimentEpisodeCountColumn;
        private System.Windows.Forms.ColumnHeader experimentEpisodeStepCountColumn;
        private System.Windows.Forms.ColumnHeader experimentTotalStepCountLimitColumn;
        private System.Windows.Forms.ColumnHeader progressColumn;
        private System.Windows.Forms.ColumnHeader statusColumn;
        private System.Windows.Forms.ProgressBar totalProgressBar;
        private System.Windows.Forms.ComboBox batchModeComboBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button startAllButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button pauseAllButton;
        private System.Windows.Forms.Button startSelectedButton;
        private System.Windows.Forms.Button pauseSelectedButton;
        private System.Windows.Forms.Button viewSelectedButton;
    }
}