using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Models forces acting on nodes. Each node has a certain number of 
    /// influences dictating it's dynamic behavior.
    /// </summary>
    internal interface IInfluence
    {
        /// <summary>
        /// Returns the force due to an influence on a particular node. </summary>
        /// <param name="target"> The node on which we want to get the force due to this influence. </param>
        /// <returns> the force due to this influence on the target node. </returns>
        Vector2D GetForce(Node target);
    }
}