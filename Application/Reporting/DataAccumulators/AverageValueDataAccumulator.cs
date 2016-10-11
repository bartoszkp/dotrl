namespace Application.Reporting.DataAccumulators
{
    public class AverageValueDataAccumulator : DataAccumulator
    {
        public override void ExperimentStarted(DataProvider dataProvider)
        {
            this.value = 0;
            this.count = 0;
        }
        
        public override void StepDone(DataProvider dataProvider)
        {
            double? currentValue = dataProvider.GetCurrentValue();

            if (!currentValue.HasValue)
            {
                return;
            }

            this.value += currentValue.Value;
            this.count += 1;
        }

        public override double? GetCurrentValue()
        {
            if (count == 0)
            {
                return null;
            }

            return this.value / count;
        }

        protected double value;
        protected int count;
    }
}
