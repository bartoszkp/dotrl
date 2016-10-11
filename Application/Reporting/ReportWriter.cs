using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Reporting.ReportTriggers;

namespace Application.Reporting
{
    public abstract class ReportWriter : IDisposable
    {
        public ReportTrigger ReportTrigger { get; private set; }

        public IEnumerable<ReportElement> ReportElements
        {
            get
            {
                return this.reportElements;
            }
        }

        public string Separator { get; set; }

        public int? BatchModeIndex { get; set; }

        public ReportWriter(ReportTrigger reportTrigger)
        {
            this.ReportTrigger = reportTrigger;
            this.reportElements = new List<ReportElement>();
            Separator = ";";
        }

        public abstract void Write(string text);

        public abstract void Dispose();

        public abstract ReportWriter CloneFor(ExperimentBase experiment);
        
        public virtual void ExperimentStarted(ExperimentBase experiment)
        {
            ProcessReportElements(reportElement => reportElement.ExperimentStarted(experiment));

            foreach (ReportElement reportElement in this.reportElements)
            {
                this.Write(reportElement.ColumnHeaderText);

                if (reportElement != this.reportElements.Last())
                {
                    this.Write(this.Separator);
                }
            }

            this.WriteLine();
        }

        public void AddReportElements(IEnumerable<ReportElement> reportElements)
        {
            this.reportElements.AddRange(reportElements);
        }

        public void EpisodeStarted(ExperimentBase experiment)
        {
            ProcessReportElements(reportElement => reportElement.EpisodeStarted(experiment));
            ProcessReportTrigger(reportTrigger => reportTrigger.EpisodeStarted(experiment), experiment);
        }

        public void StepDone(ExperimentBase experiment)
        {
            ProcessReportElements(reportElement => reportElement.StepDone(experiment));
            ProcessReportTrigger(reportTrigger => reportTrigger.StepDone(experiment), experiment);
        }

        public void EpisodeEnded(ExperimentBase experiment)
        {
            ProcessReportElements(reportElement => reportElement.EpisodeEnded(experiment));
            ProcessReportTrigger(reportTrigger => reportTrigger.EpisodeEnded(experiment), experiment);
        }

        private void ProcessReportElements(Action<ReportElement> reportElementAction)
        {
            foreach (ReportElement reportElement in this.reportElements.Where(eo => eo != null))
            {
                reportElementAction(reportElement);
            }
        }

        private void ProcessReportTrigger(Func<ReportTrigger, bool> reportTriggerAction, ExperimentBase experiment)
        {
            if (reportTriggerAction(this.ReportTrigger))
            {
                string line = string.Join(
                    this.Separator,
                    this.reportElements.Where(eo => eo != null).Select(reportElement => reportElement.GetReportToken()));

                this.WriteLine(line);

                ProcessReportElements(reportElement => reportElement.ReportTriggered(experiment));
            }
        }

        private void WriteLine()
        {
            this.WriteLine(string.Empty);
        }

        private void WriteLine(string line)
        {
            this.Write(line + Environment.NewLine);
        }

        private List<ReportElement> reportElements;
    }
}
