using Core.Parameters;

namespace Application.Reporting.ReportTriggers
{
    public class EpisodeCountReportTrigger : ReportTrigger
    {
        [Parameter(1, int.MaxValue)]
        public int EpisodeCountInterval { get; private set; }

        public EpisodeCountReportTrigger()
        {
            EpisodeCountInterval = 1;
        }

        public override bool EpisodeEnded(ExperimentBase experiment)
        {
            return experiment.EpisodeCount % this.EpisodeCountInterval == 0;
        }
    }
}
