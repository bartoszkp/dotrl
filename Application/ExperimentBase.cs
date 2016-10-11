using Application.Reporting;
using Core;
using Core.Parameters;
using Core.Reporting;

namespace Application
{
    public abstract class ExperimentBase : IParametrizedObject
    {
        public static ExperimentBase Instantiate(Component environment, Component presentationEnvironment, Component agent)
        {
            environment.UserInitializationActionManager = new UserInitializationActionWindow();
            agent.UserInitializationActionManager = new UserInitializationActionWindow();

            System.Type environmentType = environment.GetType();
            System.Type agentType = agent.GetType();
           
            System.Type[] componentType = environmentType.BaseType.GetGenericArguments();

            var experimentType = typeof(Experiment<,>).MakeGenericType(componentType);

            return experimentType
                .GetConstructor(new[] { environmentType, environmentType, agentType })
                .Invoke(new object[] { environment, presentationEnvironment, agent }) as ExperimentBase;
        }

        public Reporter Reporter { get; set; }

        [ReportedValue]
        public double? CurrentReinforcement
        {
            get 
            {
                if (CurrentSample == null)
                {
                    return null;
                }

                return CurrentSample.Reinforcement;
            }
        }

        public abstract Component Agent { get; }

        public abstract Component Environment { get; }

        public abstract Component PresentationEnvironment { get; }

        public abstract int EpisodeStepCountLimit { get; protected set; }

        public abstract int EpisodeCountLimit { get; protected set; }

        public abstract int TotalStepCountLimit { get; protected set; }

        public abstract int EpisodeStepCount { get; protected set; }

        public abstract int TotalStepCount { get; protected set; }

        public abstract int EpisodeCount { get; protected set; }

        public abstract bool IsEndOfEpisode { get; }

        public abstract bool IsEndOfExperiment { get; }

        public abstract ISample CurrentSample { get; }

        public abstract void Initialize();

        public abstract void StartPresentationEpisode();

        public abstract void DoPresentationStep();

        public abstract void DoStep();

        public abstract void End();

        public ExperimentBase()
        {
            this.Reporter = new Reporter();
        }

        public ExperimentBase Clone()
        {
            Component environment = this.Environment.Clone();
            Component presentationEnvironment = this.PresentationEnvironment.Clone();
            Component agent = this.Agent.Clone();

            ExperimentBase result = ExperimentBase.Instantiate(environment, presentationEnvironment, agent);

            Reporter reporter = this.Reporter.CloneFor(result);

            result.Reporter = reporter;

            result.CopyParametersFrom(this);

            return result;
        }

        public virtual void ParametersChanged()
        {
        }
    }
}
