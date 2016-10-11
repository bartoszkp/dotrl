using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Sample<TStateSpaceType, TActionSpaceType> : ISample
    {
        public State<TStateSpaceType> PreviousState
        {
            get
            {
                return previousState;
            }
        }

        public Action<TActionSpaceType> Action
        {
            get
            {
                return action;
            }
        }

        public State<TStateSpaceType> CurrentState
        {
            get
            {
                return currentState;
            }
        }

        public Reinforcement Reinforcement { get; protected set; }

        /// <summary>
        /// Takes ownership of previousState, currentState and action.
        /// </summary>
        public Sample(State<TStateSpaceType> previousState, Action<TActionSpaceType> action, State<TStateSpaceType> currentState, Reinforcement reinforcement)
        {
            this.previousState = previousState;
            this.action = action;
            this.currentState = currentState;
            this.Reinforcement = reinforcement;
        }

        public IEnumerable<double> GetPreviousStateVector()
        {
            if (this.previousState == null)
            {
                return null;
            }

            return this.previousState.StateVector.Cast<double>();
        }

        public IEnumerable<double> GetActionVector()
        {
            if (this.action == null)
            {
                return null;
            }

            return this.action.ActionVector.Cast<double>();
        }

        public IEnumerable<double> GetCurrentStateVector()
        {
            if (this.currentState == null)
            {
                return null;
            }

            return this.currentState.StateVector.Cast<double>();
        }

        protected State<TStateSpaceType> previousState;
        protected Action<TActionSpaceType> action;
        protected State<TStateSpaceType> currentState;
    }
}
