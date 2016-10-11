using System.Collections.Generic;
using System.Linq;
using BackwardCompatibility.ODEFramework;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal sealed class EnvironmentSimulator : ODEEquationPartAggregate, IODEEquation
    {
        private Arm arm;
        private Node[] nodes;

        public EnvironmentSimulator(ConstantSet constants, Arm arm, HashSet<Food> food)
        {
            this.arm = arm;

            nodes = arm.Nodes.Concat(food.Cast<Node>()).ToArray();

            IInfluence gravity = new GravityInfluence(constants);
            IInfluence buoyancy = new BuoyancyInfluence(constants);
            IInfluence friction = new SphericalFrictionInfluence(constants);

            // The spherical friction is added to food particles only
            foreach (Food f in food)
            {
                f.AddInfluence(friction);
                AddPart(f);
            }

            foreach (Node node in arm.Nodes)
            {
                node.AddInfluence(gravity);
                node.AddInfluence(buoyancy);
                IInfluence repulsion = new RepulsionInfluence(constants, node);
                foreach (Node j in nodes)
                {
                    if (node != j)
                    {
                        j.AddInfluence(repulsion);
                    }
                }

                // We add all of the ODE parts to this aggregate
                AddPart(node);
            }
        }

        public ODEState GetDerivative(double time, ODEState state)
        {
            // We first set the state of all of our constituent parts
            SetODEState(time, state);
            return ODEStateDerivative;
        }

        public override void SetODEState(double time, ODEState state)
        {
            base.SetODEState(time, state);
            arm.UpdateInfluences();
        }
    }
}