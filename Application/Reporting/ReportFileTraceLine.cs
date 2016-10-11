using System.Collections.Generic;
using Application.Reporting.ExperimentObservers;
using Application.Reporting.TraceTriggers;

namespace Application.Reporting
{
    public class ReportFileTraceLine
    {
        public TraceTrigger TraceTrigger { get; private set; }

        public IEnumerable<ExperimentObserver> ExperimentObservers
        { 
            get 
            {
                return this.experimentObservers; 
            }
        }

        public ReportFileTraceLine(TraceTrigger traceTrigger)
        {
            this.TraceTrigger = traceTrigger;
            this.experimentObservers = new List<ExperimentObserver>();
        }

        public void AddExperimentObserver(ExperimentObserver experimentObserver)
        {
            this.experimentObservers.Add(experimentObserver);
        }

        private List<ExperimentObserver> experimentObservers;
    }
}
