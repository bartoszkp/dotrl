using System;
using System.Collections.Generic;
using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    /// <summary>
    /// Models an octopus muscle ( as a link between two vertices).
    /// </summary>
    internal abstract class MuscleInfluence : IInfluence
    {
        protected MuscleInfluence(ConstantSet constants, Node n1, Node n2, double width)
        {
            this.constants = constants;
            this.N1 = n1;
            this.N2 = n2;
            this.Width = width;
            this.action = 0;
            this.InitialLength = n1.Position.Subtract(n2.Position).Norm;
            this.Forces = new Dictionary<Node, Vector2D>();

            // The constants are not set here, override this class to set the constants to the proper value
            this.ActiveConstant = 0;
            this.PassiveConstant = 0;
            this.DampingConstant = 0;
        }

        public virtual void Update()
        {
            Vector2D displacement = N2.Position.Subtract(N1.Position);
            Vector2D center = N1.Position.AddScaled(displacement, 0.5);
            Vector2D velocity = N1.Velocity.Subtract(N2.Velocity);

            double projectedVelocity = velocity.Dot(displacement.Normalize());

            // Follows the linear muscle model
            double normalizedLength = displacement.Norm / InitialLength;
            double forceMag = 0;
            if (normalizedLength > constants.MuscleNormalizedMinLength)
            {
                forceMag = (ActiveConstant * action + PassiveConstant) * (normalizedLength - constants.MuscleNormalizedMinLength);
            }

            forceMag += projectedVelocity * DampingConstant;
            Forces.Clear();
            foreach (Node n in new Node[] { N1, N2 })
            {
                Forces.Add(n, center.Subtract(n.Position).ScaleTo(forceMag));
            }
        }

        public double Action
        {
            get
            {
                return action;
            }

            set
            {
                action = Math.Min(Math.Max(0.0, value), 1.0);
            }
        }

        public virtual Vector2D GetForce(Node target)
        {
            return Forces.ContainsKey(target) ? Forces[target] : Vector2D.ZERO;
        }

        protected internal Node N1 { get; set; }

        protected internal Node N2 { get; set; }

        protected internal double Width { get; set; }

        protected internal double InitialLength { get; set; }

        protected internal double action { get; set; }

        protected internal IDictionary<Node, Vector2D> Forces { get; private set; }

        protected internal double ActiveConstant { get; set; }

        protected internal double PassiveConstant { get; set; }

        protected internal double DampingConstant { get; set; }

        private ConstantSet constants;
    }
}