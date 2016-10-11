using System;
using System.Linq;
using System.Collections.Generic;

namespace Environments.ContinuousStateDiscreteDecision
{
    public class ContinuousLabyrinth : Environments.Environment<double, int>
    {
        public enum Action
        {
            Left = 0,
            Up = 1,
            Right = 2,
            Down = 3
        };

        public ContinuousLabyrinth()
        {
            this.CurrentState = new Core.MutableState<double>(9);
        }

        public override Core.EnvironmentDescription<double, int> GetEnvironmentDescription()
        {
            var spaceDesc = new Core.SpaceDescription<double>(
                new[] { 0.0, 0.0, -1.0, -1.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
                new[] { 10.0, 10.0, 1.0, 1.0, 20.0, 10.0, 100.0, 3.17, 3.17 });
            var actionDesc = Core.SpaceDescription<int>.CreateOneDimensionalSpaceDescription(0, 3);
            var rewardDesc = new Core.DimensionDescription<double>(-5, 10);
            return new Core.EnvironmentDescription<double, int>(spaceDesc, actionDesc, rewardDesc, 0.9);
        }

        public override Core.Reinforcement PerformAction(Core.Action<int> action)
        {
            switch (action.SingleValue)
            {
                case (int)Action.Left:
                    Left();
                    break;
                case (int)Action.Up:
                    Up();
                    break;
                case (int)Action.Right:
                    Right();
                    break;
                case (int)Action.Down:
                    Down();
                    break;
                default:
                    throw new InvalidOperationException();
            }

            bool checkRightBarrier = x < 3 && y >= 1 && y < 8;
            bool checkLeftBarrier = x > 3 && y >= 1 && y < 8;
            bool checkUpBarrier = y < 8 && x >= 1 && x < 3;
            bool checkDownBarrier = y > 8 && x >= 1 && x < 3;

            x += vx;
            y += vy;

            if ((checkRightBarrier && x >= 3)
                || (checkLeftBarrier && x <= 3)
                || (checkUpBarrier && y >= 8)
                || (checkDownBarrier && y <= 8)
                || x < 0
                || x > 10
                || y < 0
                || y > 10)
            {
                StartEpisodeInDefinedState(CurrentState.StateVector[0], CurrentState.StateVector[1], 0, 0);
                return -5;
            }

            vx /= 3;
            vy /= 3;

            RectifyState();

            UpdateCurrentState();

            if (this.CurrentState.IsTerminal)
            {
                return 10;
            }

            return -1;
        }

        private void RectifyState()
        {
            if (double.IsNaN(x))
                x = 0;
            if (double.IsNaN(y))
                y = 0;
            if (double.IsNaN(vx))
                vx = 0;
            if (double.IsNaN(vy))
                vy = 0;

            x = Math.Max(0, x);
            y = Math.Max(0, y);
            x = Math.Min(10, x);
            y = Math.Min(10, y);

            vx = Math.Max(-1, vx);
            vy = Math.Max(-1, vy);
            vx = Math.Min(1, vx);
            vy = Math.Min(1, vy);
        }

        private void UpdateCurrentState()
        {
            this.CurrentState[0] = x;
            this.CurrentState[1] = y;
            this.CurrentState[2] = vx;
            this.CurrentState[3] = vy;
            this.CurrentState[4] = x + y;
            this.CurrentState[5] = Math.Abs(x - y);
            this.CurrentState[6] = x * y;
            this.CurrentState[7] = Math.Sqrt(x);
            this.CurrentState[8] = Math.Sqrt(y);

            this.CurrentState.IsTerminal = Equal(x, 8) && Equal(y, 8);
        }

        public void StartEpisodeInPartiallyDefinedState(double[] partialState, params int[] dimensions)
        {
            partialState = partialState.Select((v, i) => (v < 0 && i != 2 && i != 3) ? 0.0 : v).ToArray();

            double x = DeduceX(partialState, dimensions);
            double y = DeduceYWithX(partialState, x, dimensions);
            double vx = dimensions.Contains(2) ? partialState[2] : (2 * r.NextDouble() - 1);
            double vy = dimensions.Contains(3) ? partialState[3] : (2 * r.NextDouble() - 1);

            StartEpisodeInDefinedState(x, y, vx, vy);
        }

        private double DeduceX(double[] partialState, params int[] dimensions)
        {
            if (dimensions.Contains(0))
            {
                return partialState[0];
            }
            else if (dimensions.Contains(7))
            {
                return partialState[7] * partialState[7];
            }
            else if (dimensions.Contains(1) || dimensions.Contains(8))
            {
                double y = dimensions.Contains(1) ? partialState[1] : (partialState[8] * partialState[8]);

                if (dimensions.Contains(4) || dimensions.Contains(5) || (dimensions.Contains(6) && y > 0))
                {
                    return DeduceXWithY(partialState, y, dimensions);
                }
            }
            else if (dimensions.Contains(4) && dimensions.Contains(6))
            {
                double delta = partialState[4] * partialState[4] - 4 * partialState[6];

                delta = Math.Max(0, delta);

                return (partialState[4] + Math.Sqrt(delta)) / 2;
            }
            else if (dimensions.Contains(4) && dimensions.Contains(5))
            {
                var x1 = (partialState[4] + partialState[5]) / 2;
                var x2 = (partialState[4] - partialState[5]) / 2;

                if (x1 < 0 || x1 > 10)
                {
                    return x2;
                }
                else if (x2 < 0 || x2 > 10)
                {
                    return x1;
                }

                return new[] {x1, x2}[r.Next(2)];
            }
            else if (dimensions.Contains(5) && dimensions.Contains(6))
            {
                double delta = partialState[5] * partialState[5] + 4 * partialState[6];

                delta = Math.Max(0, delta);

                return (partialState[5] + Math.Sqrt(delta)) / 2;
            }
            else if (dimensions.Contains(4))
            {
                if (partialState[4] >= 10)
                {
                    double minimumX = partialState[4] - 10;
                    return r.NextDouble() * (10 - minimumX) + minimumX;
                }
                else
                {
                    return r.NextDouble() * partialState[4];
                }
            }
            else if (dimensions.Contains(5))
            {
                if (r.NextDouble() >= 0.5)
                {
                    double minimumX = partialState[5];
                    return r.NextDouble() * (10 - minimumX) + minimumX;
                }
                else
                {
                    double maximumX = 10 - partialState[5];
                    return r.NextDouble() * maximumX;
                }
            }
            else if (dimensions.Contains(6))
            {
                if (partialState[6] == 0 && r.Next(2) > 0)
                {
                    return 0;
                }
                else
                {
                    double minimumX = partialState[6] / 10;
                    return r.NextDouble() * (10 - minimumX) + minimumX;
                }
            }

            return r.NextDouble() * 10;
        }

        private double DeduceXWithY(double[] partialState, double y, params int[] dimensions)
        {
            if (dimensions.Contains(4))
            {
                return partialState[4] - y;
            }
            else if (dimensions.Contains(6) && y > 0)
            {
                return partialState[6] / y;
            }
            else if (dimensions.Contains(5))
            {
                var x1 = partialState[5] + y;
                var x2 = y - partialState[5];

                if (x1 < 0 || x1 > 10)
                {
                    return x2;
                }
                else if (x2 < 0 || x2 > 10)
                {
                    return x1;
                }

                return new[] { x1, x2 }[r.Next(2)];
            }

            throw new InvalidOperationException();
        }

        private double DeduceYWithX(double[] partialState, double x, params int[] dimensions)
        {
            if (dimensions.Contains(1))
            {
                return partialState[1];
            }
            else if (dimensions.Contains(8))
            {
                return partialState[8] * partialState[8];
            }
            else if (dimensions.Contains(4))
            {
                return partialState[4] - x;
            }
            else if (dimensions.Contains(6) && x > 0)
            {
                return partialState[6] / x;
            }
            else if (dimensions.Contains(5))
            {
                var y1 = partialState[5] + x;
                var y2 = x - partialState[5];

                if (y1 < 0 || y1 > 10)
                {
                    return y2;
                }
                else if (y2 < 0 || y2 > 10)
                {
                    return y1;
                }

                return new[] { y1, y2 }[r.Next(2)];
            }

            return r.NextDouble() * 10;
        }

        private void Left()
        {
            vx -= 0.1;
        }

        private void Up()
        {
            vy += 0.1;
        }

        private void Right()
        {
            vx += 0.1;
        }

        private void Down()
        {
            vy -= 0.1;
        }

        private bool Equal(double v1, double v2)
        {
            return Math.Abs(v1 - v2) < 0.01;
        }

        public override void StartEpisode()
        {
            this.x = r.NextDouble() * 10;
            this.y = r.NextDouble() * 10;
            this.vx = 2 * r.NextDouble() - 1;
            this.vy = 2 * r.NextDouble() - 1;

            UpdateCurrentState();
        }

        public void StartEpisodeInDefinedState(double[] state)
        {
            StartEpisodeInDefinedState(state[0], state[1], state[2], state[3]);
        }

        public void StartEpisodeInDefinedState(double x, double y, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;

            RectifyState();
            UpdateCurrentState();
        }

        public IEnumerable<int> GetFixedDimensions(int[] dimensions)
        {
            if (ContainsX(dimensions) || (ContainsY(dimensions) && ContainsCompound(dimensions)))
            {
                yield return 0;
            }

            if (ContainsY(dimensions) || (ContainsX(dimensions) && ContainsCompound(dimensions)))
            {
                yield return 1;
            }

            if (dimensions.Contains(2))
            {
                yield return 2;
            }

            if (dimensions.Contains(3)) 
            {
                yield return 3;
            }

            if (dimensions.Contains(4))
            {
                yield return 4;
            }

            if (dimensions.Contains(5))
            {
                yield return 5;
            }

            if (dimensions.Contains(6))
            {
                yield return 6;
            }
        }

        private bool ContainsX(int[] dimensions)
        {
            return dimensions.Contains(0) || dimensions.Contains(7);
        }

        private bool ContainsY(int[] dimensions)
        {
            return dimensions.Contains(1) || dimensions.Contains(8);
        }

        private bool ContainsCompound(int[] dimensions)
        {
            return dimensions.Contains(4) || dimensions.Contains(5) || dimensions.Contains(6);
        }

        public void StartEpisodeInStateCloseTo(double[] state, int[] fix, double[] epsilon)
        {
            StartEpisode();

            if (fix.Contains(0))
            {
                this.x = Disturb(state[0], epsilon[0]);
            }
            if (fix.Contains(1))
            {
                this.y = Disturb(state[1], epsilon[1]);
            }
            if (fix.Contains(2))
            {
                this.vx = Disturb(state[2], epsilon[2]);
            }
            if (fix.Contains(3))
            {
                this.vy = Disturb(state[3], epsilon[3]);
            }

            RectifyState();

            if (!fix.Contains(0) && fix.Contains(4))
            {
                if (fix.Contains(5))
                {
                    var x1 = (state[4] + state[5]) / 2;
                    var x2 = (state[4] - state[5]) / 2;

                    if (x1 < 0 || x1 > 10)
                    {
                        this.x = x2;
                    }
                    else if (x2 < 0 || x2 > 10)
                    {
                        this.x = x1;
                    }
                    else
                    {
                        this.x = new[] { x1, x2 }[r.Next(2)];
                    }
                }
                else if (fix.Contains(6))
                {
                    double delta = state[4] * state[4] - 4 * state[6];

                    delta = Math.Max(0, delta);

                    this.x = (state[4] + Math.Sqrt(delta)) / 2;
                }
                else if (state[4] >= 10)
                {
                    double minimumX = state[4] - 10;
                    this.x = r.NextDouble() * (10 - minimumX) + minimumX;
                }
                else
                {
                    this.x = r.NextDouble() * state[4];
                }

                this.y = state[4] - this.x;
            }
            else if (!fix.Contains(0) && fix.Contains(5))
            {
                if (fix.Contains(6))
                {
                    double delta = state[5] * state[5] + 4 * state[6];

                    delta = Math.Max(0, delta);

                    this.x = (state[5] + Math.Sqrt(delta)) / 2;
                }
                else if (r.NextDouble() >= 0.5)
                {
                    double minimumX = state[5];
                    this.x = r.NextDouble() * (10 - minimumX) + minimumX;
                }
                else
                {
                    double maximumX = 10 - state[5];
                    this.x = r.NextDouble() * maximumX;
                }

                var y1 = state[5] + x;
                var y2 = x - state[5];

                if (y1 < 0 || y1 > 10)
                {
                    this.y = y2;
                }
                else if (y2 < 0 || y2 > 10)
                {
                    this.y = y1;
                }
                else
                {
                    this.y = new[] { y1, y2 }[r.Next(2)];
                }
            }
            else if (!fix.Contains(0) && fix.Contains(6))
            {
                if (state[6] == 0 && r.Next(2) > 0)
                {
                    this.x = 0;
                }
                else
                {
                    double minimumX = state[6] / 10;
                    this.x = r.NextDouble() * (10 - minimumX) + minimumX;
                    this.y = state[6] / this.x;
                }
            }

            RectifyState();
            UpdateCurrentState();
        }

        private double Disturb(double v, double e)
        {
            return v + (2 * r.NextDouble() - 1) * e;
        }

        private double x;
        private double y;
        private double vx;
        private double vy;
        private System.Random r = new Random();
    }
}
