using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Core;

namespace Application
{
    public partial class BatchExperimentWindow : Form
    {
        public BatchExperimentWindow()
        {
            InitializeComponent();

            this.batchExperiment = null;
            this.okButton.Enabled = false;
            this.inhibitBatchModeChangedEvent = false;
            this.startAllButton.Enabled = false;
            this.pauseAllButton.Enabled = false;
            this.batchModeComboBox.Enabled = false;
            this.startSelectedButton.Enabled = false;
            this.pauseSelectedButton.Enabled = false;
            this.viewSelectedButton.Enabled = false;

            this.batchModeComboBox.Items.AddRange(Enum
                .GetValues(typeof(BatchMode))
                .Cast<BatchMode>()
                .Select(bm => bm.ToString())
                .ToArray());

            this.started = false;
        }

        public void SetBatchExperiment(BatchExperiment batchExperiment)
        {
            this.batchExperiment = batchExperiment;

            this.experimentImageList.Images.Clear();

            this.progressChangedSemaphore = new SemaphoreSlim(1, 1);

            this.progressBarBitmapBounds = Rectangle.Empty;
            this.progressBarBitmapBounds.Size = this.experimentImageList.ImageSize;

            int totalProgress = 0;
            int experimentIndex = 0;
            foreach (ExperimentBase e in batchExperiment.Experiments)
            {
                Bitmap progressBarBitmap = new Bitmap(
                    this.experimentImageList.ImageSize.Width,
                    this.experimentImageList.ImageSize.Height);

                this.experimentImageList.Images.Add(progressBarBitmap);

                ProgressBar progressBar = new ProgressBar();
                progressBar.MinimumSize = this.experimentImageList.ImageSize;
                progressBar.MaximumSize = this.experimentImageList.ImageSize;
                progressBar.Size = this.experimentImageList.ImageSize;

                ExperimentInfo ei = new ExperimentInfo()
                {
                    Experiment = e,
                    ExperimentWorker = new ExperimentWorker(e),
                    ProgressBar = progressBar,
                    ProgressBarBitmap = progressBarBitmap,
                    Status = Status.Waiting
                };

                ei.ExperimentWorker.ConfigureProgressBar(ei.ProgressBar);

                ListViewItem lvi = new ListViewItem(GetExperimentDescription(ei), this.experimentListView.Items.Count);

                lvi.UseItemStyleForSubItems = true;

                this.experimentListView.Items.Add(lvi);

                lvi.Tag = ei;

                ei.ExperimentWorker.Tag = lvi;
                ei.ExperimentWorker.ExperimentFinished += new System.EventHandler(ExperimentFinished);

                totalProgress += progressBar.Maximum;

                ei.Experiment.Reporter.SetBatchModeIndex(experimentIndex);
                
                experimentIndex++;
            }

            this.totalProgressBar.Maximum = totalProgress;

            if (totalProgress == 0)
            {
                this.totalProgressBar.Style = ProgressBarStyle.Marquee;
                this.totalProgressBar.MarqueeAnimationSpeed = 500;
            }

            this.experimentListView.Refresh();

            ResizeColumns();

            this.inhibitBatchModeChangedEvent = true;
            this.batchModeComboBox.SelectedIndex = (int)(this.batchExperiment.BatchMode);
            this.inhibitBatchModeChangedEvent = false;
            this.startAllButton.Enabled = true;
            this.batchModeComboBox.Enabled = true;
        }

        private void ExperimentFinished(object sender, System.EventArgs e)
        {
            MethodInvoker mi = new MethodInvoker(() => ExperimentFinished(sender as ExperimentWorker));

            this.Invoke(mi);
        }

        public void ExperimentFinished(ExperimentWorker experimentWorker)
        {
            ExperimentInfo ei = ((experimentWorker.Tag as ListViewItem).Tag as ExperimentInfo);
            ei.Status = Status.Finished;
            ei.Error = experimentWorker.Error;

            if (this.experimentListView.Items.Cast<ListViewItem>().All(lvi => (lvi.Tag as ExperimentInfo).Status == Status.Finished))
            {
                this.batchModeComboBox.Enabled = false;
                this.okButton.Enabled = true;
                this.cancelButton.Enabled = false;
                this.startAllButton.Enabled = false;
                this.pauseAllButton.Enabled = false;
            }
            else if (this.batchExperiment.BatchMode != BatchMode.Parallel)
            {
                StartSequential();
            }

            Refresh();
        }

