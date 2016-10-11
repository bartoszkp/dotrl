using System;
using System.Collections.Generic;
using Core;
using Core.Parameters;

namespace Application.Reporting.DataAccumulators
{
    public abstract class DataAccumulator : IParametrizedObject
    {
        public static IEnumerable<Type> GetDataAccumulators()
        {
            return TypeUtilities.GetTypes<DataAccumulator>();
        }

        public DataAccumulator Clone()
        {
            return this.GetType().InstantiateWithDefaultConstructor<DataAccumulator>();
        }

        public abstract double? GetCurrentValue();

        public virtual void ReportTriggered(DataProvider dataProvider)
        {
        }

        public virtual void ExperimentStarted(DataProvider dataProvider)
        {
        }

        public virtual void EpisodeStarted(DataProvider dataProvider)
        {
        }

        public virtual void StepDone(DataProvider dataProvider)
        {
        }

        public virtual void EpisodeEnded(DataProvider dataProvider)
        {
        }

        public virtual void ParametersChanged()
        {
        }
    }
}
