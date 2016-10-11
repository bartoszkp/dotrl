using Core;
using Core.Parameters;

namespace Environments
{
    public abstract class Environment<TStateSpaceType, TActionSpaceType> : Component
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

        public abstract void StartEpisode();

        public abstract Reinforcement PerformAction(Action<TActionSpaceType> action);

        public abstract EnvironmentDescription<TStateSpaceType, TActionSpaceType> GetEnvironmentDescription();

        public virtual State<TStateSpaceType> GetCurrentState()
        {
            return CurrentState;
        }

        public virtual void ExperimentEnded()
        {
        }

        public override void ParametersChanged()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "GetCurrentState method returns the current object state as immutable object. CurrentState property exposes the same object so it's not confusing. The same object is exposed in two ways, so subclasses can access directly the mutable version, for performance reasons.")]
        protected MutableState<TStateSpaceType> CurrentState { get; set; }
    }
}
