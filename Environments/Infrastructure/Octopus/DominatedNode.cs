using BackwardCompatibility;
using BackwardCompatibility.ODEFramework;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// A node whose position and velocity are set using setters and whose update
    /// method does nothing.
    /// </summary>
    internal class DominatedNode : Node
    {
        /// <seealso cref= Node#Node(Vector2D) </seealso>
        public DominatedNode(double mass, Vector2D initialPosition, Vector2D initialVelocity)
            : base(mass, initialPosition, initialVelocity)
        {
        }

        public void SetPosition(Vector2D position)
        {
            Position = position;
        }

        public void SetVelocity(Vector2D velocity)
        {
            Velocity = velocity;
        }

        public override ODEState CurrentODEState
        {
            get
            {
                return null;
            }
        }

        public override ODEState ODEStateDerivative
        {
            get
            {
                return null;
            }
        }

        public override int StateLength
        {
            get
            {
                return 0;
            }
        }

        public override void SetODEState(double time, ODEState state)
        {
        }
    }
}