using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Application.Reporting.ExperimentObservers;
using Application.Reporting.TraceTriggers;

namespace Application.Reporting
{
    public class ReportFile : IDisposable
    {
        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public string Separator { get; set; }

        public IEnumerable<ReportFileTraceLine> TraceLines
        {
            get
            {
                return this.traceLines;
            }
        }

        public int TraceLineCount
        {
            get
            {
                return this.traceLines.Count;
            }
        }

        public ReportFile(string fileName)
        {
            this.fileName = fileName;
            this.traceLines = new List<ReportFileTraceLine>();
            Separator = ";";
        }

        public void Initialize()
        {
            this.streamWriter = new StreamWriter(this.fileName);

            int columns = this.traceLines.Max(traceLine => traceLine.ExperimentObservers.Count());

            for (int i = 0; i < columns; ++i)
            {
                ExperimentObserver experimentObserver = this.traceLines
                    .Select(traceLine => traceLine.ExperimentObservers.ElementAt(i))
                    .First(e => e != null);

                this.streamWriter.Write(experimentObserver.GetColumnHeader());

                if (i < columns - 1)
                {
                    this.streamWriter.Write(this.Separator);
                }
            }

            this.streamWriter.WriteLine();
        }

        public void AddTraceLine(ReportFileTraceLine reportTraceLine)
        {
            this.traceLines.Add(reportTraceLine);
        }

        public void EpisodeStarted(ExperimentBase experiment)
        {
            ProcessObservers(experimentObserver => experimentObserver.EpisodeStarted(experiment));
            ProcessTraceLines(traceTrigger => traceTrigger.EpisodeStarted(experiment));
        }

        public void StepDone(ExperimentBase experiment)
        {
            ProcessObservers(experimentObserver => experimentObserver.StepDone(experiment));
            ProcessTraceLines(traceTrigger => traceTrigger.StepDone(experiment));
        }

        public void AgentTraceReceived(string agentTrace)
        {
            ProcessObservers(experimentObserver => experimentObserver.AgentTraceReceived(agentTrace));
            ProcessTraceLines(traceTrigger => traceTrigger.AgentTraceReceived(agentTrace));
        }

        public void EnvironmentTraceReceived(string environmentTrace)
        {
            ProcessObservers(experimentObserver => experimentObserver.EnvironmentTraceReceived(environmentTrace));
            ProcessTraceLines(traceTrigger => traceTrigger.AgentTraceReceived(environmentTrace));
        }

        public void EpisodeEnded(ExperimentBase experiment)
        {
            ProcessObservers(experimentObserver => experimentObserver.EpisodeEnded(experiment));
            ProcessTraceLines(traceTrigger => traceTrigger.EpisodeEnded(experiment));
        }

        public void Dispose()
        {
            this.streamWriter.Close();
            this.streamWriter.Dispose();
        }

        private void ProcessObservers(Action<ExperimentObserver> experimentObserverAction)
        {
            foreach (ReportFileTraceLine reportTraceLine in this.traceLines)
            {
                foreach (ExperimentObserver experimentObserver in reportTraceLine.ExperimentObservers.Where(experimentObserver => experimentObserver != null))
                {
                    experimentObserverAction(experimentObserver);
                }
            }
        }

        private void ProcessTraceLines(Func<TraceTrigger, bool> traceTriggerAction)
        {
            foreach (ReportFileTraceLine traceLine in this.traceLines)
            {
                if (traceTriggerAction(traceLine.TraceTrigger))
                {
                    this.streamWriter.WriteLine(
                        string.Join(this.Separator, traceLine.ExperimentObservers.Select(experimentObserver => experimentObserver.GetReportToken())));
                }
            }
        }

        private List<ReportFileTraceLine> traceLines;
        private string fileName;
        private StreamWriter streamWriter;
    }
}
