using Core.Parameters;

namespace Application.Reporting.ReportTriggers
{
    class StepCountReportTrigger : ReportTrigger
    {
        [Parameter(1, int.MaxValue)]
        public int StepCountInterval { get; private set; }

        public StepCountReportTrigger()
        {
            StepCountInterval = 100;
        }

        public override bool StepDone(ExperimentBase experiment)
        {
            return experiment.TotalStepCount % this.StepCountInterval == 0;
        }
    }
}
