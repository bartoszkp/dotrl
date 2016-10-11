using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application
{
    public class BatchExperiment
    {
        public BatchMode BatchMode { get; set; }

        public IEnumerable<ExperimentBase> Experiments
        {
            get
            {
                return this.experiments;
            }
        }

        public BatchExperiment(BatchMode batchMode)
        {
            this.BatchMode = batchMode;
            this.experiments = new List<ExperimentBase>();
        }

        public void AddExperiments(IEnumerable<ExperimentBase> experiments)
        {
            this.experiments.AddRange(experiments);
        }

        private List<ExperimentBase> experiments;
    }
}
