using BackwardCompatibility;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    public class Mouth
    {
        public double PositionX { get; private set; }

        public double PositionY { get; private set; }

        public double Width { get; private set; }

        public double Height { get; private set; }

        public Mouth(MouthSpec spec)
        {
            PositionX = spec.PositionX;
            PositionY = spec.PositionY;
            Width = spec.Width;
            Height = spec.Height;
        }

        public bool Contains(Vector2D point)
        {
            double a2 = (Width / 2) * (Width / 2);
            double b2 = (Height / 2) * (Height / 2);

            double x = point.PositionX - (PositionX + Width / 2);
            double y = point.PositionY - (PositionY + Height / 2);

            return (x * x / a2) + (y * y / b2) < 1;
        }
    }
}