namespace Application.Reporting.DataAccumulators
{
    public class CurrentValueDataAccumulator : DataAccumulator
    {
        public override void ExperimentStarted(DataProvider dataProvider)
        {
            this.value = null;
        }
        
        public override void EpisodeStarted(DataProvider dataProvider)
        {
            this.value = dataProvider.GetCurrentValue();
        }

        public override void StepDone(DataProvider dataProvider)
        {
            this.value = dataProvider.GetCurrentValue();
        }

        public override void EpisodeEnded(DataProvider dataProvider)
        {
            this.value = dataProvider.GetCurrentValue();
        }

        public override double? GetCurrentValue()
        {
            return this.value;
        }

        private double? value;
    }
}
