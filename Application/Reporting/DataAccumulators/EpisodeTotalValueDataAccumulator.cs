namespace Application.Reporting.DataAccumulators
{
    public class EpisodeTotalValueDataAccumulator : TotalValueDataAccumulator
    {
        public override void EpisodeStarted(DataProvider dataProvider)
        {
            this.value = 0;
        }
    }
}
