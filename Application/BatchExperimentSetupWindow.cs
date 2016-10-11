using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Application.ExperimentTemplates;
using Core;

namespace Application
{
    public partial class BatchExperimentSetupWindow : Form
    {
        public static BatchExperiment LoadBatchExperiment()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "bet";
            ofd.Filter = "Batch experiment templates (*.bet)|*.bet";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            BatchExperimentTemplate bet = null;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BatchExperimentTemplate));
            using (System.IO.StreamReader sw = new System.IO.StreamReader(ofd.FileName))
            {
                bet = (BatchExperimentTemplate)serializer.Deserialize(sw.BaseStream);
            }

            return bet.ToBatchExperiment();
        }

        public BatchExperimentSetupWindow()
        {
            InitializeComponent();

            SetBatchExperiment(null);
        }

        public void SetBatchExperiment(BatchExperiment batchExperiment)
        {
            this.experimentListBox.Items.Clear();
            this.batchModeComboBox.SelectedIndex = 0;

            RefreshLabels(null);

            if (batchExperiment == null)
            {
                return;
            }

            this.batchModeComboBox.SelectedIndex = (int)batchExperiment.BatchMode;

            foreach (ExperimentBase e in batchExperiment.Experiments)
            {
                AddExperiment(e);
            }
        }

        public BatchExperiment GetConfiguredBatchExperiment()
        {
            BatchExperiment result = new BatchExperiment(GetSelectedBatchMode());

            result.AddExperiments(GetConfiguredExperiments());

            return result;
        }

        private BatchMode GetSelectedBatchMode()
        {
            return (BatchMode)(this.batchModeComboBox.SelectedIndex);
        }

        private IEnumerable<ExperimentBase> GetConfiguredExperiments()
        {
            return this.experimentListBox.Items.Cast<ExperimentInfo>().Select(e => e.Experiment);
        }

        private void AddExperimentButtonClick(object sender, EventArgs e)
        {
            ExperimentSetupWindow esw = new ExperimentSetupWindow();

            if (esw.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            AddExperiment(esw.GetConfiguredExperiment());
        }
        
        private void LoadExperimentButtonClick(object sender, EventArgs e)
        {
            ExperimentBase experiment = ExperimentSetupWindow.LoadExperiment();

            if (experiment != null)
            {
                AddExperiment(experiment);
            }
        }

        private void AddExperiment(ExperimentBase experiment)
        {
            this.experimentListBox.Items.Add(new ExperimentInfo()
            {
                Ordinal = this.experimentListBox.Items.Count + 1,
                Experiment = experiment
            });

            this.experimentListBox.SelectedIndex = this.experimentListBox.Items.Count - 1;
        }

        private void ExperimentListSelectedChanged(object sender, EventArgs e)
        {
            RefreshLabels(GetSelectedExperimentOrNull());
        }

        private ExperimentInfo GetSelectedExperimentInfo()
        {
            return this.experimentListBox.SelectedItem as ExperimentInfo;
        }

        private ExperimentBase GetSelectedExperimentOrNull()
        {
            var selectedExperimentInfo = GetSelectedExperimentInfo();

            if (selectedExperimentInfo == null)
            {
                return null;
            }

            return selectedExperimentInfo.Experiment;
        }

        private void ConfigureButtonClick(object sender, EventArgs e)
        {
            ExperimentInfo selectedExperimentInfo = GetSelectedExperimentInfo();

            if (selectedExperimentInfo == null)
            {
                return;
            }

            ExperimentSetupWindow esw = new ExperimentSetupWindow();
            esw.SetExperiment(selectedExperimentInfo.Experiment);

            if (esw.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            selectedExperimentInfo.Experiment = esw.GetConfiguredExperiment();

            RefreshLabels(selectedExperimentInfo.Experiment);
        }

        private void RemoveButtonClick(object sender, EventArgs e)
        {
            ExperimentInfo selectedExperimentInfo = GetSelectedExperimentInfo();

            if (selectedExperimentInfo == null)
            {
                return;
            }

            int removedOrdinal = selectedExperimentInfo.Ordinal;

            this.experimentListBox.Items.Remove(selectedExperimentInfo);

            foreach (ExperimentInfo ei in this.experimentListBox.Items.Cast<ExperimentInfo>().Where(ei => ei.Ordinal > removedOrdinal).ToArray())
            {
                ei.Ordinal -= 1;
                this.experimentListBox.Items[ei.Ordinal - 1] = ei;
            }

            if (this.experimentListBox.Items.Count > 0)
            {
                this.experimentListBox.SelectedIndex = this.experimentListBox.Items.Count - 1;
            }
        }

        private void CloneButtonClick(object sender, EventArgs e)
        {
            ExperimentInfo selectedExperimentInfo = GetSelectedExperimentInfo();

            if (selectedExperimentInfo == null)
            {
                return;
            }

            for (int i = 0; i < this.cloneCountUpDown.Value; ++i)
            {
                AddExperiment(selectedExperimentInfo.Experiment.Clone());
            }
        }

        private void SaveBatchExperimentTemplateButtonClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "bet";
            sfd.Filter = "Batch experiment templates (*.bet)|*.bet";
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            BatchExperimentTemplate bet = BatchExperimentTemplate
                .Create(GetSelectedBatchMode())
                .WithExperimentTemplates(GetConfiguredExperiments()
                                           .Select(ex => ExperimentTemplate.Create(ex)));

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(BatchExperimentTemplate));
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
            {
                serializer.Serialize(sw, bet);
            }
        }

        private void LoadBatchExperimentTemplateButtonClick(object sender, EventArgs e)
        {
            BatchExperiment be = BatchExperimentSetupWindow.LoadBatchExperiment();

            if (be == null)
            {
                return;
            }

            SetBatchExperiment(be);
        }

        private void RefreshLabels(ExperimentBase experiment)
        {
            if (experiment == null)
            {
                this.environmentLabel.Text = string.Empty;
                this.agentLabel.Text = string.Empty;
                this.experimentEpisodeCountLabel.Text = string.Empty;
                this.experimentEpisodeStepCountLabel.Text = string.Empty;
                this.experimentTotalStepCountLabel.Text = string.Empty;
                return;
            }

            this.environmentLabel.Text = experiment.Environment.GetType().GetDisplayName();
            this.agentLabel.Text = experiment.Agent.GetType().GetDisplayName();

            if (experiment.EpisodeCountLimit > 0)
            {
                this.experimentEpisodeCountLabel.Text = experiment.EpisodeCountLimit + " episodes";
            }
            else
            {
                this.experimentEpisodeCountLabel.Text = "Unlimited number of episodes";
            }

            if (experiment.EpisodeStepCountLimit > 0)
            {
                this.experimentEpisodeStepCountLabel.Text = experiment.EpisodeStepCountLimit + " steps in episode";
            }
            else
            {
                this.experimentEpisodeStepCountLabel.Text = "Unlimited number of steps in episode";
            }

            if (experiment.TotalStepCountLimit > 0)
            {
                this.experimentTotalStepCountLabel.Text = experiment.TotalStepCountLimit + " total steps";
            }
            else
            {
                this.experimentTotalStepCountLabel.Text = "Unlimited number of total steps";
            }
        }

        private class ExperimentInfo
        {
            public int Ordinal { get; set; }
            public ExperimentBase Experiment { get; set; }

            public override string ToString()
            {
                return "Experiment " + this.Ordinal;
            }
        }
    }
}
