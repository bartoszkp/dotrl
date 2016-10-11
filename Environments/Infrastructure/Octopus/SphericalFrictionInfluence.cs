using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Implements viscous fluid friction for each node
    /// </summary>
    internal class SphericalFrictionInfluence : IInfluence
    {
        private ConstantSet constants;

        public SphericalFrictionInfluence(ConstantSet constants)
        {
            this.constants = constants;
        }

        public virtual Vector2D GetForce(Node target)
        {
            /* We assume the friction constant is positive (otherwise positive
             * feedback occurs.) */
            double speed = target.Velocity.Norm;
            return target.Velocity.ScaleTo(-speed * speed * constants.FrictionPerpendicular);
        }
    }
}