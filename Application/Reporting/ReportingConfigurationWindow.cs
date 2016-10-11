using System.Collections.Generic;
using System.Windows.Forms;

namespace Application.Reporting
{
    public partial class ReportingConfigurationWindow : Form
    {
        public ReportingConfigurationWindow(ExperimentBase experiment)
        {
            InitializeComponent();

            this.reportingConfigurationControl1.Experiment = experiment;
        }

        public void SetReportFileWriters(IEnumerable<ReportFileWriter> reportFileWriters, bool editable)
        {
            this.reportingConfigurationControl1.SetReportFileWriters(reportFileWriters, editable);
        }

        public IEnumerable<ReportFileWriter> GetConfiguredEditableReportWriters()
        {
            return this.reportingConfigurationControl1.GetConfiguredEditableReportWriters();
        }
    }
}
