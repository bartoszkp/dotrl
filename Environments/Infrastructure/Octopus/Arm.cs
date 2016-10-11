using System.Collections.Generic;
using System.Linq;
using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    public class Arm
    {
        public Compartment[] Compartments { get; set; }

        public Node[] UpperNodes { get; set; }

        public Node[] LowerNodes { get; set; }

        public Node[] Nodes { get; set; }

        internal Arm(ConstantSet constants, ArmSpec spec)
        {
            IList<NodePairSpec> nodePairs = spec.NodePair;

            NodeSpec upperFirstSpec = nodePairs[0].Upper;
            NodeSpec lowerFirstSpec = nodePairs[0].Lower;

            DominatedNode lowerFirst = new DominatedNode(lowerFirstSpec.Mass, Vector2D.FromDuple(lowerFirstSpec.Position), Vector2D.FromDuple(lowerFirstSpec.Velocity));
            Node upperFirst = new RotationDominantNode(upperFirstSpec.Mass, Vector2D.FromDuple(upperFirstSpec.Position), Vector2D.FromDuple(upperFirstSpec.Velocity), lowerFirst);

            UpperNodes = Enumerable
                .Repeat(upperFirst, 1)
                .Concat(nodePairs.Skip(1).Select(np => new Node(np.Upper)))
                .ToArray();
            LowerNodes = Enumerable
                .Repeat(lowerFirst as Node, 1)
                .Concat(nodePairs.Skip(1).Select(np => new Node(np.Lower)))
                .ToArray();
            Nodes = UpperNodes.Concat(LowerNodes).ToArray();

            Compartments = UpperNodes
                .Select((node, index)
                    => (index == 0) ? null : new Compartment(constants, UpperNodes[index - 1], UpperNodes[index], LowerNodes[index], LowerNodes[index - 1]))
                .Skip(1)
                .ToArray();
        }

        public virtual void UpdateInfluences()
        {
            foreach (Compartment c in Compartments)
            {
                c.UpdateInfluences();
            }
        }
    }
}