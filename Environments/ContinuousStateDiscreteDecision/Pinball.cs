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
* Ported to dotRL/C# by bartoszkp@gmail.com
*/
using System;
using System.Collections.Generic;
using Core.Parameters;
using Environments.Infrastructure.Pinball;

namespace Environments.ContinuousStateDiscreteDecision
{
    public class Pinball : Environment<double, int>
    {
        [Parameter(StringParameterType.Multiline, "Configuration")]
        private string configuration = Pinball.defaultConfiguration;

        [Parameter("Target horizontal position")]
        public double TargetX { get; private set; }

        [Parameter("Target vertical position")]
        public double TargetY { get; private set; }

        [Parameter("Target radius")]
        public double TargetRadius { get; private set; }

        [Parameter("Ball radius")]
        public double BallRadius { get; private set; }

        public Ball Ball { get; private set; }

        public Target Target { get; private set; }

        public Pinball()
        {
            this.TargetX = 0;
            this.TargetY = 0;
            this.TargetRadius = 0.05;
            this.BallRadius = 0.015;
            this.intercept = new Point(0, 0);

            this.random = new Random();

            this.CurrentState = new Core.MutableState<double>(4);
        }

        public override void ParametersChanged()
        {
            var lines = configuration.Split('\n');
            List<Obstacle> os = new List<Obstacle>();
            List<Ball> ss = new List<Ball>();
            foreach (var line in lines)
            {
                if (Ball.matchTag(line))
                {
                    Ball = Ball.create(line);
                }
                else if (Target.matchTag(line))
                {
                    Target = Target.create(line);
                }
                else if (Obstacle.matchTag(line))
                {
                    Obstacle po = Obstacle.create(line);
                    os.Add(po);
                }
                else if (line.StartsWith("start"))
                {
                    var tokens = line.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 1; i < tokens.Length; i += 2)
                    {
                        double xx = double.Parse(tokens[i], System.Globalization.CultureInfo.InvariantCulture);
                        double yy = double.Parse(tokens[i + 1], System.Globalization.CultureInfo.InvariantCulture);

                        Ball s = new Ball(new Point(xx, yy), 0);
                        ss.Add(s);
                    }
                }
            }

            if (Ball == null || Target == null)
            {
                throw new InvalidOperationException("Ball or target not loaded");
            }

            obstacles = os.ToArray();
            startStates = ss.ToArray();
        }

        public override Core.EnvironmentDescription<double, int> GetEnvironmentDescription()
        {
            var stateSpaceDescription = new Core.SpaceDescription<double>(
                new[] { 0.0, 0.0, -1.0, -1.0 },
                new[] { 1.0, 1.0, 1.0, 1.0 },
                new[] { 0.5, 0.5, 0.0, 0.0 },
                new[] { 0.1, 0.1, 0.1, 0.1 });
            var actionSpaceDescription = new Core.SpaceDescription<int>(
                new[] { 0 }, new[] { 3 });
            var reinforcementSpaceDescription = new Core.DimensionDescription<double>(-5, 10000);

            return new Core.EnvironmentDescription<double, int>(
                stateSpaceDescription,
                actionSpaceDescription,
                reinforcementSpaceDescription,
                0.95);
        }

        public override void StartEpisode()
        {
            int r = random.Next(startStates.Length);
            this.Ball.x = startStates[r].x;
            this.Ball.y = startStates[r].y;
            this.Ball.xdot = random.NextDouble();
            this.Ball.ydot = random.NextDouble();
        }

        public override Core.Reinforcement PerformAction(Core.Action<int> action)
        {
            switch (action.SingleValue)
            {
                case 0:
                    this.Ball.xdot += 0.2;
                    break;
                case 1:
                    this.Ball.ydot += 0.2;
                    break;
                case 2:
                    this.Ball.xdot -= 0.2;
                    break;
                case 3:
                    this.Ball.ydot -= 0.2;
                    break;
            }

            this.Ball.xdot = Math.Min(Math.Max(this.Ball.xdot, -1), 1);
            this.Ball.ydot = Math.Min(Math.Max(this.Ball.ydot, -1), 1);

            for (int i = 1; i < 20; ++i)
            {
                this.Ball.step();

                int collisions = 0;
                double dx = 0;
                double dy = 0;

                foreach (Obstacle o in obstacles)
                {
                    if (o.collision(Ball))
                    {
                        double[] d = o.collisionEffect(Ball);
                        dx += d[0];
                        dy += d[1];
                        collisions++;
                        intercept = o.getIntercept();
                    }
                }

                if (collisions == 1)
                {
                    this.Ball.setVelocities(dx, dy);

                    if (i == 19)
                    {
                        this.Ball.step();
                    }
                }
                else if (collisions > 1)
                {
                    this.Ball.setVelocities(-this.Ball.getXDot(), -this.Ball.getYDot());
                }

                if (episodeEnd())
                {
                    UpdateCurrentState();
                    return 10000;
                }
            }

            this.Ball.addDrag();
            checkBounds();

            UpdateCurrentState();

            if (action.SingleValue == 4)
                return -1;

            return -5;
        }

