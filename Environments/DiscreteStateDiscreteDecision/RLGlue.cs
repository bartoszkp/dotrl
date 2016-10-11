using System.Linq;
using Core;
using Core.Infrastructure;
using Core.Parameters;

namespace Environments.DiscreteStateDiscreteDecision
{
    public class RLGlue : Environment<int, int>
    {
        [Parameter(256, 65536)]
        private int portNumber = 4096;

        public RLGlue()
        {
            this.rlGlueConnectionManager = new RLGlueConnectionManager(this);
        }

        public override EnvironmentDescription<int, int> GetEnvironmentDescription()
        {
            string taskSpecString = this.rlGlueConnectionManager.InitializeEnvironment(portNumber);

            if (string.IsNullOrEmpty(taskSpecString))
            {
                throw new System.InvalidOperationException("Failed to connect with an RL-Glue environment.");
            }

            DotRLGlueCodec.TaskSpec.TaskSpec<int, int> taskSpec
                = (new DotRLGlueCodec.TaskSpec.TaskSpecParser()).Parse(taskSpecString)
                as DotRLGlueCodec.TaskSpec.TaskSpec<int, int>;

            EnvironmentDescription<int, int> result = new EnvironmentDescription<int, int>(
                new SpaceDescription<int>(taskSpec.ObservationMinimumValues.ToArray(), taskSpec.ObservationMaximumValues.ToArray()),
                new SpaceDescription<int>(taskSpec.ActionMinimumValues.ToArray(), taskSpec.ActionMaximumValues.ToArray()),
                new DimensionDescription<double>(taskSpec.ReinforcementMinimumValue, taskSpec.ReinforcementMaximumValue),
                taskSpec.DiscountFactor);

            this.action = new DotRLGlueCodec.Types.Action(result.ActionSpaceDescription.Dimensionality, 0, 0);
            this.CurrentState = new MutableState<int>(result.StateSpaceDescription.Dimensionality);

            return result;
        }

        public override void StartEpisode()
        {
            var observation = this.rlGlueConnectionManager.StartEpisodeEnvironment();

            CurrentState.StateVector = observation.IntArray.ToArray();
        }

        public override Reinforcement PerformAction(Action<int> action)
        {
            this.action.SetIntArray(action.ActionVector.ToArray());

            return this.rlGlueConnectionManager.StepEnvironment(
                this.action,
                isTerminal => this.CurrentState.IsTerminal = isTerminal,
                observation => this.CurrentState.StateVector = observation.IntArray.ToArray());
        }

        public override void ExperimentEnded()
        {
            this.rlGlueConnectionManager.DisconnectEnvironment();
        }

        public override void ParametersChanged()
        {
        }

        public override Component Clone()
        {
            return this;
        }

        private DotRLGlueCodec.Types.Action action;
        private RLGlueConnectionManager rlGlueConnectionManager;
    }
}
