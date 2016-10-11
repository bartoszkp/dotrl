using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Parameters;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Environments.ContinuousStateContinuousDecision
{
    public class RobotWeightlifting : Environment<double, double>
    {
        [Parameter(0, Math.PI)]
        private double maxArc = Math.PI * 5 / 6;
        [Parameter(0, 10000)]
        private double crushPenalty = 100;
        [Parameter(0, 100)]
        private double tauPenalty = 0.025;
        [Parameter(0, 100)]
        private double goalReward = 0;
        [Parameter]
        private bool goalWorks = false;
        [Parameter(0, 3)]
        private double minLoad = 0;
        [Parameter(3, 6)]
        private double maxLoad = 3;
        [Parameter(0, 100)]
        private double maxTau = 1;
        [Parameter(0.001, 0.2)]
        private double externalDiscretization = 0.05;
        [Parameter(0.001, 0.2)]
        private double internalDiscretization = 0.001;

        public RobotWeightlifting()
        {
            sampler = new System.Random();
            arc = new double[3];
            dArc = new double[3];
            d2Arc = new DenseVector(3);

            b = new DenseMatrix(3, 3);
            c = new DenseVector(3);
            h = new DenseVector(3);
            tau = new DenseVector(3);

            Arc1 = -Math.PI / 2;
            Load = 0;

            sampler = new System.Random();
            joint = Enumerable.Range(0, 4).Select(i => new DenseVector(2)).ToArray();

            pole = new DenseVector[] { new DenseVector(new[] { -1.5, -3 }), new DenseVector(new[] { -1.5, 0 }) };
            cantilever = new DenseVector[] { new DenseVector(new[] { -1.5, -1 }), new DenseVector(new[] { -0.5, 0 }) };
            crossbeam = new DenseVector[] { new DenseVector(new[] { -1.5, 0 }), new DenseVector(new[] { 0.0, 0.0 }) };

            CurrentState = new MutableState<double>(6);

            g = 9.81;
        }

        public override EnvironmentDescription<double, double> GetEnvironmentDescription()
        {
            double[] standardDeviation = new double[6];
            double[] averageState = Enumerable.Repeat<double>(0.0, 6).ToArray();

            standardDeviation[0] = 3;
            standardDeviation[1] = 2;
            standardDeviation[2] = 3;
            standardDeviation[3] = 2;
            standardDeviation[4] = 3;
            standardDeviation[5] = 2;

            SpaceDescription<double> stateDescription = new SpaceDescription<double>(null, null, averageState, standardDeviation);

            SpaceDescription<double> actionDescription
                = new SpaceDescription<double>(
                    Enumerable.Repeat<double>(-50, 3).ToArray(),
                    Enumerable.Repeat<double>(50, 3).ToArray());
            
            DimensionDescription<double> reinforcementDescription = new DimensionDescription<double>(-4, 4);

            return new EnvironmentDescription<double, double>(stateDescription, actionDescription, reinforcementDescription, 0.9);
        }

        public override Reinforcement PerformAction(Core.Action<double> action)
        {
            double tau_violation = 0;

            CalculateTau(action[0], action[1], action[2], ref tau, ref tau_violation);

            for (double t = 0; t < externalDiscretization; t += internalDiscretization)
            {
                double dt = Math.Min(internalDiscretization, externalDiscretization - t);
                double dt2 = dt * dt;

                Calculate2ndDerivatives(tau);

                for (int i = 0; i < 3; i++)
                {
                    arc[i] += dArc[i] * dt + 0.5 * d2Arc[i] * dt2;
                    dArc[i] += d2Arc[i] * dt;
                }
            }

            if (arc.Any(v => double.IsNaN(v)) || dArc.Any(v => double.IsNaN(v)))
            {
                for (int i = 0; i < 3; i++)
                {
                    arc[i] = 100;
                    dArc[i] = 0;
                }
            }

            double reward = -tau_violation * tauPenalty;

            CopyState();

            if (IsStateOK())
            {
                if (goalWorks && IsAtGoalPosition())
                {
                    reward += goalReward;
                    CurrentState.IsTerminal = true;
                }
                else
                {
                    reward += GetStandardReward();
                    CurrentState.IsTerminal = false;
                }
            }
            else
            {
                reward -= crushPenalty;
                CurrentState.IsTerminal = true;
            }

            return reward;
        }

        public override void StartEpisode()
        {
            Arc1 = sampler.NextDouble() * Math.PI / 2;
            Arc1 = -Math.PI / 2;
            Arc2 = Arc3 = 0;
            Arc1 = 0;
            Arc2 = -Math.PI / 2;
            Arc3 = -Math.PI / 2;
            dArc[0] = dArc[1] = dArc[2] = 0;

            if (sampler.NextDouble() < 0.5)
            {
                Load = minLoad;
            }
            else
            {
                Load = maxLoad;
            }

            CopyState();
        }

        public bool IsStateOK()
        {
            if (Arc1 < -maxArc || Arc1 > maxArc
                || Arc2 < -maxArc || Arc2 > maxArc
                || Arc3 < -maxArc || Arc3 > maxArc)
            {
                return (false);
            }

            GetJointsPositions(joint[1], joint[2], joint[3]);

            DenseVector[] firstsegment = new DenseVector[] { joint[0], joint[1] };
            DenseVector[] lastsegment = new DenseVector[] { joint[2], joint[3] };
            if (CrossWith(lastsegment[0], lastsegment[1], pole[0], pole[1])
                || CrossWith(lastsegment[0], lastsegment[1], cantilever[0], cantilever[1])
                || CrossWith(lastsegment[0], lastsegment[1], crossbeam[0], crossbeam[1])
                || CrossWith(lastsegment[0], lastsegment[1], firstsegment[0], firstsegment[1]))
            {
                return (false);
            }

            return (true);
        }

        public IEnumerable<double> GetJoints()
        {
            return joint.Skip(1).SelectMany(j => new[] { j[0], j[1] });
        }

        private void CopyState()
        {
            CurrentState[0] = Arc1;
            CurrentState[1] = dArc[0];
            CurrentState[2] = Arc1 + Arc2;
            CurrentState[3] = dArc[0] + dArc[1];
            CurrentState[4] = Arc1 + Arc2 + Arc3;
            CurrentState[5] = dArc[0] + dArc[1] + dArc[2];
        }

        public double Arc1
        {
            get { return arc[0]; }
            set { arc[0] = value; }
        }

        public double Arc2
        {
            get { return arc[1]; }
            set { arc[1] = value; }
        }

        public double Arc3
        {
            get { return arc[2]; }
            set { arc[2] = value; }
        }

        public double Load
        {
            get { return m - 0.5; }
            set { m = value + 0.5; }
        }

        public void GetJointsPositions(DenseVector j1, DenseVector j2, DenseVector j3)
        {
            j1[0] = Math.Cos(Arc1);
            j1[1] = Math.Sin(Arc1);
            j2[0] = j1[0] + Math.Cos(Arc1 + Arc2);
            j2[1] = j1[1] + Math.Sin(Arc1 + Arc2);
            j3[0] = j2[0] + Math.Cos(Arc1 + Arc2 + Arc3);
            j3[1] = j2[1] + Math.Sin(Arc1 + Arc2 + Arc3);
        }

        public bool IsAtGoalPosition()
        {
            double arc_limit = 5 * Math.PI / 180;
            double darc_limit = 0.5;
            if (Math.Abs(arc[0] - Math.PI / 2) < arc_limit && Math.Abs(dArc[0]) < darc_limit
                && Math.Abs(arc[1]) < arc_limit && Math.Abs(dArc[1]) < darc_limit
                && Math.Abs(arc[2]) < arc_limit && Math.Abs(dArc[2]) < darc_limit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsStateNumeric()
        {
            for (int i = 0; i < 3; i++)
            {
                if (double.IsNaN(arc[i]) || double.IsNaN(dArc[i]))
                {
                    return (false);
                }
            }

            return (true);
        }

        private void CalculateTau(double u1, double u2, double u3, ref DenseVector tau, ref double violation)
        {
            tau = new DenseVector(3);
            tau[0] = Math.Min(Math.Max(-maxTau, u1), maxTau);
            tau[1] = Math.Min(Math.Max(-maxTau, u2), maxTau);
            tau[2] = Math.Min(Math.Max(-maxTau, u3), maxTau);
            violation
                = Math.Abs(tau[0] - u1)
                + Math.Abs(tau[1] - u2)
                + Math.Abs(tau[2] - u3);
        }

        private void Calculate2ndDerivatives(Vector tau)
        {
            double cos1 = Math.Cos(Arc1);
            double cos12 = Math.Cos(Arc1 + Arc2);
            double cos123 = Math.Cos(Arc1 + Arc2 + Arc3);
            double cos2 = Math.Cos(Arc2);
            double cos23 = Math.Cos(Arc2 + Arc3);
            double cos3 = Math.Cos(Arc3);

            double sin2 = Math.Sin(Arc2);
            double sin23 = Math.Sin(Arc2 + Arc3);
            double sin3 = Math.Sin(Arc3);

            b[0, 0] = 3 + 2 * cos2 + m * (3 + 2 * cos2 + 2 * cos23 + 2 * cos3);
            b[0, 1] = 1 + cos2 + m * (2 + cos2 + cos23 + 2 * cos3);
            b[0, 2] = m * (1 + cos23 + cos3);
            b[1, 0] = 1 + cos2 + m * (2 + cos2 + cos23 + 2 * cos3);
            b[1, 1] = 1 + m * (2 + 2 * cos3);
            b[1, 2] = m * (1 + cos3);
            b[2, 0] = m * (1 + cos23 + cos3);
            b[2, 1] = m * (1 + cos3);
            b[2, 2] = m;

            h[0] = g * ((2 + m) * cos1 + (1 + m) * cos12 + m * cos123);
            h[1] = g * ((1 + m) * cos12 + m * cos123);
            h[2] = g * m * cos123;

            c[0] = 0
                + dArc[0] * dArc[1] * (-2 * sin2 - m * (2 * sin2 + 2 * sin23))
                + dArc[0] * dArc[2] * -m * (2 * sin23 + 2 * sin3)
                + dArc[1] * dArc[1] * (-sin2 - m * (sin2 + sin23))
                + dArc[1] * dArc[2] * -m * (2 * sin23 + 2 * sin3)
                + dArc[2] * dArc[2] * -m * (sin23 + sin3);

            c[1] = dArc[0] * dArc[0] * (sin2 + m * (sin2 + sin23))
                + 0
                + dArc[0] * dArc[2] * -m * 2 * sin3
                + 0
                + dArc[1] * dArc[2] * -m * 2 * sin3
                + dArc[2] * dArc[2] * -m * sin3;

            c[2] = dArc[0] * dArc[0] * m * (sin23 + sin3)
                + dArc[0] * dArc[1] * m * 2 * sin3
                + 0
                + dArc[1] * dArc[1] * m * sin3
                + 0
                + 0;

            // TODO: determine what exceptions Solve can throw, or how does it signal calculation failure
            ////try
            ////{
                LU lu = new DenseLU(b);
                d2Arc = lu.Solve(tau - c - h);
            ////}
            ////catch
            ////{
            ////    d2Arc.Clear();
            ////}
        }

        private double GetStandardReward()
        {
            double distance_xy
                = Math.Sqrt(joint[1][0].Squared() + (joint[1][1] - 1).Squared())
                + Math.Sqrt(joint[2][0].Squared() + (joint[2][1] - 2).Squared())
                + Math.Sqrt(joint[3][0].Squared() + (joint[3][1] - 3).Squared());

            double distance_arc
                = Math.Abs(Math.PI / 2 - Arc1)
                + Math.Abs(Math.PI / 2 - Arc1 - Arc2)
                + Math.Abs(Math.PI / 2 - Arc1 - Arc2 - Arc3);

            double st_reward = 10 - distance_xy * 5 / 12 - distance_arc * 5 / 3 / Math.PI;

            if (Arc2 > 0 || Arc3 > 0)
            {
                st_reward -= Math.Max(Arc2, Arc3) * 2;
            }

            return st_reward;
        }

        private static int SignSinDifference(Vector<double> v1, Vector<double> v2)
        {
            return Math.Sign(v1[1] * v2[0] - v1[0] * v2[1]);
        }

        private static bool CrossWith(Vector x1, Vector x2, Vector x3, Vector x4)
        {
            return SignSinDifference(x2 - x1, x3 - x1)
                * SignSinDifference(x2 - x1, x4 - x1)
                < 0
                &&
                SignSinDifference(x4 - x3, x1 - x3)
                * SignSinDifference(x4 - x3, x2 - x3)
                < 0;
        }

        private DenseVector[] joint;
        private DenseVector[] pole;
        private DenseVector[] cantilever;
        private DenseVector[] crossbeam;
        private System.Random sampler;
        private double g;
        private double m;
        private double[] arc;
        private double[] dArc;
        private DenseMatrix b;
        private DenseVector c;
        private DenseVector h;
        private DenseVector tau;
        private Vector<double> d2Arc;
    }
}
