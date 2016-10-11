using System;
using System.Collections.Generic;
using Core;
using Core.Parameters;

namespace Application.Reporting.ReportTriggers
{
    public abstract class ReportTrigger : IParametrizedObject
    {
        public static IEnumerable<Type> GetReportTriggers()
        {
            return TypeUtilities.GetTypes<ReportTrigger>();
        }

        public ReportTrigger Clone()
        {
            return this.GetType().InstantiateWithDefaultConstructor<ReportTrigger>();
        }

        public virtual bool EpisodeStarted(ExperimentBase experiment)
        {
            return false;
        }

        public virtual bool StepDone(ExperimentBase experiment)
        { 
            return false; 
        }

        public virtual bool EpisodeEnded(ExperimentBase experiment)
        {
            return false;
        }

        public virtual void ParametersChanged()
        {
        }
    }
}
