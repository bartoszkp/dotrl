using System;
using System.Diagnostics;
using BackwardCompatibility;
using BackwardCompatibility.ODEFramework;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// A node who is constrained to move only on a circle and who enforces that the
    /// associated dominated node stays at 180 degrees with it.
    /// </summary>
    internal class RotationDominantNode : Node
    {
        private const int RotationStateLength = 2;

        private DominatedNode other;
        private Vector2D center;
        private double radius;

        // This is the state ...
        private double angle;
        private double angularVelocity;

        /// <summary>
        /// Constructs a node constrained to rotate about a center of rotation
        /// determined as the midpoint between the initial position and the initial
        /// position of the dominated node.
        /// </summary>
        /// <param name="initialPosition"> </param>
        /// <param name="initialVelocity"> </param>
        /// <param name="other">
        /// The dominated node that will stay at 180 degrees... </param>
        public RotationDominantNode(double mass, Vector2D initialPosition, Vector2D initialVelocity, DominatedNode otherNode)
            : base(mass, initialPosition, initialVelocity)
        {
            other = otherNode;
            center = initialPosition.Add(otherNode.Position).Scale(0.5);
            Vector2D fromCenter = initialPosition.Subtract(center);
            radius = fromCenter.Norm;
            angle = Math.Atan2(fromCenter.PositionY, fromCenter.PositionX);
            angularVelocity = 0;
        }

        public override ODEState CurrentODEState
        {
            get
            {
                double[] state = new double[RotationStateLength];
                state[0] = angle;
                state[1] = angularVelocity;
                return new ODEState(state);
            }
        }

        public override ODEState ODEStateDerivative
        {
            get
            {
                double[] deriv = new double[RotationStateLength];
                deriv[0] = angularVelocity;

                // This node can only rotate so it is heavily constrained, in
                // particular,
                // we consider only the velocity in the tangential direction.
                Vector2D tangent = Position.Subtract(center).Rotate90().Normalize();
                Vector2D force = NetForce;

                // We subtract the dominated node's forces...
                force = force.Subtract(other.NetForce);

                // We take only the tangential component
                double angularForce = Math.Atan2(force.Dot(tangent), radius);
                double angularAccel = angularForce / Mass;
                deriv[1] = angularAccel;
                return new ODEState(deriv);
            }
        }

        public override int StateLength
        {
            get
            {
                return RotationStateLength;
            }
        }

        public override void SetODEState(double time, ODEState state)
        {
            double[] s = state.State;
            Debug.Assert(s.Length == RotationStateLength);
            angle = s[0];
            angularVelocity = s[1];
            Vector2D angleVector = new Vector2D(Math.Cos(angle), Math.Sin(angle));
            Vector2D posDiff = angleVector.Scale(radius);
            Position = center.Add(posDiff);
            other.SetPosition(center.Subtract(posDiff));
            Velocity = posDiff.Rotate90().Scale(angularVelocity);
            other.SetVelocity(Velocity.Scale(-1));
        }
    }
}