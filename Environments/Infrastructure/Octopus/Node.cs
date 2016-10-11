using System.Collections.Generic;
using BackwardCompatibility;
using BackwardCompatibility.ODEFramework;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Models a joint between muscles. (Theoretically octopus arms do not have
    /// joints but joints are the result of the discretization of an octopus)
    /// </summary>
    public class Node : PointMass2D
    {
        private IList<IInfluence> influences;

        public Node(NodeSpec spec)
            : this(spec.Mass, Vector2D.FromDuple(spec.Position), Vector2D.FromDuple(spec.Velocity))
        {
        }

        public Node(double mass, Vector2D initialPosition, Vector2D initialVelocity)
            : base(mass, initialPosition, initialVelocity)
        {
            influences = new List<IInfluence>();
        }

        internal virtual void AddInfluence(IInfluence inf)
        {
            influences.Add(inf);
        }

        public override Vector2D NetForce
        {
            get
            {
                Vector2D netForce = Vector2D.ZERO;
                foreach (IInfluence i in influences)
                {
                    netForce = netForce.Add(i.GetForce(this));
                }

                return netForce;
            }
        }
    }
}