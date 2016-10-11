namespace Application.Reporting.DataAccumulators
{
    public class EpisodeAverageValueDataAccumulator : AverageValueDataAccumulator
    {
        public override void EpisodeStarted(DataProvider dataProvider)
        {
            this.value = 0;
            this.count = 0;
        }
    }
}
