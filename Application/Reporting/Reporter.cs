using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Reporting
{
    public class Reporter : IDisposable
    {
        public List<ReportWriter> ReportWriters { get; set; }

        public Reporter()
        {
            ReportWriters = new List<ReportWriter>();
        }

        public void SetBatchModeIndex(int batchModeIndex)
        {
            foreach (var rw in this.ReportWriters)
            {
                rw.BatchModeIndex = batchModeIndex;
            }
        }

        public void ExperimentStarted(ExperimentBase experiment)
        {
            ProcessReportWriters(reportFile => reportFile.ExperimentStarted(experiment));
        }

        public void EpisodeStarted(ExperimentBase experiment)
        {
            ProcessReportWriters(reportFile => reportFile.EpisodeStarted(experiment));
        }

        public void StepDone(ExperimentBase experiment)
        {
            ProcessReportWriters(reportFile => reportFile.StepDone(experiment));
        }

        public void EpisodeEnded(ExperimentBase experiment)
        {
            ProcessReportWriters(reportFile => reportFile.EpisodeEnded(experiment));
        }

        public void Dispose()
        {
            ProcessReportWriters(reportFile => reportFile.Dispose());
        }

        private void ProcessReportWriters(Action<ReportWriter> reportFileAction)
        {
            ReportWriters.ForEach(reportFileAction);
        }

        public Reporter CloneFor(ExperimentBase experiment)
        {
            Reporter result = new Reporter();
            
            result.ReportWriters.AddRange(this.ReportWriters.Select(rw => rw.CloneFor(experiment)));

            return result;
        }
    }
}
