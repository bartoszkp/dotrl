using System.Diagnostics;
using BackwardCompatibility;

namespace BackwardCompatibility.ODEFramework
{
    /// <summary>
    /// Represents a point mass in 2 dimensions, it has a mass, position, velocity and acceleration
    /// </summary>
    public abstract class PointMass2D : IODEEquationPart
    {
        private const int PointMassStateLength = 4;

        public virtual int StateLength 
        {
            get 
            {
                return PointMassStateLength; 
            }
        }

        public Vector2D Position { get; protected set; }

        public Vector2D Velocity { get; protected set; }

        public double Mass { get; private set; }

        public double Time { get; private set; }

        protected PointMass2D(double mass, Vector2D position, Vector2D velocity)
        {
            Mass = mass;
            Position = position;
            Velocity = velocity;
            Time = 0;
        }

        public abstract Vector2D NetForce { get; }

        /// <summary>
        /// The state is returned as mass, positionX, positionY, velocityX, velocityY </summary>
        /// <seealso cref= ODEFramework.ODEEquationPart#getCurrentODEState() </seealso>
        public virtual ODEState CurrentODEState
        {
            get
            {
                double[] state = new double[PointMassStateLength];
                state[0] = Position.PositionX;
                state[1] = Position.PositionY;
                state[2] = Velocity.PositionX;
                state[3] = Velocity.PositionY;
                return new ODEState(state);
            }
        }

        public virtual ODEState ODEStateDerivative
        {
            get
            {
                double[] deriv = new double[PointMassStateLength];
                deriv[0] = Velocity.PositionX;
                deriv[1] = Velocity.PositionY;

                // The actual computation of the forces
                Vector2D accel = NetForce.Scale(1 / Mass);
                deriv[2] = accel.PositionX;
                deriv[3] = accel.PositionY;
                return new ODEState(deriv);
            }
        }

        public virtual void SetODEState(double time, ODEState state)
        {
            double[] s = state.State;
            Debug.Assert(s.Length == PointMassStateLength);
            Position = new Vector2D(s[0], s[1]);
            Velocity = new Vector2D(s[2], s[3]);
            Time = time;
        }
    }
}