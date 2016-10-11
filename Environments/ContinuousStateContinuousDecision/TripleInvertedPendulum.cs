using System;
using System.Collections.Generic;
using System.Linq;
using Core.Parameters;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Environments.ContinuousStateContinuousDecision
{
    public class TripleInvertedPendulum : Environment<double, double>
    {
        [Parameter(-1000, 0)]
        private double minX = -1;
        [Parameter(0, 1000)]
        private double maxX = 1;
        [Parameter(-1000, 0)]
        private double minF = -30;
        [Parameter(0, 1000)]
        private double maxF = 30;
        [Parameter(0, 10000)]
        private double forcePenalty = 0.2;
        [Parameter(0, 10000)]
        private double boundryPenalty = 100;
        [Parameter(0.001, 0.5)]
        private double internalDiscretization = 0.001;
        [Parameter(0.001, 0.5)]
        private double externalDiscretization = 0.005;
        
        public TripleInvertedPendulum()
        {
            SetConstants();

            CurrentState = new Core.MutableState<double>(8);
        }

        public override Core.EnvironmentDescription<double, double> GetEnvironmentDescription()
        {
            double[] standardDeviation = new double[8];
            double[] averageState = Enumerable.Repeat<double>(0.0, 8).ToArray();

            standardDeviation[0] = 2;
            standardDeviation[1] = 0.1;
            standardDeviation[2] = 0.1;
            standardDeviation[3] = 0.1;
            standardDeviation[4] = 1;
            standardDeviation[5] = 1;
            standardDeviation[6] = 1;
            standardDeviation[7] = 1;

            Core.SpaceDescription<double> stateDescription
                = new Core.SpaceDescription<double>(null, null, averageState, standardDeviation);
            Core.SpaceDescription<double> actionDescription
                = Core.SpaceDescription<double>.CreateOneDimensionalSpaceDescription(-10, 10);
            Core.DimensionDescription<double> reinforcementDescription
                = new Core.DimensionDescription<double>(-4, 4);

            return new Core.EnvironmentDescription<double, double>(stateDescription, actionDescription, reinforcementDescription, 0.9);
        }

        public override void StartEpisode()
        {
            CurrentState.StateVector = new double[8];
            CurrentState[3] = 0.001;
        }
        
        public override Core.Reinforcement PerformAction(Core.Action<double> action)
        {
            double force = Math.Max(minF, Math.Min(action[0], maxF));
            double reward = -forcePenalty * Math.Abs(action[0] - force);

            Vector<double> ddq = null;

            for (double t = 0; t < externalDiscretization; t += internalDiscretization)
            {
                CalculateBChf(force);
                for (int i = 4; i < 8; ++i)
                {
                    dq.At(i - 4, CurrentState[i]);
                }

                LU lu = new DenseLU(b);

                ddq = lu.Solve(f - h - c * dq);

                double dt = Math.Min(internalDiscretization, externalDiscretization - t);
                double dt2 = dt * dt;
                for (int i = 0; i < 4; i++)
                {
                    CurrentState[i] += dt * CurrentState[4 + i] + 0.5 * dt2 * ddq[i];
                    CurrentState[4 + i] += dt * ddq[i];
                }
            }

            for (int i = 1; i < 4; i++)
            {
                if (CurrentState[i] > Math.PI)
                {
                    CurrentState[i] -= Math.PI * 2;
                }

                if (CurrentState[i] < -Math.PI)
                {
                    CurrentState[i] += Math.PI * 2;
                }
            }

            CurrentState.IsTerminal = !IsStateOK();

            if (IsStateOK())
            {
                reward -= Math.Abs(CurrentState[0]);
                reward -= Math.Abs(CurrentState[1]);
                reward -= Math.Abs(CurrentState[2]);
                reward -= Math.Abs(CurrentState[3]);
            }
            else
            {
                reward -= boundryPenalty;
            }

            return reward;
        }

        public bool IsStateOK()
        {
            if (CurrentState[0] < minX || CurrentState[0] > maxX)
            {
                return false;
            }

            for (int i = 1; i < 4; i++)
            {
                if (Math.Abs(CurrentState[i] / Math.PI * 180) > 20)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<double> GetPositions()
        {
            double[] result = new double[8];
            
            result[0] = CurrentState[0];
            result[1] = 0;
            result[2] = result[0] + ll1 * Math.Sin(CurrentState[1]);
            result[3] = result[1] + ll1 * Math.Cos(CurrentState[1]);
            result[4] = result[2] + ll2 * Math.Sin(CurrentState[2]);
            result[5] = result[3] + ll2 * Math.Cos(CurrentState[2]);
            result[6] = result[4] + ll3 * Math.Sin(CurrentState[3]);
            result[7] = result[5] + ll3 * Math.Cos(CurrentState[3]);
            
            return result;
        }

        private void SetConstants()
        {
            g = 9.8;
            M = 1.014;
            m1 = 0.4506;
            m2 = 0.219;
            m3 = 0.0568;
            l1 = 0.37;
            l2 = 0.3;
            l3 = 0.05;
            ll1 = 0.43;
            ll2 = 0.33;
            ll3 = 0.13;
            i1 = 0.0042;
            i2 = 0.0012;
            i3 = 0.00010609;
            cc = 5.5;
            c1 = 0.00026875;
            c2 = c1;
            c3 = c1;
            ks = 24.7125;
            mux = 0.07;
            mu1 = 0.003;
            mu2 = 0.003;
            mu3 = 0.003;

            a1 = M + m1 + m2 + m3;
            a2 = m1 * l1 + (m2 + m3) * ll1;
            a3 = m2 * l2 + m3 * ll2;
            a4 = m3 * l3;
            a5 = cc;
            a6 = -m1 * l1 - (m2 + m3) * ll1;
            a7 = -(m2 * l2 + m3 * ll2);
            a8 = -m3 * l3;
            a9 = m1 * l1 + (m2 + m3) * ll1;
            a10 = i1 + m2 * l1 * l1 + (m2 + m3) * ll1 * ll1;
            a11 = (m2 * l2 + m3 * ll2) * ll1;
            a12 = m3 * l3 * ll1;
            a13 = c1 + c2;
            a14 = (m2 * l2 + m3 * ll2) * ll1;
            a15 = -c2;
            a16 = m3 * l3 * ll1;
            a17 = -g * (m1 * l1 + m2 * ll1 + m3 * ll1);
            a18 = m2 * l2 + m3 * ll2;
            a19 = (m2 * l2 + m3 * ll2) * ll1;
            a20 = i2 + m3 * ll2 * ll2 + m2 * l2 * l2;

            a21 = m3 * l3 * ll2;
            a22 = -(m2 * l2 + m3 * ll2) * ll1;
            a23 = -c2;
            a24 = c2 + c3;
            a25 = m3 * l3 * ll2;
            a26 = -c3;
            a27 = -g * (m2 * l2 + m3 * ll2);
            a28 = m3 * l3;
            a29 = m3 * l3 * ll1;
            a30 = m3 * l3 * ll2;
            a31 = i3 + m3 * l3 * l3;
            a32 = c3;
            a33 = -m3 * l3 * ll1;
            a34 = -g * m3 * l3;
            a35 = -m3 * l3 * ll2;
            a36 = -c3;
            a37 = 1.3;
            a38 = 0.506;
            a39 = 0.219;
            a40 = 0.568;
        }

        private void CalculateBChf(double u)
        {
            double x = CurrentState[0];
            double th1 = CurrentState[1];
            double th2 = CurrentState[2];
            double th3 = CurrentState[3];
            double dth1 = CurrentState[5];
            double dth2 = CurrentState[6];
            double dth3 = CurrentState[7];
            double cos1 = Math.Cos(th1);
            double cos2 = Math.Cos(th2);
            double cos3 = Math.Cos(th3);
            double cos12 = Math.Cos(th1 - th2);
            double cos13 = Math.Cos(th1 - th3);
            double cos23 = Math.Cos(th2 - th3);
            double sin1 = Math.Sin(th1);
            double sin2 = Math.Sin(th2);
            double sin3 = Math.Sin(th3);
            double sin12 = Math.Sin(th1 - th2);
            double sin13 = Math.Sin(th1 - th3);
            double sin23 = Math.Sin(th2 - th3);

            double[,] b
                = new double[4, 4] 
                {
                { a1, a2 * cos1, a3 * cos2, a4 * cos3 },
                { a9 * cos1, a10, a11 * cos12, a12 * cos13 },
                { a18 * cos2, a19 * cos12, a20, a21 * cos23 },
                { a28 * cos3, a29 * cos13, a30 * cos23, a31 }
                };

            double[,] c
                = new double[4, 4] 
                {
                { a5, a6 * sin1 * dth1, a7 * sin2 * dth2, a8 * sin3 * dth3 },
                { 0, a13, a14 * sin12 * dth2 + a15, a16 * sin13 * dth3 },
                { 0, a22 * sin12 * dth1 * a23, a24, a25 * sin23 * dth3 + a26 },
                { 0, a33 * sin13 * dth1, a35 * sin23 * dth2 + a36, a32 }
                };
            
            double[] h
                = new double[4] { 0, a17 * sin1, a27 * sin2, a34 * sin3 };
            
            double[] f
                = new double[4]
                {
                    ks * u - Math.Sign(x) * mux * a37,
                    -Math.Sign(th1) * mu1 * a38,
                    -Math.Sign(th2) * mu2 * a39,
                    -Math.Sign(th3) * mu3 * a40
                };

            this.b = new DenseMatrix(b);
            this.c = new DenseMatrix(c);
            this.h = new DenseVector(h);
            this.f = new DenseVector(f);
        }

        private double g, M, m1, m2, m3, l1, l2, l3, ll1, ll2, ll3, i1, i2, i3;
        private double cc, c1, c2, c3, ks, mux, mu1, mu2, mu3;
        private double a1, a2, a3, a4, a5, a6, a7, a8, a9;
        private double a10, a11, a12, a13, a14, a15, a16, a17, a18, a19;
        private double a20, a21, a22, a23, a24, a25, a26, a27, a28, a29;
        private double a30, a31, a32, a33, a34, a35, a36, a37, a38, a39, a40;
        private Vector<double> dq = new DenseVector(4);
        private DenseMatrix b, c;
        private DenseVector h, f;
    }
}
