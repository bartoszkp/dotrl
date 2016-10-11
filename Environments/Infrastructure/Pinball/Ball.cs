/*
 * Copyright 2009 George Konidaris
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * Ported to C# by bartoszkp@gmail.com
 */
using System;
using System.Linq;

namespace Environments.Infrastructure.Pinball
{
    public class Ball
    {
        public Ball(Point p, double rad)
        {
            x = p.getX();
            y = p.getY();
            xdot = 0;
            ydot = 0;
            radius = rad;
        }

        public double getRadius()
        {
            return radius;
        }

        public double getX()
        {
            return x;
        }

        public double getY()
        {
            return y;
        }

        public double getXDot()
        {
            return xdot;
        }

        public double getYDot()
        {
            return ydot;
        }

        public static bool matchTag(string line)
        {
            return line.StartsWith("ball");
        }

        public static Ball create(string line)
        {
            var tokens = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            double rad = double.Parse(tokens.Last(), System.Globalization.CultureInfo.InvariantCulture);

            return new Ball(new Point(0, 0), rad);
        }

        public void step()
        {
            x += (xdot * radius / 20.0);
            y += (ydot * radius / 20.0);
        }

        public void addDrag()
        {
            xdot = DRAG * xdot;
            ydot = DRAG * ydot;
        }

        public double getVelocity()
        {
            Point p = new Point(xdot, ydot);
            return Math.Sqrt(p.dot(p));
        }

        public void addImpulse(double tox, double toy)
        {
            xdot += (tox / 5.0);
            ydot += (toy / 5.0);

            xdot = clip(xdot, -1.0, 1.0);
            ydot = clip(ydot, -1.0, 1.0);
        }

        protected double clip(double d, double low, double high)
        {
            if (d > high)
                d = high;
            if (d < low)
                d = low;

            return d;
        }

        public void setVelocities(double dx, double dy)
        {
            xdot = dx;
            ydot = dy;
        }

        public void setPosition(double xx, double yy)
        {
            x = xx;
            y = yy;

            // Clear velocities.
            xdot = 0;
            ydot = 0;
        }

        public Point getCenter()
        {
            return new Point(x, y);
        }

        public const double DRAG = 0.995;

        public double x, y;
        public double xdot, ydot;
        public double radius;
    }
}
