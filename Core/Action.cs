using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Action<TActionSpaceType>
    {
        public static Action<TActionSpaceType> WithSingleValue(TActionSpaceType value)
        {
            return new Action<TActionSpaceType>(new[] { value });
        }

        public IEnumerable<TActionSpaceType> ActionVector
        {
            get 
            { 
                return this.actionVector; 
            }
        }

        public int Dimensionality
        {
            get
            {
                return this.actionVector.Length;
            }
        }

        public TActionSpaceType this[int index]
        {
            get
            {
                return this.actionVector[index];
            }
        }

        public TActionSpaceType SingleValue
        {
            get
            {
                if (this.actionVector.Length != 1)
                {
                    throw new System.InvalidOperationException("SingleValue property is valid only for one dimensional actions");
                }

                return this.actionVector.Single();
            }
        }

        /// <summary>
        /// Makes its own copy of actionVector.
        /// </summary>
        public Action(IEnumerable<TActionSpaceType> actionVector)
            : this(actionVector.ToArray())
        {
        }

        /// <summary>
        /// Takes ownership of stateVector.
        /// </summary>
        public Action(TActionSpaceType[] actionVector)
        {
            this.actionVector = actionVector;
        }

        protected TActionSpaceType[] actionVector;
    }
}