        protected void checkBounds()
        {
            Point p = Ball.getCenter();

            if (p.getX() > 1.0)
            {
                Ball.setPosition(0.95, Ball.getY());
            }
            else if (p.getX() < 0.0)
            {
                Ball.setPosition(0.05, Ball.getY());
            }

            p = Ball.getCenter();

            if (p.getY() > 1.0)
            {
                Ball.setPosition(Ball.getX(), 0.95);
            }
            else if (p.getY() < 0.0)
            {
                Ball.setPosition(Ball.getX(), 0.05);
            }
        }

        public bool episodeEnd()
        {
            return Target.collision(Ball);
        }

        private void UpdateCurrentState()
        {
            this.CurrentState[0] = this.Ball.x;
            this.CurrentState[1] = this.Ball.y;
            this.CurrentState[2] = this.Ball.xdot;
            this.CurrentState[3] = this.Ball.ydot;

            this.CurrentState.IsTerminal = episodeEnd();
        }

        private Random random;
        private Obstacle[] obstacles;
        private Ball[] startStates;
        private Point intercept;

        private static string defaultConfiguration =
@"ball 0.015
target 0.5 0.06 0.04
start 0.055 0.95  0.945 0.95

polygon 0.0 0.0 0.0 0.01 1.0 0.01 1.0 0.0 
polygon 0.0 0.0 0.01 0.0 0.01 1.0 0.0 1.0 
polygon 0.0 1.0 0.0 0.99 1.0 0.99 1.0 1.0 
polygon 1.0 1.0 0.99 1.0 0.99 0.0 1.0 0.0 
polygon 0.034 0.852 0.106 0.708 0.33199999999999996 0.674 0.17599999999999996 0.618 0.028 0.718 
polygon 0.15 0.7559999999999999 0.142 0.93 0.232 0.894 0.238 0.99 0.498 0.722 
polygon 0.8079999999999999 0.91 0.904 0.784 0.7799999999999999 0.572 0.942 0.562 0.952 0.82 0.874 0.934 
polygon 0.768 0.814 0.692 0.548 0.594 0.47 0.606 0.804 0.648 0.626 
polygon 0.22799999999999998 0.5760000000000001 0.39 0.322 0.3400000000000001 0.31400000000000006 0.184 0.456 
polygon 0.09 0.228 0.242 0.076 0.106 0.03 0.022 0.178 
polygon 0.11 0.278 0.24600000000000002 0.262 0.108 0.454 0.16 0.566 0.064 0.626 0.016 0.438 
polygon 0.772 0.1 0.71 0.20599999999999996 0.77 0.322 0.894 0.09600000000000002 0.8039999999999999 0.17600000000000002 
polygon 0.698 0.476 0.984 0.27199999999999996 0.908 0.512 
polygon 0.45 0.39199999999999996 0.614 0.25799999999999995 0.7340000000000001 0.438 
polygon 0.476 0.868 0.552 0.8119999999999999 0.62 0.902 0.626 0.972 0.49 0.958 
polygon 0.61 0.014000000000000002 0.58 0.094 0.774 0.05000000000000001 0.63 0.054000000000000006 
polygon 0.33399999999999996 0.014 0.27799999999999997 0.03799999999999998 0.368 0.254 0.7 0.20000000000000004 0.764 0.108 0.526 0.158 
polygon 0.294 0.584 0.478 0.626 0.482 0.574 0.324 0.434 0.35 0.39 0.572 0.52 0.588 0.722 0.456 0.668 
";
    }
}
