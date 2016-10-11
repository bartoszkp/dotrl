/* Copyright 2009 George Konidaris
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

namespace Environments.Infrastructure.Pinball
{
    public class Target
    {
        public Target(Point P, double rad)
        {
            x = P.getX();
            y = P.getY();
            radius = rad;
        }

        public double[] collisionEffect(Ball b)
        {
            double[] d = { 0, 0 };
            return d;
        }

        public Point getCenter()
        {
            return new Point(x, y);
        }

        public bool collision(Ball b)
        {
            return b.getCenter().distanceTo(getCenter()) < radius;
        }

        public bool inside(Point p)
        {
            return getCenter().distanceTo(p) < radius;
        }

        public static bool matchTag(string line)
        {
            return line.StartsWith("target");
        }

        public static Target create(string line)
        {
            var tokens = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            double xx = double.Parse(tokens[1], System.Globalization.CultureInfo.InvariantCulture);
            double yy = double.Parse(tokens[2], System.Globalization.CultureInfo.InvariantCulture);
            double rad = double.Parse(tokens[3], System.Globalization.CultureInfo.InvariantCulture);

            return new Target(new Point(xx, yy), rad);
        }

        public Point getIntercept()
        {
            return null;
        }

        public double getX()
        {
            return x;
        }

        public double getY()
        {
            return y;
        }

        public double getRadius()
        {
            return radius;
        }

        private double x, y, radius;
    }
}