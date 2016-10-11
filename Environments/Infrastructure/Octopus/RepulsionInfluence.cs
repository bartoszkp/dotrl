using System;
using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Represents a repulsive field from a single node.
    /// </summary>
    internal class RepulsionInfluence : IInfluence
    {
        private ConstantSet constants;
        private Node source;

        public RepulsionInfluence(ConstantSet constants, Node source)
        {
            this.constants = constants;
            this.source = source;
        }

        public virtual Vector2D GetForce(Node target)
        {
            Vector2D displacement = target.Position.Subtract(source.Position);
            double distance = displacement.Norm;
            if (distance < this.constants.RepulsionThreshold)
            {
                double forceMag = constants.RepulsionConstant / Math.Pow(distance, this.constants.RepulsionPower);
                return displacement.ScaleTo(forceMag);
            }
            else
            {
                return Vector2D.ZERO;
            }
        }
    }
}