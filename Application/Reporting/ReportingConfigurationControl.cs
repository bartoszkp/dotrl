using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Application.Reporting.DataAccumulators;
using Application.Reporting.ReportTriggers;
using Core;

namespace Application.Reporting
{
    public partial class ReportingConfigurationControl : UserControl
    {
        public ExperimentBase Experiment 
        {
            get
            {
                return this.experiment;
            }
            set
            {
                this.reportFilesTabControl.TabPages.Clear();
                this.experiment = value;

                this.addReportFileButton.Enabled = this.experiment != null;
            }
        }

        public ReportingConfigurationControl()
        {
            InitializeComponent();

            this.addReportFileButton.Enabled = false;
        }

        public void SetReportFileWriters(IEnumerable<ReportFileWriter> reportFileWriters, bool editable)
        {
            this.reportFilesTabControl.TabPages.Clear();

            foreach (ReportFileWriter reportFile in reportFileWriters)
            {
                TabPageInfo tabPageInfo = AddReportFile(reportFile.FileName, editable);
                tabPageInfo.ReportWriterControl.SetReportWriter(reportFile);
            }
        }

        public IEnumerable<ReportFileWriter> GetConfiguredEditableReportWriters()
        {
            Reporter r = new Reporter();
            
            List<ReportFileWriter> result = new List<ReportFileWriter>();

            foreach (TabPage tabPage in this.reportFilesTabControl.TabPages)
            {
                TabPageInfo tabPageInfo = tabPage.Tag as TabPageInfo;

                if (!tabPageInfo.ReportWriterControl.Editable)
                {
                    continue;
                }

                ReportFileWriter rfw = new ReportFileWriter(
                    tabPageInfo.ReportWriterControl.GetConfiguredReportTrigger(),
                    tabPageInfo.FileName);

                rfw.AddReportElements(tabPageInfo.ReportWriterControl.GetConfiguredReportElements());

                result.Add(rfw);
            }

            return result;
        }

        private void AddReportFileButtonClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.CreatePrompt = false;
            sfd.OverwritePrompt = true;
            sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Title = "Choose name of the new report file";

            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            AddReportFile(sfd.FileName, true);
        }

        private TabPageInfo AddReportFile(string fileName, bool editable)
        {
            TabPage newTabPage = new TabPage(Path.GetFileName(fileName));
            
            this.reportFilesTabControl.TabPages.Add(newTabPage);

            Button removeReportFileButton = new Button();
            removeReportFileButton.Text = "Remove this report file";
            removeReportFileButton.AutoSize = true;

            removeReportFileButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            int rightOffset = removeReportFileButton.Margin.Right + newTabPage.Padding.Right;
            int bottomOffset = removeReportFileButton.Margin.Bottom + newTabPage.Padding.Bottom;

            removeReportFileButton.Location = new Point(
                newTabPage.Width - removeReportFileButton.Width - rightOffset,
                newTabPage.Height - removeReportFileButton.Height - bottomOffset);

            removeReportFileButton.Tag = newTabPage;
            removeReportFileButton.Click += new EventHandler(RemoveReportFileButtonClick);

            newTabPage.Controls.Add(removeReportFileButton);

            ReportWriterConfigurationControl reportWriterControl = new ReportWriterConfigurationControl(
                ReportTrigger.GetReportTriggers().ToArray(),
                DataAccumulator.GetDataAccumulators().ToArray(),
                this.Experiment,
                editable);

            reportWriterControl.Dock = DockStyle.Top;

            newTabPage.Controls.Add(reportWriterControl);

            TabPageInfo tabPageInfo = new TabPageInfo()
            {
                FileName = fileName,
                ReportWriterControl = reportWriterControl
            };

            newTabPage.Tag = tabPageInfo;

            newTabPage.Select();

            return tabPageInfo;
        }

        private void RemoveReportFileButtonClick(object sender, EventArgs e)
        {
            this.reportFilesTabControl.TabPages.Remove((TabPage)((Button)sender).Tag);
        }

        private class TabPageInfo
        {
            public string FileName { get; set; }

            public ReportWriterConfigurationControl ReportWriterControl { get; set; }
        }

        private ExperimentBase experiment;
    }
}
