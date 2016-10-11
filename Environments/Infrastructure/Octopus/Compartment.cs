using System;
using System.Collections.Generic;
using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    public class Compartment
    {
        private const double AngleConstant = 0.1 / 90.0;
        private const double NeighborAngleScale = 0.2;

        private ConstantSet constants;
        private Node d, dt, tv, v;
        private Node[] nodes;
        private PressureInfluence pressure;

        private MuscleInfluence dorsal, transversal, ventral;
        private AxialFrictionInfluence dorsalFriction, ventralFriction;
        private double desiredArea;

        private double area;

        private IList<object> pressureShapes;

        /// <summary>
        /// Constructs a new compartment from four nodes </summary>
        /// <param name="dorsalNode"> Dorsal node </param>
        /// <param name="dorsalTransversalNode"> Dorsal transversal Node </param>
        /// <param name="ventralTransversalNode"> Ventral transversal node </param>
        /// <param name="ventralNode"> Ventral node </param>
        internal Compartment(ConstantSet constants, Node dorsalNode, Node dorsalTransversalNode, Node ventralTransversalNode, Node ventralNode)
        {
            this.constants = constants;
            this.d = dorsalNode;
            this.dt = dorsalTransversalNode;
            this.tv = ventralTransversalNode;
            this.v = ventralNode;

            nodes = new Node[] { dorsalNode, dorsalTransversalNode, ventralTransversalNode, ventralNode };

            // Internal pressures
            pressure = new PressureInfluence(this.constants, this);
            foreach (Node n in nodes)
            {
                n.AddInfluence(pressure);
            }

            // Muscles
            dorsal = new LongitudinalMuscleInfluence(this.constants, dorsalNode, dorsalTransversalNode, dorsalTransversalNode.Position.Subtract(ventralTransversalNode.Position).Norm);
            dorsalNode.AddInfluence(dorsal);
            dorsalTransversalNode.AddInfluence(dorsal);
            transversal = new TransversalMuscleInfluence(this.constants, dorsalTransversalNode, ventralTransversalNode, dorsalTransversalNode.Position.Subtract(dorsalNode.Position).Norm);
            dorsalTransversalNode.AddInfluence(transversal);
            ventralTransversalNode.AddInfluence(transversal);
            ventral = new LongitudinalMuscleInfluence(this.constants, ventralTransversalNode, ventralNode, dorsalTransversalNode.Position.Subtract(ventralTransversalNode.Position).Norm);
            ventralTransversalNode.AddInfluence(ventral);
            ventralNode.AddInfluence(ventral);

            // Friction
            dorsalFriction = new AxialFrictionInfluence(this.constants, dorsalTransversalNode, dorsalNode);
            dorsalTransversalNode.AddInfluence(dorsalFriction);
            ventralFriction = new AxialFrictionInfluence(this.constants, ventralTransversalNode, ventralNode);
            ventralTransversalNode.AddInfluence(ventralFriction);

            pressureShapes = new List<object>();

            ComputeArea();
            desiredArea = area;
        }

        public virtual void SetAction(double dorsalAction, double transversalAction, double ventralAction)
        {
            dorsal.Action = dorsalAction;
            transversal.action = transversalAction;
            ventral.action = ventralAction;
        }

        public virtual void UpdateInfluences()
        {
            ComputeArea();

            pressureShapes.Clear();
            pressure.Update();

            dorsal.Update();
            transversal.Update();
            ventral.Update();
        }

        public double Area
        {
            get
            {
                ComputeArea();
                return area;
            }
        }

        // Dorsal means upper
        public Node DorsalNode
        {
            get
            {
                return d;
            }
        }

        // Versal means lower
        public Node VentralNode
        {
            get
            {
                return v;
            }
        }

        public double CenterX
        {
            get
            {
                return (dt.Position.PositionX + d.Position.PositionX + v.Position.PositionX + tv.Position.PositionX) / 4;
            }
        }

        public double CenterY
        {
            get
            {
                return (dt.Position.PositionY + d.Position.PositionY + v.Position.PositionY + tv.Position.PositionY) / 4;
            }
        }

        public Vector2D Center
        {
            get
            {
                return new Vector2D(CenterX, CenterY);
            }
        }

        public double CenterVelocityX
        {
            get
            {
                return (dt.Velocity.PositionX + d.Velocity.PositionX + v.Velocity.PositionX + tv.Velocity.PositionX) / 4;
            }
        }

        public double CenterVelocityY
        {
            get
            {
                return (dt.Velocity.PositionY + d.Velocity.PositionY + v.Velocity.PositionY + tv.Velocity.PositionY) / 4;
            }
        }

        public Vector2D CenterVelocity
        {
            get
            {
                return new Vector2D(CenterVelocityX, CenterVelocityY);
            }
        }

        public bool Contains(Vector2D point)
        {
            return point.Subtract(d.Position).IsRightFrom(dt.Position.Subtract(d.Position))
                && point.Subtract(dt.Position).IsRightFrom(tv.Position.Subtract(dt.Position))
                && point.Subtract(tv.Position).IsRightFrom(v.Position.Subtract(tv.Position))
                && point.Subtract(v.Position).IsRightFrom(d.Position.Subtract(v.Position)); 
        }

        private void ComputeArea()
        {
            // The area of a quadrilateral is 0.5*p*q*sin(theta)
            // Where p and q are the length of the 2 diagonals and theta is the angle between them
            // If we rotate P in the right direction, then area = 0.5*p*q*cos(theta) where theta is 
            // the angle between Q and the rotated P, but this is simply 0.5 * dot product of P' and Q
            // Note, it is possible because of the cos to get a negative area, when this happens, it means
            // that the quadrilateral has flipped and that the normally inward pointing normals are
            // now pointing outward, so if we get a negative area, we should flip the force vectors...
            Vector2D p = tv.Position.Subtract(d.Position);
            Vector2D q = dt.Position.Subtract(v.Position);
            area = 0.5 * p.Rotate90().Dot(q);
            //if (Math.Sign(area) != Math.Sign(desiredArea) && desiredArea != 0)
            //{
            //    Console.WriteLine("Compartment:" + this.ToString() + " has flipped inside out. Area:" + area);
            //}
        }

        private class PressureInfluence : IInfluence
        {
            private ConstantSet constants;
            private IDictionary<Node, Vector2D> forces;
            private Compartment parent;

            public PressureInfluence(ConstantSet constants, Compartment parent)
            {
                this.constants = constants;
                this.parent = parent;
                forces = new Dictionary<Node, Vector2D>();
            }

            public virtual void Update()
            {
                // See Compute Area for reason of the absolute value and signum of area...
                double pressureForce = constants.Pressure * (Math.Abs(parent.area) - Math.Abs(parent.desiredArea));
                pressureForce = Math.Sign(parent.area) * Math.Sign(pressureForce) * Math.Sqrt(Math.Abs(pressureForce));

                forces.Clear();

                // The pressure is applied on every segment proportionally to its area.
                for (int i = 0, n = parent.nodes.Length; i < n; i++)
                {
                    Node prevNode = parent.nodes[i];
                    Node curNode = parent.nodes[(i + 1) % n];
                    Node nextNode = parent.nodes[(i + 2) % n];

                    Vector2D prevVector = curNode.Position.Subtract(prevNode.Position).Rotate270();
                    Vector2D nextVector = nextNode.Position.Subtract(curNode.Position).Rotate270();
                    forces.Add(curNode, prevVector.Scale(pressureForce).Add(nextVector.Scale(pressureForce)));
                }
            }

            public virtual Vector2D GetForce(Node target)
            {
                return forces.ContainsKey(target) ? forces[target] : Vector2D.ZERO;
            }
        }
    }
}