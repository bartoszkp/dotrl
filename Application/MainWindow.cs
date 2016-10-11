using System;
using System.Windows.Forms;
using Application.Parameters;
using Core;

namespace Application
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void NewExperimentButtonClick(object sender, EventArgs e)
        {
            var experimentSetupWindow = new ExperimentSetupWindow();

            if (experimentSetupWindow.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var experimentWindow = experimentSetupWindow.GetConfiguredExperimentWindow();

            experimentWindow.Show();
        }

        private void GlobalSettingsButtonClick(object sender, EventArgs e)
        {
            ParameterWindow.ConfigureParametrizedObjectAndInnerObjects(new GlobalParameters());
        }

        private void ConnectToRlGlueButtonClick(object sender, EventArgs e)
        {
            var configurationWindow = new Integration.RLGlue.RLGlueExperimentConfigurationWindow();

            if (configurationWindow.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            var component = configurationWindow.ComponentType.InstantiateWithDefaultConstructor<Component>();

            if (ParameterWindow.ConfigureParametrizedObjectAndInnerObjects(component) != DialogResult.OK)
            {
                return;
            }

            var experimentWindow = new Integration.RLGlue.RLGlueExperimentWindow(
                configurationWindow.Host,
                configurationWindow.PortNumber,
                component);

            experimentWindow.Show();
        }

        private void BatchExperimentButtonClick(object sender, EventArgs e)
        {
            BatchExperimentSetupWindow bsw = new BatchExperimentSetupWindow();

            if (bsw.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            BatchExperimentWindow bew = new BatchExperimentWindow();
            bew.SetBatchExperiment(bsw.GetConfiguredBatchExperiment());
            bew.ShowDialog();
        }
    }
}