        private string[] GetExperimentDescription(ExperimentInfo experimentInfo)
        {
            string[] result = new string[7];

            result[0] = string.Empty;
            result[1] = experimentInfo.Experiment.Environment.GetType().GetDisplayName();
            result[2] = experimentInfo.Experiment.Agent.GetType().GetDisplayName();
            result[3] = GetLimitDescription(experimentInfo.Experiment.EpisodeCountLimit);
            result[4] = GetLimitDescription(experimentInfo.Experiment.EpisodeStepCountLimit);
            result[5] = GetLimitDescription(experimentInfo.Experiment.TotalStepCountLimit);
            result[6] = GetStatusString(experimentInfo);

            return result;
        }

        private string GetStatusString(ExperimentInfo experimentInfo)
        {
            string result = experimentInfo.Status.ToString();

            if (experimentInfo.Status == Status.Finished && experimentInfo.Error != null)
            {
                result += " with error";
            }

            return result;
        }

        private string GetLimitDescription(int limit)
        {
            if (limit == 0)
            {
                return "Unlimited";
            }

            return string.Empty + limit;
        }

        private void BatchModeComboBoxSelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.inhibitBatchModeChangedEvent)
            {
                return;
            }

            this.batchExperiment.BatchMode = (BatchMode)(this.batchModeComboBox.SelectedIndex);

