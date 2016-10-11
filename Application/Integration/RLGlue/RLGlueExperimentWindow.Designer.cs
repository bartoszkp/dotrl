namespace Application.Integration.RLGlue
{
    partial class RLGlueExperimentWindow
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.connectionStatusLabel = new System.Windows.Forms.Label();
            this.currentRewardLabel = new System.Windows.Forms.Label();
            this.averageRewardLabel = new System.Windows.Forms.Label();
            this.episodeAverageRewardLabel = new System.Windows.Forms.Label();
            this.finishButton = new System.Windows.Forms.Button();
            this.connectionStatusTextBox = new System.Windows.Forms.TextBox();
            this.rlGlueConnectionStateLabel = new System.Windows.Forms.Label();
            this.rlGlueConnectionStateTextBox = new System.Windows.Forms.TextBox();
            this.currentRewardTextBox = new System.Windows.Forms.TextBox();
            this.averageRewardTextBox = new System.Windows.Forms.TextBox();
            this.episodeAverageRewardTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.titleLabel.Location = new System.Drawing.Point(12, 9);
            this.titleLabel.MaximumSize = new System.Drawing.Size(400, 100);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(364, 42);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "[Title]";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.AutoSize = true;
            this.connectionStatusLabel.Location = new System.Drawing.Point(9, 54);
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(95, 13);
            this.connectionStatusLabel.TabIndex = 1;
            this.connectionStatusLabel.Text = "Connection status:";
            // 
            // currentRewardLabel
            // 
            this.currentRewardLabel.AutoSize = true;
            this.currentRewardLabel.Location = new System.Drawing.Point(9, 93);
            this.currentRewardLabel.Name = "currentRewardLabel";
            this.currentRewardLabel.Size = new System.Drawing.Size(79, 13);
            this.currentRewardLabel.TabIndex = 2;
            this.currentRewardLabel.Text = "Current reward:";
            // 
            // averageRewardLabel
            // 
            this.averageRewardLabel.AutoSize = true;
            this.averageRewardLabel.Location = new System.Drawing.Point(9, 112);
            this.averageRewardLabel.Name = "averageRewardLabel";
            this.averageRewardLabel.Size = new System.Drawing.Size(85, 13);
            this.averageRewardLabel.TabIndex = 3;
            this.averageRewardLabel.Text = "Average reward:";
            // 
            // episodeAverageRewardLabel
            // 
            this.episodeAverageRewardLabel.AutoSize = true;
            this.episodeAverageRewardLabel.Location = new System.Drawing.Point(9, 131);
            this.episodeAverageRewardLabel.Name = "episodeAverageRewardLabel";
            this.episodeAverageRewardLabel.Size = new System.Drawing.Size(125, 13);
            this.episodeAverageRewardLabel.TabIndex = 4;
            this.episodeAverageRewardLabel.Text = "Episode average reward:";
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.Location = new System.Drawing.Point(158, 184);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 23);
            this.finishButton.TabIndex = 5;
            this.finishButton.Text = "[Finish]";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.FinishButtonClick);
            // 
            // connectionStatusTextBox
            // 
            this.connectionStatusTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.connectionStatusTextBox.Enabled = false;
            this.connectionStatusTextBox.Location = new System.Drawing.Point(158, 54);
            this.connectionStatusTextBox.Name = "connectionStatusTextBox";
            this.connectionStatusTextBox.ReadOnly = true;
            this.connectionStatusTextBox.Size = new System.Drawing.Size(100, 13);
            this.connectionStatusTextBox.TabIndex = 6;
            // 
            // rlGlueConnectionStateLabel
            // 
            this.rlGlueConnectionStateLabel.AutoSize = true;
            this.rlGlueConnectionStateLabel.Location = new System.Drawing.Point(9, 74);
            this.rlGlueConnectionStateLabel.Name = "rlGlueConnectionStateLabel";
            this.rlGlueConnectionStateLabel.Size = new System.Drawing.Size(128, 13);
            this.rlGlueConnectionStateLabel.TabIndex = 7;
            this.rlGlueConnectionStateLabel.Text = "RLGlue connection state:";
            // 
            // rlGlueConnectionStateTextBox
            // 
            this.rlGlueConnectionStateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rlGlueConnectionStateTextBox.Enabled = false;
            this.rlGlueConnectionStateTextBox.Location = new System.Drawing.Point(158, 74);
            this.rlGlueConnectionStateTextBox.Name = "rlGlueConnectionStateTextBox";
            this.rlGlueConnectionStateTextBox.ReadOnly = true;
            this.rlGlueConnectionStateTextBox.Size = new System.Drawing.Size(100, 13);
            this.rlGlueConnectionStateTextBox.TabIndex = 8;
            // 
            // currentRewardTextBox
            // 
            this.currentRewardTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.currentRewardTextBox.Enabled = false;
            this.currentRewardTextBox.Location = new System.Drawing.Point(158, 93);
            this.currentRewardTextBox.Name = "currentRewardTextBox";
            this.currentRewardTextBox.ReadOnly = true;
            this.currentRewardTextBox.Size = new System.Drawing.Size(100, 13);
            this.currentRewardTextBox.TabIndex = 9;
            // 
            // averageRewardTextBox
            // 
            this.averageRewardTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.averageRewardTextBox.Enabled = false;
            this.averageRewardTextBox.Location = new System.Drawing.Point(158, 112);
            this.averageRewardTextBox.Name = "averageRewardTextBox";
            this.averageRewardTextBox.ReadOnly = true;
            this.averageRewardTextBox.Size = new System.Drawing.Size(100, 13);
            this.averageRewardTextBox.TabIndex = 10;
            // 
            // episodeAverageRewardTextBox
            // 
            this.episodeAverageRewardTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.episodeAverageRewardTextBox.Enabled = false;
            this.episodeAverageRewardTextBox.Location = new System.Drawing.Point(158, 131);
            this.episodeAverageRewardTextBox.Name = "episodeAverageRewardTextBox";
            this.episodeAverageRewardTextBox.ReadOnly = true;
            this.episodeAverageRewardTextBox.Size = new System.Drawing.Size(100, 13);
            this.episodeAverageRewardTextBox.TabIndex = 11;
            // 
            // rLGlueExperimentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 219);
            this.Controls.Add(this.episodeAverageRewardTextBox);
            this.Controls.Add(this.averageRewardTextBox);
            this.Controls.Add(this.currentRewardTextBox);
            this.Controls.Add(this.rlGlueConnectionStateTextBox);
            this.Controls.Add(this.rlGlueConnectionStateLabel);
            this.Controls.Add(this.connectionStatusTextBox);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.episodeAverageRewardLabel);
            this.Controls.Add(this.averageRewardLabel);
            this.Controls.Add(this.currentRewardLabel);
            this.Controls.Add(this.connectionStatusLabel);
            this.Controls.Add(this.titleLabel);
            this.Name = "rLGlueExperimentWindow";
            this.Text = "RLGlue experiment";
            this.Shown += new System.EventHandler(this.RLGlueAgentExperimentWindowShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label connectionStatusLabel;
        private System.Windows.Forms.Label currentRewardLabel;
        private System.Windows.Forms.Label averageRewardLabel;
        private System.Windows.Forms.Label episodeAverageRewardLabel;
        private System.Windows.Forms.Button finishButton;
        private System.Windows.Forms.TextBox connectionStatusTextBox;
        private System.Windows.Forms.Label rlGlueConnectionStateLabel;
        private System.Windows.Forms.TextBox rlGlueConnectionStateTextBox;
        private System.Windows.Forms.TextBox currentRewardTextBox;
        private System.Windows.Forms.TextBox averageRewardTextBox;
        private System.Windows.Forms.TextBox episodeAverageRewardTextBox;
    }
}