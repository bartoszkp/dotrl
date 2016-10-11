using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// A friction class representing a cylinder or other object which has
    /// a different friction coefficient in the tangential and in the
    /// perpendicular direction.
    /// 
    /// </summary>
    internal class AxialFrictionInfluence : IInfluence
    {
        private ConstantSet constants;
        private Node target;
        private Node axialNode;

        /// <summary>
        /// The friction influence will only influence the target, the axial
        /// node serves only to give the orientation of the segment that
        /// gives the tangential direction for friction computation. </summary>
        /// <param name="target"> The node on which the friction will be computed. </param>
        /// <param name="axialNode"> A node that serves only to determine the orientation of the axis. </param>
        public AxialFrictionInfluence(ConstantSet constants, Node target, Node axialNode)
        {
            this.constants = constants;
            this.target = target;
            this.axialNode = axialNode;
        }

        public virtual Vector2D GetForce(Node target)
        {
            if (target == this.target)
            {
                Vector2D axis = target.Position.Subtract(axialNode.Position).Normalize();

                // We project the speed in the perpendicular and tangential 
                // direction and apply the different coefficients to each
                Vector2D velocity = target.Velocity;
                double tanSpeed = velocity.Dot(axis);
                Vector2D tangential = axis.ScaleTo(tanSpeed);
                Vector2D perpendicular = velocity.Subtract(tangential);
                double perSpeed = perpendicular.Norm;
                return tangential.ScaleTo(-tanSpeed * tanSpeed * constants.FrictionTangential).Add(perpendicular.ScaleTo(-perSpeed * perSpeed * constants.FrictionPerpendicular));
            }
            else
            {
                return Vector2D.ZERO;
            }
        }
    }
}