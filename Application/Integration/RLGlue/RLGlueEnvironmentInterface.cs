using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using DotRLGlueCodec;
using DotRLGlueCodec.TaskSpec;
using DotRLGlueCodec.Types;
using Environments;

namespace Application.Integration.RLGlue
{
    public class RLGlueEnvironmentInterface<TStateSpaceType, TActionSpaceType> : EnvironmentInterface, IRLGlueInterface
        where TStateSpaceType : struct
        where TActionSpaceType : struct
    {
        public double CurrentReward
        {
            get 
            { 
                return this.currentReward; 
            }
        }
        
        public double AverageReward 
        { 
            get 
            { 
                return this.totalSteps > 0
                    ? (this.totalReward / this.totalSteps)
                    : 0.0; 
            }
        }
        
        public double EpisodeAverageReward 
        { 
            get 
            {
                return this.episodeSteps > 0
                    ? (this.episodeTotalReward / this.episodeSteps) 
                    : 0.0;
            }
        }

        public RLGlueEnvironmentInterface(Component environment)
        {
            this.environment = environment as Environment<TStateSpaceType, TActionSpaceType>;
        }

        public void EnvironmentCleanup()
        {
            this.environment = null;
        }

        public string EnvironmentInit()
        {
            this.currentReward = 0;
            this.totalReward = 0;
            this.episodeTotalReward = 0;
            this.totalSteps = 0;
            this.episodeSteps = 0;

            var environmentDescription = this.environment.GetEnvironmentDescription();
            TaskSpec<TStateSpaceType, TActionSpaceType> taskSpec = new TaskSpec<TStateSpaceType, TActionSpaceType>(
                environmentDescription.StateSpaceDescription.MinimumValues.ToArray(),
                environmentDescription.StateSpaceDescription.MaximumValues.ToArray(),
                environmentDescription.ActionSpaceDescription.MinimumValues.ToArray(),
                environmentDescription.ActionSpaceDescription.MaximumValues.ToArray(),
                environmentDescription.ReinforcementSpaceDescription.MinimumValue.Value,
                environmentDescription.ReinforcementSpaceDescription.MaximumValue.Value,
                environmentDescription.DiscountFactor,
                string.Empty);

            return (new TaskSpecStringEncoder()).Encode(taskSpec);
        }

        public string EnvironmentMessage(string message)
        {
            // TODO: handle
            return string.Empty;
        }

        public Observation EnvironmentStart()
        {
            this.episodeTotalReward = 0;
            this.episodeSteps = 0;

            this.environment.StartEpisode();

            return this.environment.GetCurrentState().ToRLGlue<TStateSpaceType>();
        }

        public RewardObservationTerminal EnvironmentStep(Action action)
        {
            double reinforcement = this.environment.PerformAction(action.ToDotRL<TActionSpaceType>());

            this.currentReward = reinforcement;
            this.totalReward += reinforcement;
            this.episodeTotalReward += reinforcement;
            this.totalSteps += 1;
            this.episodeSteps += 1;

            var currentState = this.environment.GetCurrentState();
            
            return new RewardObservationTerminal()
            {
                Reward = reinforcement,
                Observation = currentState.ToRLGlue<TStateSpaceType>(),
                Terminal = currentState.IsTerminal
            };
        }

        private Environment<TStateSpaceType, TActionSpaceType> environment;
        private double currentReward;
        private double totalReward;
        private double episodeTotalReward;
        private int totalSteps;
        private int episodeSteps;
    }
}
