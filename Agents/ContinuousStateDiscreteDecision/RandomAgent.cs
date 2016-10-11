using System.Linq;
using Core;

namespace Agents.ContinuousStateDiscreteDecision
{
    public class RandomAgent : Agent<double, int>
    {
        public override void ExperimentStarted(EnvironmentDescription<double, int> environmentDescription)
        {
            this.environmentDescription = environmentDescription;
            this.minimumActionValues = this.environmentDescription.ActionSpaceDescription.MinimumValues.ToArray();
            this.maximumActionValues = this.environmentDescription.ActionSpaceDescription.MaximumValues.ToArray();
        }

        public override Action<int> GetActionWhenNotLearning(State<double> currentState)
        {
            int[] newAction = new int[environmentDescription.ActionSpaceDescription.Dimensionality];
            for (int index = 0; index < environmentDescription.ActionSpaceDescription.Dimensionality; index++)
            {
                int min = this.minimumActionValues[index];
                int max = this.maximumActionValues[index];
                newAction[index] = (this.random.Next(max - min + 1) + min);
            }

            return new Action<int>(newAction);
        }

        public override Action<int> GetActionWhenLearning(State<double> currentState)
        {
            return this.GetActionWhenNotLearning(currentState);
        }

        private System.Random random = new System.Random();
        private EnvironmentDescription<double, int> environmentDescription;
        private int[] minimumActionValues;
        private int[] maximumActionValues;
    }
}
