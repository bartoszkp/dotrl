using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Implements buoyancy force for each node (which corresponds to the upward
    /// force on a body immersed or partly immersed in a fluid)
    /// 
    /// </summary>
    internal class BuoyancyInfluence : IInfluence
    {
        private ConstantSet constants;

        public BuoyancyInfluence(ConstantSet constants)
        {
            this.constants = constants;
        }

        public virtual Vector2D GetForce(Node target)
        {
            Vector2D position = target.Position;
            double surfaceDistance = constants.SurfaceLevel - position.PositionY;
            if (surfaceDistance > 0)
            {
                return new Vector2D(0, constants.Gravity * target.Mass);
            }
            else
            {
                return Vector2D.ZERO;
            }
        }
    }
}