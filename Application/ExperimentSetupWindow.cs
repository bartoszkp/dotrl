using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Application.ExperimentTemplates;
using Application.Reporting;
using Core;
using Core.Parameters;
using Presenters;

namespace Application
{
    public partial class ExperimentSetupWindow : Form
    {
        public static ExperimentBase LoadExperiment()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "et";
            ofd.Filter = "Experiment templates (*.et)|*.et";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            ExperimentTemplate et = null;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ExperimentTemplate));
            using (System.IO.StreamReader sw = new System.IO.StreamReader(ofd.FileName))
            {
                et = (ExperimentTemplate)serializer.Deserialize(sw.BaseStream);
            }

            return et.ToExperiment();
        }

        public ExperimentSetupWindow()
        {
            this.inhibitEvents = false;

            InitializeComponent();

            IEnumerable<Type> environments = EnvironmentRegistry.GetEnvironments().ToArray();

            List<string> environmentNames = environments
                .Select(environmentType => environmentType.Name)
                .ToList();

            int e = 0;
            foreach (Type environmentType in environments)
            {
                RadioButton radioButton = new RadioButton();
                this.environmentPanel.Controls.Add(radioButton);

                radioButton.AutoSize = true;
                radioButton.Location = new System.Drawing.Point(20, e * 16);
                radioButton.Name = "environmentRadioButton" + e;
                radioButton.TabIndex = e;
                radioButton.TabStop = true;
                radioButton.Tag = environmentType;
                radioButton.Text = environmentType.GetDisplayName();
                radioButton.CheckedChanged += new System.EventHandler(this.EnvironmentSelected);
                ++e;
            }

            this.okButton.Enabled = false;
            this.environmentParametersApplyButton.Enabled = false;
            this.environmentParametersRevertButton.Enabled = false;
            this.agentParametersApplyButton.Enabled = false;
            this.agentParametersRevertButton.Enabled = false;
            this.experimentParametersApplyButton.Enabled = false;
            this.experimentParametersRevertButton.Enabled = false;
            this.tabControl.TabPages.Remove(this.experimentParametersTabPage);
            this.tabControl.TabPages.Remove(this.reportingTabPage);
        }

        public void SetExperiment(ExperimentBase experiment)
        {
            this.inhibitEvents = true;

            this.environment = experiment.Environment;
            this.presentationEnvironment = experiment.PresentationEnvironment;
            this.experiment = experiment;

            foreach (RadioButton r in this.environmentPanel.Controls.Cast<RadioButton>())
            {
                r.Checked = r.Tag.Equals(this.environment.GetType());
            }

            RebuildAgentList(this.environment.ComponentType);

            foreach (RadioButton r in this.agentPanel.Controls.Cast<RadioButton>())
            {
                r.Checked = r.Tag.Equals(this.experiment.Agent.GetType());
            }

            RefreshTabPages();

            this.environmentParameterControl.Initialize(this.environment, true);
            this.agentParameterControl.Initialize(experiment.Agent, true);
            this.experimentParameterControl.Initialize(this.experiment, false);

            this.reportingConfigurationControl.Experiment = this.experiment;
            this.reportingConfigurationControl.SetReportFileWriters(experiment.Reporter.ReportWriters.Cast<ReportFileWriter>(), true);

            this.inhibitEvents = false;
        }

        public ExperimentBase GetConfiguredExperiment()
        {
            return this.experiment;
        }

        public ExperimentWindow GetConfiguredExperimentWindow()
        {
            ExperimentWindow result = new ExperimentWindow(this.experiment);

            return result;
        }

        private void EnvironmentSelected(object sender, EventArgs e)
        {
            if (this.inhibitEvents)
            {
                return;
            } 
            
            Type environmentType = (Type)((RadioButton)sender).Tag;
            this.environment = environmentType.InstantiateWithDefaultConstructor<Component>();
            this.presentationEnvironment = this.environment.Clone();

            RebuildAgentList(this.environment.ComponentType);
          
            this.environmentParameterControl.Initialize(this.environment, true);
        }

        private void RebuildAgentList(ComponentType componentType)
        {
            this.agentPanel.Controls.Clear();

            IEnumerable<Type> agentTypes = AgentRegistry.GetAgents(componentType);

            int a = 0;
            foreach (Type agentType in agentTypes)
            {
                RadioButton radioButton = new RadioButton();
                this.agentPanel.Controls.Add(radioButton);

                radioButton.AutoSize = true;
                radioButton.Location = new System.Drawing.Point(20, a * 16);
                radioButton.Name = "agentRadioButton" + a;
                radioButton.TabIndex = a;
                radioButton.TabStop = true;
                radioButton.Tag = agentType;
                radioButton.Text = agentType.GetDisplayName();
                radioButton.CheckedChanged += new System.EventHandler(this.AgentSelected);
                radioButton.Checked = a == 0;
                ++a;
            }

            this.okButton.Enabled = agentTypes.Any();
        }

        private void AgentSelected(object sender, EventArgs e)
        {
            if (this.inhibitEvents)
            {
                return;
            }

            RadioButton radioButton = (RadioButton)sender;
            Type agentType = (Type)radioButton.Tag;
            
            Component agent = agentType.InstantiateWithDefaultConstructor<Component>();

            this.agentParameterControl.Initialize(agent, true);

            InstantiateExperiment(agent);

            RefreshTabPages();
        }

        private void RefreshTabPages()
        {
            if (!this.tabControl.TabPages.Contains(this.experimentParametersTabPage))
            {
                this.tabControl.TabPages.Add(this.experimentParametersTabPage);
            }

            if (!this.tabControl.TabPages.Contains(this.reportingTabPage))
            {
                this.tabControl.TabPages.Add(this.reportingTabPage);
            }
        }

        private void InstantiateExperiment(Component agent)
        {
            this.experiment = ExperimentBase.Instantiate(this.environment, this.presentationEnvironment, agent);

            this.experimentParameterControl.Initialize(this.experiment, false);
            this.reportingConfigurationControl.Experiment = this.experiment;
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            this.environmentParameterControl.ApplyChanges();
            this.agentParameterControl.ApplyChanges();
            this.experimentParameterControl.ApplyChanges();

            Reporter reporter = new Reporter();
            reporter.ReportWriters.AddRange(this.reportingConfigurationControl.GetConfiguredEditableReportWriters());
            this.experiment.Reporter = reporter;
        }

        private void EnvironmentParameterControlParametersChanged(object sender, EventArgs e)
        {
            this.presentationEnvironment.CopyParametersFromWithInnerObjects(this.environment);
        }

        private void EnvironmentParametersApplyButtonClick(object sender, EventArgs e)
        {
            this.environmentParameterControl.ApplyChanges();
            this.environmentParametersApplyButton.Enabled = false;
            this.environmentParametersRevertButton.Enabled = false;        
        }

        private void AgentParametersApplyButtonClick(object sender, EventArgs e)
        {
            this.agentParameterControl.ApplyChanges();
            this.agentParametersApplyButton.Enabled = false;
            this.agentParametersRevertButton.Enabled = false;
        }

        private void ExperimentParametersApplyButtonClick(object sender, EventArgs e)
        {
            this.experimentParameterControl.ApplyChanges();
            this.experimentParametersApplyButton.Enabled = false;
            this.experimentParametersRevertButton.Enabled = false;
        }

        private void EnvironmentParametersRevertButtonClick(object sender, EventArgs e)
        {
            this.environmentParameterControl.RevertChanges();
            this.environmentParametersApplyButton.Enabled = false;
            this.environmentParametersRevertButton.Enabled = false;
        }

        private void AgentParametersRevertButtonClick(object sender, EventArgs e)
        {
            this.agentParameterControl.RevertChanges();
            this.agentParametersApplyButton.Enabled = false;
            this.agentParametersRevertButton.Enabled = false;
        }

        private void ExperimentParametersRevertButtonClick(object sender, EventArgs e)
        {
            this.experimentParameterControl.RevertChanges();
            this.experimentParametersApplyButton.Enabled = false;
            this.experimentParametersRevertButton.Enabled = false;
        }

        private void EnvironmentParameterControlParameterValueEdited(object sender, EventArgs e)
        {
            this.environmentParametersApplyButton.Enabled = true;
            this.environmentParametersRevertButton.Enabled = true;
        }

        private void AgentParameterControlParameterValueEdited(object sender, EventArgs e)
        {
            this.agentParametersApplyButton.Enabled = true;
            this.agentParametersRevertButton.Enabled = true;
        }

        private void ExperimentParameterControlParameterValueEdited(object sender, EventArgs e)
        {
            this.experimentParametersApplyButton.Enabled = true;
            this.experimentParametersRevertButton.Enabled = true;
        }

        private void SaveExperimentTemplateButtonClick(object sender, EventArgs e)
        {
            this.environmentParameterControl.ApplyChanges();
            this.agentParameterControl.ApplyChanges();
            this.experimentParameterControl.ApplyChanges();
            this.environmentParametersApplyButton.Enabled = false;
            this.environmentParametersRevertButton.Enabled = false;
            this.agentParametersApplyButton.Enabled = false;
            this.agentParametersRevertButton.Enabled = false;
            this.experimentParametersApplyButton.Enabled = false;
            this.experimentParametersRevertButton.Enabled = false;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "et";
            sfd.Filter = "Experiment templates (*.et)|*.et";
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Reporter reporter = new Reporter();
            reporter.ReportWriters.AddRange(this.reportingConfigurationControl.GetConfiguredEditableReportWriters());

            ExperimentTemplate et = ExperimentTemplate
                .Create(experiment)
                .WithReporter(reporter);

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ExperimentTemplate));
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
            {
                serializer.Serialize(sw, et);
            }
        }

        private void LoadExperimentTemplateButtonClick(object sender, EventArgs e)
        {
            ExperimentBase experiment = ExperimentSetupWindow.LoadExperiment();

            if (experiment != null)
            {
                SetExperiment(experiment);
            }
        }

        private Component environment;
        private Component presentationEnvironment;
        private ExperimentBase experiment;
        private bool inhibitEvents;
    }
}
