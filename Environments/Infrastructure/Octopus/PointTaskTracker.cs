using System;
using System.Linq;
using Environments.ContinuousStateContinuousDecision;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class PointTaskTracker : ITaskTracker
    {
        private Octopus env;
        private bool terminated;
        private double terminalReward;
        private double minTargetRadius;
        private double maxTargetRadius;
        private double prevDistance;
        private double currentReward;
        private Random sampler;

        public PointTaskTracker(Octopus parent, PointTaskDef def)
        {
            env = parent;
            terminated = false;
            terminalReward = def.Reward;
            minTargetRadius = def.MinTargetRadius;
            maxTargetRadius = def.MaxTargetRadius;
            sampler = new Random();
            env.Targets = new Target[] { new Target(10, 0, terminalReward) };
            Reset();
        }

        public bool Terminal
        {
            get { return terminated; }
        }

        public double Reward
        {
            get { return currentReward; }
        }

        public void Reset()
        {
            terminated = false;
            double phi = sampler.NextDouble() * Math.PI / 2 - Math.PI / 4;
            double r = minTargetRadius + sampler.NextDouble() * (maxTargetRadius - minTargetRadius);
            env.Targets.First().Position.PositionX = Math.Cos(phi) * r;
            env.Targets.First().Position.PositionY = Math.Sin(phi) * r;
            prevDistance = env.Targets.First().Position.GetDistanceFrom(env.Arm.Compartments.Last().CenterX, env.Arm.Compartments.Last().CenterY);
        }

        public void Update()
        {
            if (env.Arm.Compartments.Last().Contains(env.Targets.First().Position))
            {
                currentReward = prevDistance + terminalReward;
                prevDistance = 0;
                terminated = true;
            }
            else
            {
                double act_distance = env.Targets.First().Position.GetDistanceFrom(env.Arm.Compartments.Last().CenterX, env.Arm.Compartments.Last().CenterY);
                currentReward = (prevDistance - act_distance) - 1;
                prevDistance = act_distance;
            }
        }
    }
}
