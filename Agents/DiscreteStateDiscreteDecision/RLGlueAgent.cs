using System.Linq;
using Core;
using Core.Infrastructure;
using Core.Parameters;

namespace Agents.DiscreteStateDiscreteDecision
{
    public class RLGlueAgent : Agent<int, int>
    {
        [Parameter(256, 65536)]
        private int portNumber = 4096;

        public RLGlueAgent()
        {
            this.rlGlueConnectionManager = new RLGlueConnectionManager(this);
        }

        public override void ExperimentStarted(EnvironmentDescription<int, int> environmentDescription)
        {
            DotRLGlueCodec.TaskSpec.TaskSpec<int, int> taskSpec = new DotRLGlueCodec.TaskSpec.TaskSpec<int, int>(
                environmentDescription.StateSpaceDescription.MinimumValues,
                environmentDescription.StateSpaceDescription.MaximumValues,
                environmentDescription.ActionSpaceDescription.MinimumValues,
                environmentDescription.ActionSpaceDescription.MaximumValues,
                environmentDescription.ReinforcementSpaceDescription.MinimumValue ?? 0,
                environmentDescription.ReinforcementSpaceDescription.MaximumValue ?? 0,
                environmentDescription.DiscountFactor,
                string.Empty);

            string taskSpecString = (new DotRLGlueCodec.TaskSpec.TaskSpecStringEncoder()).Encode(taskSpec); 
            
            if (!this.rlGlueConnectionManager.InitializeAgent(portNumber, taskSpecString))
            {
                throw new System.InvalidOperationException("Failed to connect with an RL-Glue agent.");
            }

            this.previousReinforcement = 0;
            this.rlGlueEpisodeStarted = false;

            this.observation = new DotRLGlueCodec.Types.Observation(environmentDescription.StateSpaceDescription.Dimensionality, 0, 0);
            this.Action = new MutableAction<int>(environmentDescription.ActionSpaceDescription.Dimensionality);
        }

        public override Action<int> GetActionWhenNotLearning(State<int> currentState)
        {
            // In RL-Glue there is currently no standard way to instruct agent to stop learning
            // so unfortunately, presentation mode may affect agent's learning state
            StartRlGlueEpisode(currentState);

            return this.Action;
        }

        public override Action<int> GetActionWhenLearning(State<int> currentState)
        {
            if (!this.rlGlueEpisodeStarted)
            {
                this.StartRlGlueEpisode(currentState);
                this.rlGlueEpisodeStarted = true;
            }
            else
            {
                this.RlGlueStep(this.previousReinforcement, currentState);
            }

            this.previousReinforcement = 0;

            return this.Action;
        }

        public override void Learn(Sample<int, int> sample)
        {
            this.previousReinforcement = sample.Reinforcement;
        }

        public override void EpisodeEnded()
        {
            this.EndRlGlueEpisode(this.previousReinforcement);
            this.previousReinforcement = 0;
        }

        public override void ExperimentEnded()
        {
            if (this.rlGlueEpisodeStarted)
            {
                this.EndRlGlueEpisode(this.previousReinforcement);
            }

            this.rlGlueConnectionManager.DisconnectAgent();
        }

        public override void ParametersChanged()
        {
        }

        private void StartRlGlueEpisode(State<int> state)
        {
            observation.SetIntArray(state.StateVector.ToArray());

            var action = this.rlGlueConnectionManager.StartEpisodeAgent(observation);
            
            this.Action.ActionVector = action.IntArray.ToArray();
        }

        private void EndRlGlueEpisode(double reinforcement)
        {
            this.rlGlueConnectionManager.EndEpisodeAgent(reinforcement);
            this.rlGlueEpisodeStarted = false;
        }

        private void RlGlueStep(double reinforcement, State<int> state)
        {
            observation.SetIntArray(state.StateVector.ToArray());

            var action = this.rlGlueConnectionManager.StepAgent(observation, reinforcement);

            this.Action.ActionVector = action.IntArray.ToArray();
        }

        private DotRLGlueCodec.Types.Observation observation;
        private RLGlueConnectionManager rlGlueConnectionManager;
        private bool rlGlueEpisodeStarted;
        private double previousReinforcement;
    }
}
