namespace Application.Reporting.DataAccumulators
{
    public class TotalValueDataAccumulator : DataAccumulator
    {
        public override void ExperimentStarted(DataProvider dataProvider)
        {
            this.value = 0;
        }
        
        public override void StepDone(DataProvider dataProvider)
        {
            double? currentValue = dataProvider.GetCurrentValue();

            if (!currentValue.HasValue)
            {
                return;
            }

            this.value += currentValue.Value;
        }

        public override double? GetCurrentValue()
        {
            return this.value;
        }

        protected double value;
    }
}
