using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Parameters;

namespace Environments.ContinuousStateDiscreteDecision
{
    public class MountainCar : Environment<double, int>
    {
        public double Position { get; private set; }
        
        public double Velocity { get; private set; }

        [Parameter("Minimum position")]
        public double MinPosition { get; private set; }
        [Parameter("Maximum position")]
        public double MaxPosition { get; private set; }
        [Parameter("Minimum velocity")]
        private double minVelocity = -0.07;
        [Parameter("Maximum velocity")]
        private double maxVelocity = 0.07;
        [Parameter("Goal position")]
        public double GoalPosition { get; private set; }
        [Parameter("Acceleration factor")]
        private double accelerationFactor = 0.001;
        [Parameter("Gravity factor")]
        private double gravityFactor = -0.0025;
        [Parameter("Hill peak frequency")]
        public double HillPeakFrequency { get; private set; }
        [Parameter("Reward per step")]
        private double rewardPerStep = -1.0;
        [Parameter("Reward at goal")]
        private double rewardAtGoal = 0.0;
        [Parameter("Initial position")]
        private double initialPosition = -0.5;
        [Parameter("Initial velocity")]
        private double initialVelocity = 0.0d;

        public int LastAction { get; private set; }

        public MountainCar()
        {
            HillPeakFrequency = 3.0;
            MinPosition = -1.2;
            MaxPosition = 0.6;
            GoalPosition = 0.5;

            this.CurrentState = new Core.MutableState<double>(2);
            this.random = new Random();
        }

        public override Core.EnvironmentDescription<double, int> GetEnvironmentDescription()
        {
            var stateSpaceDescription = new Core.SpaceDescription<double>(
                new[] { MinPosition, minVelocity },
                new[] { MaxPosition, maxVelocity },
                new[] { (MinPosition + MaxPosition) / 2, (minVelocity + maxVelocity) / 2 },
                new[] { 0.1, 0.1 });
            var actionSpaceDescription = new Core.SpaceDescription<int>(
                new[] { 0 },
                new[] { 2 });
            var reinforcementSpaceDescription = new Core.DimensionDescription<double>(-1, 0);

            return new Core.EnvironmentDescription<double, int>(stateSpaceDescription, actionSpaceDescription, reinforcementSpaceDescription, 0.9);
        }

        public override void StartEpisode()
        {
            Position = initialPosition + 0.25 * (random.NextDouble() - 0.5);
            Velocity = initialVelocity + 0.025 * (random.NextDouble() - 0.5);
        }

        public override Core.Reinforcement PerformAction(Core.Action<int> action)
        {
            var transitionNoise = 0.0;
            var noise = 2.0 * accelerationFactor * transitionNoise * (random.NextDouble() - 0.5);

            if (action.SingleValue < 0 || action.SingleValue > 2)
                throw new InvalidOperationException();

            double acceleration = accelerationFactor;

            Velocity += (noise + (action.SingleValue - 1) * acceleration) + GetSlope(Position) * gravityFactor;

            Velocity = Math.Min(Velocity, maxVelocity);
            Velocity = Math.Max(Velocity, minVelocity);

            Position += Velocity;

            Position = Math.Min(Position, MaxPosition);
            Position = Math.Max(Position, MinPosition);

            if (Position == MinPosition && Velocity < 0)
            {
                Velocity = 0;
            }

            UpdateCurrentState();

            LastAction = action.SingleValue;

            return Position >= GoalPosition ? rewardAtGoal : rewardPerStep;
        }

        private double GetSlope(double position)
        {
            return Math.Cos(HillPeakFrequency * position);
        }

        private void UpdateCurrentState()
        {
            this.CurrentState[0] = Position;
            this.CurrentState[1] = Velocity * Velocity;
            this.CurrentState.IsTerminal = Position >= GoalPosition;
        }

        private Random random;
    }
}
