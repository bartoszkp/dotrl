using Core;

namespace Agents 
{
    public abstract class Agent<TStateSpaceType, TActionSpaceType> : Component
        where TStateSpaceType : struct
        where TActionSpaceType : struct
    {
        public override ComponentType ComponentType
        { 
            get 
            {
                return new ComponentType(typeof(TStateSpaceType), typeof(TActionSpaceType)); 
            }
        }

        public abstract void ExperimentStarted(EnvironmentDescription<TStateSpaceType, TActionSpaceType> environmentDescription);

        public abstract Action<TActionSpaceType> GetActionWhenNotLearning(State<TStateSpaceType> currentState);

        public virtual void EpisodeStarted(State<TStateSpaceType> currentState)
        {
        }

        public abstract Action<TActionSpaceType> GetActionWhenLearning(State<TStateSpaceType> currentState);

        public virtual void Learn(Sample<TStateSpaceType, TActionSpaceType> sample)
        {
        }

        public virtual void EpisodeEnded()
        {
        }

        public virtual void ExperimentEnded()
        {
        }

        public override void ParametersChanged()
        {
        }

        protected MutableAction<TActionSpaceType> Action { get; set; }
    }
}
