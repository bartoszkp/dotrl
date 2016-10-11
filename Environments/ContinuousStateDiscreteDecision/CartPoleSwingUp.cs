using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Parameters;

namespace Environments.ContinuousStateDiscreteDecision
{
    public class CartPoleSwingUp : Environment<double, int>
    {
        [Parameter(-10, -1)]
        private double minX = -2.4;
        [Parameter(1, 10)]
        private double maxX = 2.4;
        [Parameter(0.1, 10)]
        private double rewardAreaRadius = 2.4;
        [Parameter(0, 1000)]
        private double boundryPenalty = 100;
        [Parameter(0, 1)]
        private double discountFactor = 0.9;

        [Parameter(0, double.PositiveInfinity)] // gravityAcceleration
        private double g = 9.81;
        [Parameter(0, double.PositiveInfinity)] // cart's mass
        private double m_c = 1;
        [Parameter(0, double.PositiveInfinity)] // pole's mass
        private double m_p = 0.1;
        [Parameter(0, double.PositiveInfinity)] // half of pole's length
        private double l = 0.5;
        [Parameter(0, double.PositiveInfinity)] // friction coefficient between the cart and the ground
        private double mic = 0.0005;
        [Parameter(0, double.PositiveInfinity)] // friction coefficient between the pole and the cart
        private double mip = 0.000002;

        public double Position
        {
            get
            { 
                return this.x; 
            }
        }

        public double Velocity 
        { 
            get 
            { 
                return this.dX;
            } 
        }

        public double Arc 
        { 
            get 
            {
                return this.theta; 
            } 
        }

        public double AngularVelocity 
        {
            get
            { 
                return this.dTheta; 
            } 
        }

        public double MinimumPositionConstraint 
        {
            get
            {
                return this.minX;
            }
        }

        public double MaximumPositionConstraint 
        {
            get 
            {
                return this.maxX; 
            }
        }

        public CartPoleSwingUp()
        {
            this.sampler = new System.Random();

            actionMap[0] = -5;
            actionMap[1] = -0.1;
            actionMap[2] = 0.1;
            actionMap[3] = 5;

            this.CurrentState = new MutableState<double>(5);
        }

        public void StartEpisodeInDefinedState(double position, double velocity, double angle, double angularVelocity)
        {
            this.x = position;
            this.dX = velocity;
            this.theta = angle;
            this.dTheta = angularVelocity;

            RectifyState();
            UpdateCurrentState();
        }

        public override void StartEpisode()
        {
            this.x = 0;
            this.dX = 0;
            this.theta = Math.PI;
            this.dTheta = 0;

            UpdateCurrentState();
        }

        public override Reinforcement PerformAction(Core.Action<int> action)
        {
            double force = this.actionMap[action.ActionVector.First()];
            
            double reward = 0;

            for (double t = 0; t < externalDiscretization; t += internalDiscretization)
            {
                double dt = Math.Min(internalDiscretization, externalDiscretization - t);
                double dt2 = dt * dt;

                double d2theta = this.TimeDerivativeFromTheta(this.dX, this.sinTheta, this.cosTheta, this.dTheta, force);
                double d2x = TimeDerivativeFromX(this.dX, this.sinTheta, this.cosTheta, this.dTheta, force, d2theta);
                this.x += (dX * dt) + (0.5 * d2x * dt2);
                this.dX += d2x * dt;

                this.theta += this.dTheta * dt + 0.5 * d2theta * dt2;
                this.dTheta += d2theta * dt;

                this.RectifyState();
            }
            
            if (IsStateValid)
            {
                if (Math.Abs(this.dTheta) < Math.PI * 2 && Math.Abs(this.x) < rewardAreaRadius)
                {
                    reward += this.cosTheta;
                }
                else
                {
                    reward += -1;
                }
            }
            else
            {
                reward -= boundryPenalty;
            }

            UpdateCurrentState();

            return reward;
        }

        public override EnvironmentDescription<double, int> GetEnvironmentDescription()
        {
            double[] minimum = new double[] { minX, -100, -1, -1, -100 };
            double[] maximum = new double[] { maxX, 100, 1, 1, 100 };
            double[] average = new double[] { 0, 0, 0, 0, 0 };
            double[] standardDeviation = new double[] { 2, 3, 0.8, 0.8, 4 };

            return new EnvironmentDescription<double, int>(
                new SpaceDescription<double>(minimum, maximum, average, standardDeviation),
                SpaceDescription<int>.CreateOneDimensionalSpaceDescription(0, 3),
                new DimensionDescription<double>(-100, 1),
                this.discountFactor);
        }

        private bool IsStateValid
        {
            get
            {
                return this.minX <= this.x && this.x <= this.maxX;
            }
        }

        private void UpdateCurrentState()
        {
            this.CurrentState[0] = this.x;
            this.CurrentState[1] = this.dX;
            this.CurrentState[2] = this.sinTheta;
            this.CurrentState[3] = this.cosTheta;
            this.CurrentState[4] = this.dTheta;
            this.CurrentState.IsTerminal = !IsStateValid;
        }

        private double TimeDerivativeFromTheta(double dx, double sinTheta, double cosTheta, double dtheta, double force)
        {
            double insided2Theta = (-force - m_p * l * dtheta.Squared() * sinTheta + mic * dx.NonZeroSignum()) / (m_c + m_p); 
            double d2theta_dt =
                ((g * sinTheta) + (cosTheta * insided2Theta) - (mip * dtheta / (m_p * l)))
                /
                (l * ((4.0 / 3.0) - ((m_p * cosTheta.Squared()) / (m_c + m_p))));
            return d2theta_dt;
        }

        private double TimeDerivativeFromX(double dx, double sinTheta, double cosTheta, double dtheta, double force, double d2theta_dt)
        {
            double insided2X = ((dtheta.Squared() * sinTheta) - (d2theta_dt * cosTheta));
            double d2x_dt =
                (force + (m_p * l * insided2X) - (mic * dx.NonZeroSignum()))
                /
                (m_c + m_p);
            return d2x_dt;
        }

        private void RectifyState()
        {
            while (this.theta < 0)
            {
                this.theta += Math.PI * 2;
            }

            while (this.theta > Math.PI * 2)
            {
                this.theta -= Math.PI * 2;
            }

            this.sinTheta = Math.Sin(this.theta);
            this.cosTheta = Math.Cos(this.theta);
        }

        private double x;
        private double dX;
        private double theta;
        private double sinTheta;
        private double cosTheta;
        private double dTheta;
        private double externalDiscretization = 0.1;
        private double internalDiscretization = 0.01;
        private System.Random sampler;
        private Dictionary<int, double> actionMap = new Dictionary<int, double>();
    }
}
