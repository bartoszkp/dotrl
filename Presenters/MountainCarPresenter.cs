using System;
using System.Drawing;
using Environments.ContinuousStateDiscreteDecision;

namespace Presenters
{
    class MountainCarPresenter : Presenter
    {
        public MountainCarPresenter(MountainCar environment)
            : base(environment.MinPosition - (environment.MinPosition * 0.1), -1.1, environment.MaxPosition + environment.MaxPosition * 0.1, 1.1)
        {
            this.environment = environment;
            this.pen = new Pen(Color.Blue, 2f);
        }

        public override void Draw()
        {
            Graphics.Clear(Color.WhiteSmoke);

            double length = environment.MaxPosition - environment.MinPosition;
            double step = length / 50;
            double? px = null;
            double? py = null;
            for (double x = environment.MinPosition; x <= environment.MaxPosition; x += step)
            {
                double y = GetHeightAtPosition(x);

                if (px.HasValue)
                {
                    DrawLine(Pens.Black, px.Value, py.Value, x, y);
                }

                px = x;
                py = y;
            }

            double cx = environment.Position;
            double cy = GetHeightAtPosition(cx);
            double r = 0.1;
            FillCircle(Brushes.Red, cx, cy, r);
            FillCircle(Brushes.Green, environment.GoalPosition, GetHeightAtPosition(environment.GoalPosition), 0.12);

            double l = 0.2 / environment.HillPeakFrequency;
            double slope = GetSlope(cx);
            r /= environment.HillPeakFrequency;
            if (environment.LastAction == 0)
            {
                DrawLine(pen, cx - r, cy - r * slope, cx - (r + l), cy - (r + l) * slope);
            }
            else if (environment.LastAction == 2)
            {
                DrawLine(pen, cx + r, cy + r * slope, cx + (r + l), cy + (r + l) * slope);
            }
        }

        private double GetHeightAtPosition(double position)
        {
            return Math.Sin(environment.HillPeakFrequency * position);
        }

        private double GetSlope(double position)
        {
            return environment.HillPeakFrequency * Math.Cos(environment.HillPeakFrequency * position);
        }

        private MountainCar environment;
        private Pen pen;
    }
}