            if (this.started)
            {
                this.StartButtonClick(null, System.EventArgs.Empty);
            }
        }

        private new void Refresh()
        {
            int currentProgress = 0;

            foreach (ListViewItem lvi in this.experimentListView.Items)
            {
                ExperimentInfo ei = lvi.Tag as ExperimentInfo;

                lvi.SubItems[lvi.SubItems.Count - 1].Text = GetStatusString(ei);

                currentProgress += ei.ProgressBar.Value;
            }

            this.totalProgressBar.Value = currentProgress;

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            this.experimentListView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.experimentListView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.experimentListView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent);
            this.experimentListView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);
            this.experimentListView.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.HeaderSize);
            this.experimentListView.AutoResizeColumn(5, ColumnHeaderAutoResizeStyle.HeaderSize);
            this.experimentListView.AutoResizeColumn(6, ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void StartButtonClick(object sender, System.EventArgs e)
        {
            this.started = true;

            if (this.batchExperiment.BatchMode == BatchMode.Parallel)
            {
                StartParallel();
            }
            else
            {
                StartSequential();
            }

            this.startAllButton.Enabled = false;
            this.pauseAllButton.Enabled = true;
            this.timer.Start();
        }

        private void PauseButtonClick(object sender, System.EventArgs e)
        {
            Pause();
            this.startAllButton.Enabled = true;
            this.pauseAllButton.Enabled = false;
        }

        private void StartParallel()
        {
            foreach (ListViewItem lvi in this.experimentListView.Items)
            {
                Start(lvi.Tag as ExperimentInfo);
            }

            Refresh();
        }

        private void StartSequential()
        {
            int count = (int)Math.Pow(2, (int)(this.batchExperiment.BatchMode) - 1);

            int found = 0;
            foreach (ListViewItem lvi in this.experimentListView.Items)
            {
                ExperimentInfo ei = lvi.Tag as ExperimentInfo;

                if (found >= count)
                {
                    if (ei.Status != Status.Finished)
                    {
                        ei.ExperimentWorker.PauseWork();
                        ei.Status = Status.Waiting;
                    }
                }
                else if (Start(ei))
                {
                    found += 1;
                }
            }

            Refresh();
        }

        private bool Start(ExperimentInfo ei)
        {
            if (ei.Status == Status.Finished)
            {
                return false;
            }

            ei.ExperimentWorker.ResumeBackgroundLearning();
            ei.Status = Status.Executing;
            return true;
        }

        private void Pause()
        {
            foreach (ListViewItem lvi in this.experimentListView.Items)
            {
                Pause(lvi.Tag as ExperimentInfo);
            }

            this.started = false;
        }

        private bool Pause(ExperimentInfo ei)
        {
            if (ei.Status == Status.Finished)
            {
                return false;
            }

            ei.ExperimentWorker.PauseWork();
            ei.Status = Status.Waiting;
            return true;
        }

        private void End()
        {
            foreach (ListViewItem lvi in this.experimentListView.Items)
            {
                ExperimentInfo ei = lvi.Tag as ExperimentInfo;

                if (ei.Status != Status.Finished)
                {
                    if (ei.Status != Status.Finished)
                    {
                        ei.ExperimentWorker.PauseWork();
                        ei.Status = Status.Waiting;
                    }
                }

                ei.Experiment.End();
            }
        }

        private void OkButtonClick(object sender, System.EventArgs e)
        {
            End();
            Refresh();
        }

        private void CancelButtonClick(object sender, System.EventArgs e)
        {
            End();
            Refresh();
        }

        private void TimerTick(object sender, System.EventArgs e)
        {
            foreach (ListViewItem lvi in this.experimentListView.Items)
            {
                ExperimentInfo ei = lvi.Tag as ExperimentInfo;

                int previousProgress = ei.ProgressBar.Value;
                ei.ProgressBar.Value = ei.ExperimentWorker.CurrentProgress;

                if (ei.ProgressBar.Value != previousProgress)
                {
                    ei.ProgressBar.DrawToBitmap(ei.ProgressBarBitmap, this.progressBarBitmapBounds);
                    this.experimentImageList.Images[lvi.Index] = ei.ProgressBarBitmap;
                }
            }

            Refresh();
        }

        private void ExperimentListViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.startSelectedButton.Enabled = false;
            this.pauseSelectedButton.Enabled = false;
            this.viewSelectedButton.Enabled = this.experimentListView.SelectedItems.Count == 1;

            foreach (ListViewItem lvi in this.experimentListView.SelectedItems)
            {
                ExperimentInfo ei = lvi.Tag as ExperimentInfo;

                if (ei.Status == Status.Waiting)
                {
                    this.startSelectedButton.Enabled = true;
                }
                else if (ei.Status == Status.Executing)
                {
                    this.pauseSelectedButton.Enabled = true;
                }
            }
        }

        private void StartSelectedButtonClick(object sender, System.EventArgs e)
        {
            foreach (ListViewItem lvi in this.experimentListView.SelectedItems)
            {
                Start(lvi.Tag as ExperimentInfo);
            }
        }

        private void PauseSelectedButtonClick(object sender, System.EventArgs e)
        {
            foreach (ListViewItem lvi in this.experimentListView.SelectedItems)
            {
                Pause(lvi.Tag as ExperimentInfo);
            }
        }

        private void ViewSelectedButtonClick(object sender, System.EventArgs e)
        {
            Pause();

            ExperimentInfo ei = this.experimentListView.SelectedItems[0].Tag as ExperimentInfo;

            if (ei.Status == Status.Finished && ei.Error != null)
            {
                ExperimentWindow.DisplayException(this, ei.Error);
                return;
            }

            ExperimentWindow ew = new ExperimentWindow(ei.ExperimentWorker);

            ew.ShowDialog();
        }

        private enum Status
        {
            Executing,
            Waiting,
            Finished
        }

        private class ExperimentInfo
        {
            public ExperimentBase Experiment { get; set; }
            public ExperimentWorker ExperimentWorker { get; set; }
            public ProgressBar ProgressBar { get; set; }
            public Bitmap ProgressBarBitmap { get; set; }
            public Status Status { get; set; }
            public System.Exception Error { get; set; }
        }

        private BatchExperiment batchExperiment;
        private bool inhibitBatchModeChangedEvent;
        private Rectangle progressBarBitmapBounds;
        private SemaphoreSlim progressChangedSemaphore;
        private bool started;
    }
}
