using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Core
{
    public class MutableAction<TActionSpaceType> : Action<TActionSpaceType>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "This class intends to allow modification of the actionVector for performance reasons.")]
        public new TActionSpaceType[] ActionVector
        {
            get
            {
                return this.actionVector;
            }
            set
            {
                this.actionVector = value;
            }
        }

        public new TActionSpaceType SingleValue
        {
            get
            {
                return base.SingleValue;
            }
            set
            {
                if (this.actionVector.Length != 1)
                {
                    throw new System.InvalidOperationException("SingleValue property is valid only for one dimensional actions");
                }

                this.actionVector[0] = value;
            }
        }

        public MutableAction(int dimensionality)
            : this(new TActionSpaceType[dimensionality])
        {
        }

        /// <summary>
        /// Makes its own copy of stateVector.
        /// </summary>
        public MutableAction(IEnumerable<TActionSpaceType> actionVector)
            : this(actionVector.ToArray())
        {
        }

        /// <summary>
        /// Takes ownership of stateVector.
        /// </summary>
        public MutableAction(TActionSpaceType[] actionVector)
            : base(actionVector)
        {
        }

        public void CopyFrom(Action<TActionSpaceType> other)
        {
            Contract.Requires(other.Dimensionality == this.Dimensionality);

            int index = 0;
            foreach (var value in other.ActionVector)
            {
                this.actionVector[index] = value;
                index += 1;
            }
        }
    }
}
