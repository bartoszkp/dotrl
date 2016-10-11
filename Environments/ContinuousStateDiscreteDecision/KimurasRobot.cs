using System.Diagnostics.Contracts;
using Core;
using Core.Parameters;

namespace Environments.ContinuousStateDiscreteDecision
{
    using MathNet.Numerics.LinearAlgebra.Double;

    public class KimurasRobot : Environment<double, int>
    {
        [Parameter(0, 1)]
        private double discountFactor = 0.9;
        [Parameter(0, 1)]
        private double unaryActionPenalty = 0.1;
        [Parameter(0, 1)]
        private double noMovePenalty = 1;
        [Parameter(0.001, 0.1)]
        private double internalTimeDiscretization = 0.05;

        public double[] MArmX { get; private set; }

        public double[] MArmY { get; private set; }

        public double[] MBodyX { get; private set; }

        public double[] MBodyY { get; private set; }

        public KimurasRobot()
        {
            this.bodyX = new double[] { 0, 0, 32, 32 };
            this.bodyY = new double[] { 0, 18, 18, 0 };
            this.armX = new double[3];
            this.armX[0] = 32;
            this.armY = new double[3];
            this.armY[0] = 18;
            this.MBodyX = new double[4];
            this.MBodyY = new double[4];
            this.MArmX = new double[3];
            this.MArmY = new double[3];
            this.sampler = new System.Random();
            this.arc0 = 35;
            this.arc1 = -35;

            this.CurrentState = new MutableState<double>(2);
        }

        public override EnvironmentDescription<double, int> GetEnvironmentDescription()
        {
            double[] averageState = new double[2] { 14, -55 };
            double[] stddevState = new double[2] { 19, 55 };
            SpaceDescription<double> stateDescription = new SpaceDescription<double>(null, null, averageState, stddevState);
            SpaceDescription<int> actionDescription = new SpaceDescription<int>(new int[] { 0 }, new int[] { 4 }, null, null);
            DimensionDescription<double> reinforcementDescription = new DimensionDescription<double>(-10, 10);

            return new EnvironmentDescription<double, int>(stateDescription, actionDescription, reinforcementDescription, discountFactor);
        }

        public override void StartEpisode()
        {
            this.arc0 = 20;
            this.arc1 = -110;
        }

        public override Reinforcement PerformAction(Action<int> action)
        {
            Contract.Requires(action.Dimensionality == 1);

            double reward = 0;
            double[] ccAction = null;

            switch (action.SingleValue)
            {
                case 0:
                    ccAction = new double[2] { -12, -12 };
                    break;
                case 1:
                    ccAction = new double[2] { -12, 12 };
                    break;
                case 2:
                    ccAction = new double[2] { 12, -12 };
                    break;
                case 3:
                    ccAction = new double[2] { 12, 12 };
                    break;
            }

            ccAction[0] += sampler.NextDouble() * 8 - 4;
            ccAction[1] += sampler.NextDouble() * 8 - 4;

            reward = CalculateTransition(ccAction);

            this.CurrentState[0] = arc0;
            this.CurrentState[1] = arc1;

            return reward;
        }

        private double CalculateTransition(double[] action)
        {
            Contract.Requires(action.Length == 2);

            double a0 = arc0;
            double a1 = arc1;
            double v0 = System.Math.Min(System.Math.Max(-maxV, action[0]), maxV);
            double v1 = System.Math.Min(System.Math.Max(-maxV, action[1]), maxV);
            double reward = -unaryActionPenalty * (System.Math.Abs(action[0] - v0) + System.Math.Abs(action[1] - v1));
            double last_top_x = MArmX[2];
            double dt = internalTimeDiscretization;

            for (double t = 0; t < 1; t += dt)
            {
                arc0 += v0 * dt;
                arc0 = System.Math.Min(System.Math.Max(-4, arc0), 35);
                arc1 += v1 * dt;
                arc1 = System.Math.Min(System.Math.Max(-120, arc1), 10);

                if (armY[2] < 0)
                {
                    reward += last_top_x - MArmX[2];
                }

                this.RefreshInternalState();

                last_top_x = MArmX[2];
            }

            if ((a0 - arc0) * (a0 - arc0) + (a1 - arc1) * (a1 - arc1) < 1e-10)
            {
                reward -= noMovePenalty;
            }

            return reward;
        }

        private void RefreshInternalState()
        {
            armX[1] = armX[0] + l0 * System.Math.Cos(arc0 * System.Math.PI / 180);
            armY[1] = armY[0] + l0 * System.Math.Sin(arc0 * System.Math.PI / 180);
            armX[2] = armX[1] + l1 * System.Math.Cos((arc0 + arc1) * System.Math.PI / 180);
            armY[2] = armY[1] + l1 * System.Math.Sin((arc0 + arc1) * System.Math.PI / 180);
            
            if (armY[2] >= 0)
            {
                System.Array.Copy(bodyX, MBodyX, bodyX.Length);
                System.Array.Copy(bodyY, MBodyY, bodyY.Length);
                System.Array.Copy(armX, MArmX, armX.Length);
                System.Array.Copy(armY, MArmY, armY.Length);
            }
            else
            {
                double sinArc2 = -armY[2] / System.Math.Sqrt(armY[2] * armY[2] + armX[2] * armX[2]);
                double cosArc2 = armX[2] / System.Math.Sqrt(armY[2] * armY[2] + armX[2] * armX[2]);
                for (int i = 0; i < 4; i++)
                {
                    MBodyX[i] = bodyX[i] * cosArc2 - bodyY[i] * sinArc2;
                    MBodyY[i] = bodyY[i] * cosArc2 + bodyX[i] * sinArc2;
                }

                for (int i = 0; i < 3; i++)
                {
                    MArmX[i] = armX[i] * cosArc2 - armY[i] * sinArc2;
                    MArmY[i] = armY[i] * cosArc2 + armX[i] * sinArc2;
                }
            }
        }
        
        private double arc0;
        private double arc1;

        private double[] armX;
        private double[] armY;
        private double[] bodyX;
        private double[] bodyY;

        private double l0 = 32;
        private double l1 = 18;
        private double maxV = 12;

        private System.Random sampler;
    }
}
