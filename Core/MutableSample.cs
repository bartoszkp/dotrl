using System.Diagnostics.Contracts;

namespace Core
{
    public class MutableSample<TStateSpaceType, TActionSpaceType> : Sample<TStateSpaceType, TActionSpaceType>
    {
        public new MutableState<TStateSpaceType> PreviousState
        {
            get
            {
                return this.mutablePreviousState;
            }
        }

        public new MutableAction<TActionSpaceType> Action
        {
            get
            {
                return this.mutableAction;
            }
        }

        public new MutableState<TStateSpaceType> CurrentState
        {
            get
            {
                return this.mutableCurrentState;
            }
        }

        public new Reinforcement Reinforcement 
        {
            get
            {
                return base.Reinforcement;
            }
            set
            {
                base.Reinforcement = value;
            }
        }

        public MutableSample(int stateDimensionality, int actionDimensionality)
            : base(
            new MutableState<TStateSpaceType>(stateDimensionality),
            new MutableAction<TActionSpaceType>(actionDimensionality),
            new MutableState<TStateSpaceType>(stateDimensionality),
            0)
        {
            Contract.Requires(currentState != null);

            this.mutablePreviousState = this.previousState as MutableState<TStateSpaceType>;
            this.mutableAction = this.action as MutableAction<TActionSpaceType>;
            this.mutableCurrentState = this.currentState as MutableState<TStateSpaceType>;
        }

        private MutableState<TStateSpaceType> mutablePreviousState;
        private MutableAction<TActionSpaceType> mutableAction;
        private MutableState<TStateSpaceType> mutableCurrentState;
    }
}
