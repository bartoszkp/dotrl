using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class State<TStateSpaceType>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "State<> class is immutable, so the readonly modifier is valid.")]
        public static readonly State<TStateSpaceType> Terminal = new State<TStateSpaceType>(null) { IsTerminal = true };

        public static State<TStateSpaceType> WithSingleValue(TStateSpaceType value)
        {
            return new State<TStateSpaceType>(new[] { value });
        }

        public IEnumerable<TStateSpaceType> StateVector 
        { 
            get 
            {
                return this.stateVector;
            }
        }

        public TStateSpaceType this[int index]
        {
            get
            {
                return this.stateVector[index];
            }
        }

        public TStateSpaceType SingleValue
        {
            get
            {
                if (this.stateVector.Length != 1)
                {
                    throw new System.InvalidOperationException("SingleValue property is valid only for one dimensional states");
                }

                return this.stateVector.Single();
            }
        }

        public int Dimensionality
        {
            get
            {
                return this.stateVector.Length;
            }
        }
        
        public bool IsTerminal { get; protected set; }

        /// <summary>
        /// Makes its own copy of stateVector.
        /// </summary>
        public State(IEnumerable<TStateSpaceType> stateVector)
            : this(stateVector.ToArray())
        {
        }

        /// <summary>
        /// Takes ownership of stateVector.
        /// </summary>
        public State(TStateSpaceType[] stateVector)
        {
            this.stateVector = stateVector;
            this.IsTerminal = false;
        }

        public State<TStateSpaceType> Copy()
        {
            return new State<TStateSpaceType>(stateVector as IEnumerable<TStateSpaceType>)
            {
                IsTerminal = IsTerminal
            };
        }

        protected TStateSpaceType[] stateVector;
    }
}
