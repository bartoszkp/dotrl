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
    public class Point
    {
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double getX()
        {
            return x;
        }

        public double getY()
        {
            return y;
        }

        public double distanceTo(Point t)
        {
            double dd = Math.Sqrt(Math.Pow((t.x - x), 2) + Math.Pow((t.y - y), 2));
            return dd;
        }

        public Point minus(Point b)
        {
            return new Point(this.x - b.x, this.y - b.y);
        }

        public double dot(Point b)
        {
            return (this.x * b.x) + (this.y * b.y);
        }

        public Point times(double d)
        {
            return new Point(x * d, y * d);
        }

        public Point add(Point b)
        {
            return new Point(x + b.x, y + b.y);
        }

        public Point addTo(Point b)
        {
            x += b.x;
            y += b.y;

            return this;
        }

        public double size()
        {
            return Math.Sqrt(this.dot(this));
        }

        public Point normalize()
        {
            double nrm = Math.Sqrt(this.dot(this));
            return new Point(x / nrm, y / nrm);
        }

        public double angleBetween(Point p)
        {
            double res = Math.Atan2(x, y) - Math.Atan2(p.getX(), p.getY());
            if (res < 0)
                res = res + (Math.PI * 2.0);

            return res;
        }

        private double x, y;
    }
}
