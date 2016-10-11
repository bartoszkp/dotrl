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
 *  Ported to C# by bartoszkp@gmail.com
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace Environments.Infrastructure.Pinball
{
    public class Obstacle
    {
        Obstacle(IEnumerable<Point> points)
        {
            Points = points.ToArray();
            computeBounds();
        }

        protected int selectIntercept(int a, int b, Ball ball)
        {
            int anext = a + 1;
            if (anext == Points.Length)
                anext = 0;

            int bnext = b + 1;
            if (bnext == Points.Length)
                bnext = 0;

            Point a_edge = Points[a].minus(Points[anext]);
            Point b_edge = Points[b].minus(Points[bnext]);

            Point ball_v = new Point(ball.getXDot(), ball.getYDot());

            double th_a = ball_v.angleBetween(a_edge);
            if (th_a > Math.PI)
                th_a -= Math.PI;

            double th_b = ball_v.angleBetween(b_edge);
            if (th_b > Math.PI)
                th_b -= Math.PI;

            double a_dist = Math.Abs(th_a - (Math.PI / 2.0));
            double b_dist = Math.Abs(th_b - (Math.PI / 2.0));

            if (a_dist < b_dist)
                return a;

            return b;
        }

        public bool collision(Ball b)
        {
            bool found = false;
            double_collision = false;

            if (b.getX() - b.getRadius() > max_x)
                return false;
            if (b.getY() - b.getRadius() > max_y)
                return false;
            if (b.getX() + b.getRadius() < min_x)
                return false;
            if (b.getY() + b.getRadius() < min_y)
                return false;

            for (int j = 0; j < Points.Length; j++)
            {
                int next = j + 1;
                if (next == Points.Length)
                    next = 0;

                if (lineIntersect(b, Points[j], Points[next]))
                {
                    if (found)
                    {
                        intercept_edge = selectIntercept(intercept_edge, j, b);
                        double_collision = true;
                    }
                    else
                    {
                        intercept_edge = j;
                        found = true;
                    }
                }
            }

            return found;
        }

        protected bool lineIntersect(Ball ball, Point p1, Point p2)
        {
            Point dir = p2.minus(p1);
            Point diff = ball.getCenter().minus(p1);

            double t = diff.dot(dir) / dir.dot(dir);

            if (t < 0.0)
                t = 0.0;
            if (t > 1.0)
                t = 1.0;

            Point closest = p1.add(dir.times(t));
            Point d = ball.getCenter().minus(closest);

            intercept = closest;

            Point rev = ball.getCenter().minus(intercept);
            rev = rev.normalize();
            rev = rev.times(ball.getVelocity());

            double distsqrt = d.dot(d);

            if (distsqrt <= ball.getRadius() * ball.getRadius())
            {
                Point direction = new Point(ball.getXDot(), ball.getYDot());
                Point dd = closest.minus(ball.getCenter());
                double thet = dd.angleBetween(direction);

                if (thet > Math.PI)
                    thet = 2 * Math.PI - thet;

                // Make sure the ball is not already heading away
                // from the obstacle
                if (thet > Math.PI / 1.99)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }

        public double[] collisionEffect(Ball b)
        {
            // Corners are difficult, just bounce directly off.
            if (double_collision)
            {
                double[] rd = new double[2];
                rd[0] = -b.getXDot();
                rd[1] = -b.getYDot();
                return rd;
            }

            int edge2 = intercept_edge + 1;
            if (edge2 == Points.Length)
                edge2 = 0;

            // Edge_dir is from left to right.
            Point edge_dir = Points[intercept_edge].minus(Points[edge2]);
            if (edge_dir.getX() < 0)
                edge_dir = Points[edge2].minus(Points[intercept_edge]);

            Point ball_v = new Point(b.getXDot(), b.getYDot());

            double theta = ball_v.angleBetween(edge_dir);

            // Rotate 180 degrees
            theta -= Math.PI;
            if (theta < 0)
                theta += (Math.PI * 2.0);

            // Adjust for the rotation of the obstacle line
            double edge_theta = (new Point(-1, 0)).angleBetween(edge_dir);
            theta += edge_theta;
            if (theta > Math.PI * 2.0)
                theta -= Math.PI * 2.0;

            Point out_vel = new Point(b.getVelocity() * Math.Cos(theta), b.getVelocity() * Math.Sin(theta));

            double[] d = new double[2];
            d[0] = out_vel.getX();
            d[1] = out_vel.getY();

            intercept = out_vel.add(b.getCenter());

            return d;
        }

        public static bool matchTag(string line)
        {
            return line.StartsWith("polygon");
        }

        public static Obstacle create(string line)
        {
            var tokens = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<Point> p = new List<Point>();

            for (int i = 1; i < tokens.Length; i += 2)
            {
                double x = double.Parse(tokens[i], System.Globalization.CultureInfo.InvariantCulture);
                double y = double.Parse(tokens[i + 1], System.Globalization.CultureInfo.InvariantCulture);

                p.Add(new Point(x, y));
            }

            return new Obstacle(p);
        }

        public IEnumerable<Point> getPoints()
        {
            return Points;
        }

        public Point getIntercept()
        {
            return intercept;
        }

        public bool inside(Point p)
        {
            double testx = p.getX();
            double testy = p.getY();

            int nvert = Points.Length;

            int i, j;
            bool c = false;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((Points[i].getY() > testy) != (Points[j].getY() > testy)) &&
                    (testx < (Points[j].getX() - Points[i].getX()) * (testy - Points[i].getY()) / (Points[j].getY() - Points[i].getY()) + Points[i].getX()))

                    c = !c;
            }

            return c;
        }

        protected void computeBounds()
        {
            max_x = 0;
            max_y = 0;
            min_x = 1;
            min_y = 1;

            foreach (Point p in Points)
            {
                max_x = Math.Max(max_x, p.getX());
                max_y = Math.Max(max_y, p.getY());
                min_x = Math.Min(min_x, p.getX());
                min_y = Math.Min(min_y, p.getY());
            }
        }

        private bool double_collision = false;
        private double max_x, max_y, min_x, min_y;

        private Point intercept;
        int intercept_edge;
        Point[] Points;
    }
}
