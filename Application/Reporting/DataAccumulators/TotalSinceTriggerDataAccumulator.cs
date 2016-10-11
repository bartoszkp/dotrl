namespace Application.Reporting.DataAccumulators
{
    public class TotalSinceTriggerDataAccumulator : TotalValueDataAccumulator
    {
        public override void ReportTriggered(DataProvider dataProvider)
        {
            this.value = 0;
        }
    }
}
