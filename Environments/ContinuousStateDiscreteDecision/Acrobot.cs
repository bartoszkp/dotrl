using Core;
using Core.Parameters;

namespace Environments.ContinuousStateDiscreteDecision
{
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Generic;
    using Matrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix;

    public class Acrobot : Environment<double, int>
    {
        [Parameter(0, 1)]
        private double discountFactor = 0.9;
        [Parameter(1, 100)]
        private double force = 1;
        [Parameter(0.0001, 1)]
        private double externalDiscretization = 0.2;
        [Parameter(0.0001, 1)]
        private double internalDiscretization = 0.01;
        [Parameter(-100, 100)]
        private double lX = 1;
        [Parameter(-100, 100)]
        private double lY = 1;
        [Parameter(-100, 100)]
        private double lcX = 0.5;
        [Parameter(-100, 100)]
        private double lcY = 0.5;
        [Parameter(-100, 100)]
        private double mX = 1;
        [Parameter(-100, 100)]
        private double mY = 1;
        [Parameter(-100, 100)]
        private double iX = 1;
        [Parameter(-100, 100)]
        private double iY = 1;
        [Parameter(0, 100)]
        private double g = 9.81;

        public double Theta1 { get; private set; }

        public double SinTheta1 { get; private set; }

        public double CosTheta1 { get; private set; }

        public double Theta2 { get; private set; }

        public double SinTheta2 { get; private set; }

        public double CosTheta2 { get; private set; }

        public Vector<double> ElbowPosition { get; private set; }

        public Vector<double> TopPosition { get; private set; }

        public Acrobot()
        {
            this.c = new DenseVector(2);
            this.phi = new DenseVector(2);
            this.d = new Matrix(2, 2);
            this.tau = new DenseVector(2);
            this.d2thetadt2 = new DenseVector(2);

            this.Theta1 = System.Math.PI * 3 / 2;
            this.Theta2 = 0;

            this.sampler = new System.Random();

            this.CurrentState = new MutableState<double>(6);
            this.ElbowPosition = new DenseVector(2);
            this.TopPosition = new DenseVector(2);

            this.RectifyState();
        }

        public override void StartEpisode()
        {
            this.Theta1 = sampler.NextDouble() * 2 * System.Math.PI;
            this.Theta2 = sampler.NextDouble() * 2 * System.Math.PI;

            this.RectifyState();
        }

        public override Reinforcement PerformAction(Action<int> action)
        {
            CalculateState(
                0,
                action[0] == 1 ? this.force : -this.force,
                externalDiscretization,
                (int)(externalDiscretization / internalDiscretization) + 1);

            double cos12 = this.CurrentState[1] * this.CurrentState[4] - this.CurrentState[0] * this.CurrentState[3];
            
            return -this.lX * this.CurrentState[1]
                - this.lY * cos12
                - 0.01 * this.CurrentState[2].Squared()
                - 0.01 * this.CurrentState[5].Squared();
        }

        public override EnvironmentDescription<double, int> GetEnvironmentDescription()
        {
            SpaceDescription<int> actionDescription = new SpaceDescription<int>(new int[1] { 0 }, new int[1] { 1 }, null, null);
            double[] averageState = new double[6];
            double[] stddevState = new double[6];
            averageState[0] = 0;
            stddevState[0] = 0.8;
            averageState[1] = 0;
            stddevState[1] = 0.8;
            averageState[2] = 0;
            stddevState[2] = 5;
            averageState[3] = 0;
            stddevState[3] = 0.8;
            averageState[4] = 0;
            stddevState[4] = 0.8;
            averageState[5] = 0;
            stddevState[5] = 5;
            SpaceDescription<double> stateDescription = new SpaceDescription<double>(null, null, averageState, stddevState);
            DimensionDescription<double> rewardDescription = new DimensionDescription<double>(-10, 10);

            return new EnvironmentDescription<double, int>(stateDescription, actionDescription, rewardDescription, this.discountFactor);
        }

