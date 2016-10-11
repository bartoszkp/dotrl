using System;
using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    public class Food : Node
    {
        public double Value { get; private set; }

        public Food(FoodSpec spec)
            : base(spec)
        {
            Value = spec.Reward;
        }

        public virtual void Warp()
        {
            double coord = (0.5 + new Random(1).NextDouble() / 2.0) * double.MaxValue;
            Position = new Vector2D(coord, coord);
        }
    }
}