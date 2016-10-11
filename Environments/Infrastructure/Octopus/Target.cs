using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    public class Target
    {
        public Vector2D Position { get; private set; }

        public double Value { get; private set; }

        public bool Eligible { get; set; }

        public Target(TargetSpec spec)
        {
            Position = Vector2D.FromDuple(spec.Position);
            Value = spec.Reward;
            Eligible = false;
        }

        public Target(double positionX, double positionY, double value)
        {
            Position = new Vector2D(positionX, positionY);
            Value = value;
            Eligible = false;
        }
    }
}