        private void RectifyState()
        {
            while (this.Theta1 < 0)
            {
                this.Theta1 += System.Math.PI * 2;
            }

            while (this.Theta1 > System.Math.PI * 2)
            {
                this.Theta1 -= System.Math.PI * 2;
            }

            while (this.Theta2 < 0)
            {
                this.Theta2 += System.Math.PI * 2;
            }

            while (this.Theta2 > System.Math.PI * 2)
            {
                this.Theta2 -= System.Math.PI * 2;
            }

            this.SinTheta1 = System.Math.Sin(this.Theta1);
            this.CosTheta1 = System.Math.Cos(this.Theta1);
            this.SinTheta2 = System.Math.Sin(this.Theta2);
            this.CosTheta2 = System.Math.Cos(this.Theta2);

            this.ElbowPosition[0] = this.lX * this.CosTheta1;
            this.ElbowPosition[1] = this.lX * this.SinTheta1;

            this.TopPosition[0] = lX * this.CosTheta1 + lY * (this.CosTheta1 * this.CosTheta2 - this.SinTheta1 * this.SinTheta2);
            this.TopPosition[1] = lX * this.SinTheta1 + lY * (this.SinTheta1 * this.CosTheta2 + this.SinTheta2 * this.CosTheta1);

            this.CurrentState[0] = this.SinTheta1;
            this.CurrentState[1] = this.CosTheta1;
            this.CurrentState[2] = this.dtheta1dt;
            this.CurrentState[3] = this.SinTheta2;
            this.CurrentState[4] = this.CosTheta2;
            this.CurrentState[5] = this.dtheta2dt;
        }

        private void CalculateState(
            double f1,
            double f2,
            double dt,
            int timedivision)
        {
            dt /= timedivision;
            this.tau[0] = f1;
            this.tau[1] = f2;

            for (int i = 0; i < timedivision; i++)
            {
                this.d[0, 0] = this.mX * this.lcX.Squared()
                    + this.mY * (this.lX.Squared() + this.lcY.Squared() + 2.0 * this.lX * this.lcY * this.CosTheta2)
                    + this.iX
                    + this.iY;
                this.d[1, 1] = this.mY * this.lcY.Squared() + this.iY;
                this.d[0, 1] = this.mY * (this.lcY.Squared() + this.lX * this.lcY * this.CosTheta2) + this.iY;
                this.d[1, 0] = d[0, 1];

                this.c[0] = -this.mY * this.lX * this.lcY * this.dtheta2dt.Squared() * this.SinTheta2
                    - 2.0 * this.mY * this.lX * this.lcY * this.dtheta1dt * dtheta2dt * this.SinTheta2;
                this.c[1] = this.mY * this.lX * this.lcY * this.dtheta1dt.Squared() * this.SinTheta2;

                // cos1p2 = cos(theta1+theta2)
                double cos1p2 = this.CosTheta1 * this.CosTheta2 - this.SinTheta1 * this.SinTheta2;

                this.phi[0] = (this.mX * this.lcX + this.mY * this.lX) * g * this.CosTheta1
                    + this.mY * this.lcY * g * cos1p2;
                this.phi[1] = this.mY * this.lcY * g * cos1p2;

                this.phi[0] += this.dtheta1dt * 0.1;
                this.phi[1] += this.dtheta2dt * 0.1;

                this.d2thetadt2 = d.Inverse() * (tau - c - phi);

                this.Theta1 += dtheta1dt * dt + d2thetadt2[0] * 0.5 * dt.Squared();
                this.SinTheta1 = System.Math.Sin(this.Theta1);
                this.CosTheta1 = System.Math.Cos(this.Theta1);
                this.dtheta1dt += this.d2thetadt2[0] * dt;

                this.Theta2 += this.dtheta2dt * dt + this.d2thetadt2[1] * 0.5 * dt.Squared();
                this.SinTheta2 = System.Math.Sin(this.Theta2);
                this.CosTheta2 = System.Math.Cos(this.Theta2);
                this.dtheta2dt += this.d2thetadt2[1] * dt;
            }

            RectifyState();
        }

        private double dtheta1dt;
        private double dtheta2dt;
        private System.Random sampler;
        private Vector c;
        private Vector phi;
        private Matrix d;
        private Vector tau;
        private MathNet.Numerics.LinearAlgebra.Generic.Vector<double> d2thetadt2;
    }
}
