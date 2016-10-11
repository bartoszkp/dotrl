using System.Collections.Generic;
using System.Linq;

namespace Application.ExperimentTemplates
{
    public class BatchExperimentTemplate
    {
        public static BatchExperimentTemplate Create(BatchMode batchMode)
        {
            return new BatchExperimentTemplate()
            {
                BatchMode = batchMode
            };
        }

        public static BatchExperimentTemplate Create(BatchExperiment batchExperiment)
        {
            return BatchExperimentTemplate
                .Create(batchExperiment.BatchMode)
                .WithExperimentTemplates(batchExperiment
                                           .Experiments
                                           .Select(e => ExperimentTemplate.Create(e)));
        }

        public BatchMode BatchMode { get; set; }
        public ExperimentTemplate[] ExperimentTemplates { get; set; }

        public BatchExperimentTemplate WithExperimentTemplates(IEnumerable<ExperimentTemplate> experimentTemplates)
        {
            this.ExperimentTemplates = experimentTemplates.ToArray();

            return this;
        }

        public BatchExperiment ToBatchExperiment()
        {
            BatchExperiment result = new BatchExperiment(this.BatchMode);

            result.AddExperiments(ExperimentTemplates.Select(et => et.ToExperiment()));

            return result;
        }
    }
}
