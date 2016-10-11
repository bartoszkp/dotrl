using System.Diagnostics.Contracts;

namespace Core
{
    public class DimensionDescription<TSpaceType>
        where TSpaceType : struct
    {
        public TSpaceType? MinimumValue { get; private set; }

        public TSpaceType? MaximumValue { get; private set; }

        public TSpaceType? AverageValue { get; private set; }

        public TSpaceType? StandardDeviation { get; private set; }

        public DimensionDescription(TSpaceType? minimumValue, TSpaceType? maximumValue)
        {
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
            this.AverageValue = null;
            this.StandardDeviation = null;
        }

        public DimensionDescription(TSpaceType? minimumValue, TSpaceType? maximumValue, TSpaceType? averageValue, TSpaceType? standardDeviation)
        {
            this.MinimumValue = minimumValue;
            this.MaximumValue = maximumValue;
            this.AverageValue = averageValue;
            this.StandardDeviation = standardDeviation;
        }
    }
}
