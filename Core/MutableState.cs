using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Core
{
    public class MutableState<TStateSpaceType> : State<TStateSpaceType>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "This class intends to allow modification of the stateVector for performance reasons.")]
        public new TStateSpaceType[] StateVector
        { 
            get 
            {
                return this.stateVector; 
            }
            set
            {
                this.stateVector = value;
            }
        }

        public new TStateSpaceType this[int index]
        {
            get
            {
                return this.stateVector[index];
            }
            set
            {
                this.stateVector[index] = value;
            }
        }

        public new TStateSpaceType SingleValue
        {
            get
            {
                return base.SingleValue;
            }
            set
            {
                if (this.stateVector.Length != 1)
                {
                    throw new System.InvalidOperationException("SingleValue property is valid only for one dimensional states");
                }

                this.stateVector[0] = value;
            }
        }

        public new bool IsTerminal
        {
            get
            {
                return base.IsTerminal;
            }
            set
            {
                base.IsTerminal = value;
            }
        }

        public MutableState(int dimensionality)
            : this(new TStateSpaceType[dimensionality])
        {
        }

        /// <summary>
        /// Makes its own copy of stateVector.
        /// </summary>
        public MutableState(IEnumerable<TStateSpaceType> stateVector)
            : this(stateVector.ToArray())
        {
        }

        /// <summary>
        /// Takes ownership of stateVector.
        /// </summary>
        public MutableState(TStateSpaceType[] stateVector)
            : base(stateVector)
        {
        }

        public void CopyFrom(State<TStateSpaceType> other)
        {
            Contract.Requires(other.Dimensionality == this.Dimensionality);

            int index = 0;
            foreach (var value in other.StateVector)
            {
                this.stateVector[index] = value;
                index += 1;
            }

            this.IsTerminal = other.IsTerminal;

            Contract.Requires(index == this.stateVector.Length);
        }
    }
}
