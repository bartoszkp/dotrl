using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Agents;
using Core;
using DotRLGlueCodec;
using DotRLGlueCodec.TaskSpec;
using DotRLGlueCodec.Types;

namespace Application.Integration.RLGlue
{
    public class RLGlueAgentInterface<TStateSpaceType, TActionSpaceType> : AgentInterface, IRLGlueInterface
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

        public RLGlueAgentInterface(Component agent)
        {
            this.agent = agent as Agent<TStateSpaceType, TActionSpaceType>;
        }

        public void AgentCleanup()
        {
            this.agent = null;
        }

        public void AgentEnd(double reward)
        {
            this.currentReward = reward;
            this.totalReward += reward;
            this.episodeTotalReward += reward;
            this.totalSteps += 1;
            this.episodeSteps += 1; 
            
            this.agent.Learn(new Sample<TStateSpaceType, TActionSpaceType>(
                this.currentState,
                this.currentAction,
                State<TStateSpaceType>.Terminal,
                reward));

            this.currentState = null;
            this.currentAction = null;
        }

        public void AgentInit(string taskSpecification)
        {
            this.currentReward = 0;
            this.totalReward = 0;
            this.episodeTotalReward = 0;
            this.totalSteps = 0;
            this.episodeSteps = 0;

            TaskSpec<TStateSpaceType, TActionSpaceType> taskSpec
                = (new TaskSpecParser()).Parse(taskSpecification)
                as TaskSpec<TStateSpaceType, TActionSpaceType>;

            // TODO: handle data exceptions higher
            if (taskSpec == null)
            {
                throw new InvalidDataException("Incompatible task spec received");
            }

            this.agent.ExperimentStarted(new EnvironmentDescription<TStateSpaceType, TActionSpaceType>(
                new SpaceDescription<TStateSpaceType>(taskSpec.ObservationMinimumValues.ToArray(), taskSpec.ObservationMaximumValues.ToArray()),
                new SpaceDescription<TActionSpaceType>(taskSpec.ActionMinimumValues.ToArray(), taskSpec.ActionMaximumValues.ToArray()),
                new DimensionDescription<double>(taskSpec.ReinforcementMinimumValue, taskSpec.ReinforcementMaximumValue),
                taskSpec.DiscountFactor));
        }

        public string AgentMessage(string message)
        {
            // TODO: visualize
            return string.Empty;
        }

        public Action AgentStart(Observation observation)
        {
            this.episodeTotalReward = 0;
            this.episodeSteps = 0;

            this.currentState = observation.ToDotRL<TStateSpaceType>();
            this.currentAction = this.agent.GetActionWhenLearning(this.currentState);

            return this.currentAction.ToRLGlue();
        }

        public Action AgentStep(double reward, Observation observation)
        {
            this.currentReward = reward;
            this.totalReward += reward;
            this.episodeTotalReward += reward;
            this.totalSteps += 1;
            this.episodeSteps += 1;

            var nextState = observation.ToDotRL<TStateSpaceType>();

            this.agent.Learn(new Sample<TStateSpaceType, TActionSpaceType>(
                this.currentState,
                this.currentAction,
                nextState,
                reward));

            this.currentState = nextState;
            this.currentAction = this.agent.GetActionWhenLearning(this.currentState);

            return this.currentAction.ToRLGlue();
        }

        private State<TStateSpaceType> currentState;
        private Action<TActionSpaceType> currentAction;
        private Agent<TStateSpaceType, TActionSpaceType> agent;
        private double currentReward;
        private double totalReward;
        private double episodeTotalReward;
        private int totalSteps;
        private int episodeSteps;
    }
}
