using Agents;
using Core;
using Core.Parameters;
using Core.Reporting;
using Environments;

namespace Application
{
    public class Experiment<TStateType, TActionType> : ExperimentBase
        where TStateType : struct
        where TActionType : struct
    {
        public override Component Agent
        {
            get
            {
                return this.agent;
            }
        }

        public override Component Environment
        {
            get 
            {
                return this.environment;
            }
        }

        public override Component PresentationEnvironment
        {
            get
            {
                return this.presentationEnvironment;
            }
        }

        [Parameter(0, int.MaxValue)]
        public override int EpisodeStepCountLimit { get; protected set; }

        [Parameter(0, int.MaxValue)]
        public override int EpisodeCountLimit { get; protected set; }

        [Parameter(0, int.MaxValue)]
        public override int TotalStepCountLimit { get; protected set; }
        
        [ReportedValue]
        public override int EpisodeStepCount { get; protected set; }

        [ReportedValue]
        public override int TotalStepCount { get; protected set; }

        [ReportedValue]
        public override int EpisodeCount { get; protected set; }

        public override bool IsEndOfEpisode
        {
            get
            {
                return (this.currentSample != null && this.currentSample.CurrentState.IsTerminal)
                    || (this.EpisodeStepCountLimit > 0 && this.EpisodeStepCount >= this.EpisodeStepCountLimit)
                    || (this.TotalStepCountLimit > 0 && this.TotalStepCount >= this.TotalStepCountLimit);
            }
        }

        public override bool IsEndOfExperiment
        {
            get
            {
                return (this.EpisodeCountLimit > 0 && this.EpisodeCount >= this.EpisodeCountLimit)
                    || (this.TotalStepCountLimit > 0 && this.TotalStepCount >= this.TotalStepCountLimit);
            }
        }

        public override ISample CurrentSample
        {
            get
            {
                return this.currentSample;
            }
        }

        public Experiment(
            Environment<TStateType, TActionType> environment, 
            Environment<TStateType, TActionType> presentationEnvironment,
            Agent<TStateType, TActionType> agent)
        {
            EpisodeStepCountLimit = 500;
            EpisodeCountLimit = 2000;
            TotalStepCountLimit = 0;

            this.agent = agent;
            this.environment = environment;
            this.presentationEnvironment = presentationEnvironment;
        }

        public override void Initialize()
        {
            EpisodeStepCount = 0;
            TotalStepCount = 0;
            EpisodeCount = 0;

            EnvironmentDescription<TStateType, TActionType> environmentDescription = this.environment.GetEnvironmentDescription();
            this.agent.ExperimentStarted(environmentDescription);

            this.Reporter.ExperimentStarted(this);

            this.StartEpisode();
            this.presentationEnvironment.StartEpisode();

            this.currentSample = new MutableSample<TStateType, TActionType>(
                environmentDescription.StateSpaceDescription.Dimensionality,
                environmentDescription.ActionSpaceDescription.Dimensionality);
        }

        // TODO: presentation methods should be moved to a presenter entity, they dont belong here
        public override void StartPresentationEpisode()
        {
            this.presentationEnvironment.StartEpisode();
        }

        public override void DoPresentationStep()
        {
            if (this.presentationEnvironment.GetCurrentState().IsTerminal)
            {
                this.presentationEnvironment.StartEpisode();
            }

            Action<TActionType> action = this.agent.GetActionWhenNotLearning(this.presentationEnvironment.GetCurrentState());
            this.presentationEnvironment.PerformAction(action);
        }

        public override void DoStep()
        {
            if (this.IsEndOfEpisode)
            {
                this.EndEpisode();
                this.StartEpisode();
            }

            State<TStateType> previousState = this.environment.GetCurrentState();           
            this.currentSample.PreviousState.CopyFrom(previousState);
            
            Action<TActionType> action = this.agent.GetActionWhenLearning(previousState);
            this.currentSample.Action.CopyFrom(action);
            
            Reinforcement reinforcement = this.environment.PerformAction(action);
            this.currentSample.Reinforcement = reinforcement;

            State<TStateType> currentState = this.environment.GetCurrentState();
            this.currentSample.CurrentState.CopyFrom(currentState);

            this.agent.Learn(currentSample);

            this.EpisodeStepCount += 1;
            this.TotalStepCount += 1;

            this.Reporter.StepDone(this);
        }

        public override void End()
        {
            this.agent.ExperimentEnded();
            this.environment.ExperimentEnded();
            this.Reporter.Dispose();
        }

        private void StartEpisode()
        {
            this.EpisodeStepCount = 0;
            this.environment.StartEpisode();
            this.agent.EpisodeStarted(this.environment.GetCurrentState());

            this.Reporter.EpisodeStarted(this);
        }

        private void EndEpisode()
        {
            this.EpisodeCount += 1;
            this.agent.EpisodeEnded();

            this.Reporter.EpisodeEnded(this);
        }

        private Agent<TStateType, TActionType> agent;

        private Environment<TStateType, TActionType> environment;

        private Environment<TStateType, TActionType> presentationEnvironment;

        private MutableSample<TStateType, TActionType> currentSample;
    }
}
