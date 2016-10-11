using Core.Parameters;

namespace Application.Reporting.ReportTriggers
{
    public class EpisodeStepCountReportTrigger : ReportTrigger
    {
        [Parameter(1, int.MaxValue)]
        public int EpisodeStepCountInterval { get; private set; }

        public EpisodeStepCountReportTrigger()
        {
            EpisodeStepCountInterval = 100;
        }
      
        public override bool StepDone(ExperimentBase experiment)
        {
            return experiment.EpisodeStepCount % this.EpisodeStepCountInterval == 0;
        }
    }
}
