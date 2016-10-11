namespace Application.Reporting.DataAccumulators
{
    public class AverageSinceTriggerDataAccumulator : AverageValueDataAccumulator
    {
        public override void ReportTriggered(DataProvider dataProvider)
        {
            this.value = 0;
            this.count = 0;
        }
    }
}
