using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Implements gravitational force for each node
    /// The gravitational force is applied to nodes for the moment, but later
    /// on it will also be applied to piece of food.
    /// </summary>
    internal class GravityInfluence : IInfluence
    {
        private ConstantSet constants;

        public GravityInfluence(ConstantSet constants)
        {
            this.constants = constants;
        }

        public virtual Vector2D GetForce(Node target)
        {
            return new Vector2D(0, -this.constants.Gravity * target.Mass);
        }
    }
}