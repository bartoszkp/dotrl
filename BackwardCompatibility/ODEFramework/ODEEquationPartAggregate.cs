using System;
using System.Collections.Generic;

namespace BackwardCompatibility.ODEFramework
{
    /// <summary>
    /// An aggregation of several ODE parts. Note that it is itself an
    /// ODE part.
    /// </summary>
    public abstract class ODEEquationPartAggregate : IODEEquationPart
    {
        protected ODEEquationPartAggregate()
        {
            parts = new List<IODEEquationPart>();
        }

        public virtual void AddPart(IODEEquationPart part)
        {
            parts.Add(part);
        }

        public virtual void RemovePart(IODEEquationPart part)
        {
            parts.Remove(part);
        }

        public virtual void SetODEState(double time, ODEState state)
        {
            int i = 0;
            foreach (IODEEquationPart p in parts)
            {
                int length = p.StateLength;
                double[] partState = new double[length];
                Array.Copy(state.State, i, partState, 0, length);
                i += length;
                p.SetODEState(time, new ODEState(partState));
            }
        }

        public virtual ODEState CurrentODEState
        {
            get
            {
                double[] state = new double[StateLength];
                int i = 0;
                foreach (IODEEquationPart p in parts)
                {
                    int length = p.StateLength;
                    /* Dominated nodes have a 0 length. */
                    if (length > 0)
                    {
                        ODEState partState = p.CurrentODEState;
                        Array.Copy(partState.State, 0, state, i, length);
                        i += length;
                    }
                }

                return new ODEState(state);
            }
        }

        /* (non-Javadoc)
         * @see ODEFramework.ODEEquationPart#getODEStateDerivative()
         */
        public virtual ODEState ODEStateDerivative
        {
            get
            {
                double[] state = new double[StateLength];
                int i = 0;
                foreach (IODEEquationPart p in parts)
                {
                    int length = p.StateLength;
                    /* Dominated nodes have a 0 length. */
                    if (length > 0)
                    {
                        ODEState partDeriv = p.ODEStateDerivative;
                        Array.Copy(partDeriv.State, 0, state, i, length);
                        i += length;
                    }
                }

                return new ODEState(state);
            }
        }

        public virtual int StateLength
        {
            get
            {
                int count = 0;
                foreach (IODEEquationPart p in parts)
                {
                    count += p.StateLength;
                }

                return count;
            }
        }

        private IList<IODEEquationPart> parts;
    }
}