using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Core
{
    public class SpaceDescription<TSpaceType>
        where TSpaceType : struct
    {
        public static SpaceDescription<TSpaceType> CreateOneDimensionalSpaceDescription(TSpaceType? minimumValue, TSpaceType? maximumValue)
        {
            return new SpaceDescription<TSpaceType>(new[] { new DimensionDescription<TSpaceType>(minimumValue, maximumValue) });
        }

        public static SpaceDescription<TSpaceType> CreateOneDimensionalSpaceDescription(TSpaceType? minimumValue, TSpaceType? maximumValue, TSpaceType? averageValue, TSpaceType? standardDeviation)
        {
            return new SpaceDescription<TSpaceType>(new[] { new DimensionDescription<TSpaceType>(minimumValue, maximumValue, averageValue, standardDeviation) });
        }

        public int Dimensionality { get; private set; }

        public IEnumerable<DimensionDescription<TSpaceType>> DimensionDescriptions { get; private set; }

        public IEnumerable<TSpaceType> MinimumValues
        {
            get
            {
                return DimensionDescriptions.Select(dd => dd.MinimumValue.Value);
            }
        }

        public IEnumerable<TSpaceType> MaximumValues
        {
            get
            {
                return DimensionDescriptions.Select(dd => dd.MaximumValue.Value);
            }
        }

        public IEnumerable<TSpaceType> AverageValues
        {
            get
            {
                return DimensionDescriptions.Select(dd => dd.AverageValue.Value);
            }
        }

        public IEnumerable<TSpaceType> StandardDeviations
        {
            get
            {
                return DimensionDescriptions.Select(dd => dd.StandardDeviation.Value);
            }
        }

        public SpaceDescription(TSpaceType[] minimumValues, TSpaceType[] maximumValues)
            : this(minimumValues, maximumValues, null, null)
        {
        }

        public SpaceDescription(TSpaceType[] minimumValues, TSpaceType[] maximumValues, TSpaceType[] averageValues, TSpaceType[] standardDeviations)
        {
            Contract.Requires(
                (new[] { minimumValues, maximumValues, averageValues, standardDeviations })
                .Where(array => array != null)
                .Select(array => array.Length)
                .Distinct()
                .Count() == 1, "Given arrays must be null or have the same length");
            Contract.Requires(
                minimumValues != null || maximumValues != null || averageValues != null || standardDeviations != null,
                "At least one given vector must not be null");

            this.Dimensionality = (new[] { minimumValues, maximumValues, averageValues, standardDeviations })
                .Where(array => array != null)
                .First()
                .Length;

            DimensionDescriptions = Enumerable
                .Range(0, this.Dimensionality)
                .Select(d => new DimensionDescription<TSpaceType>(
                    minimumValues == null ? (TSpaceType?)null : minimumValues[d],
                    maximumValues == null ? (TSpaceType?)null : maximumValues[d],
                    averageValues == null ? (TSpaceType?)null : averageValues[d],
                    standardDeviations == null ? (TSpaceType?)null : standardDeviations[d]))
                .ToArray();
        }

        public SpaceDescription(DimensionDescription<TSpaceType>[] dimensionDescriptions)
        {
            this.Dimensionality = dimensionDescriptions.Length;

            DimensionDescriptions = dimensionDescriptions;
        }
    }
}
