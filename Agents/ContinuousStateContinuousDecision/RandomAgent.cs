using System.Linq;
using Core;

namespace Agents.ContinuousStateContinuousDecision
{
    public class RandomAgent : Agent<double, double>
    {
        public override void ExperimentStarted(EnvironmentDescription<double, double> environmentDescription)
        {
            this.environmentDescription = environmentDescription;
            this.minimumActionValues = this.environmentDescription.ActionSpaceDescription.MinimumValues.ToArray();
            this.maximumActionValues = this.environmentDescription.ActionSpaceDescription.MaximumValues.ToArray();
        }

        public override Action<double> GetActionWhenNotLearning(State<double> currentState)
        {
            double[] newAction = new double[environmentDescription.ActionSpaceDescription.Dimensionality];
            for (int index = 0; index < environmentDescription.ActionSpaceDescription.Dimensionality; index++)
            {
                double min = this.minimumActionValues[index];
                double max = this.maximumActionValues[index];
                newAction[index] = (this.random.NextDouble() * (max - min)) + min;
            }

            return new Action<double>(newAction);
        }

        public override Action<double> GetActionWhenLearning(State<double> currentState)
        {
            return this.GetActionWhenNotLearning(currentState);
        }

        private System.Random random = new System.Random();
        private EnvironmentDescription<double, double> environmentDescription;
        private double[] minimumActionValues;
        private double[] maximumActionValues;
    }
}
