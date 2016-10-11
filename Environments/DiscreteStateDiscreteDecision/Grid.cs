using System.Linq;
using Core;
using Core.Parameters;

namespace Environments.DiscreteStateDiscreteDecision
{
    public class Grid : Environment<int, int>
    {
        [Parameter]
        private GridTypeType GridType { get; set; }
        [Parameter(0, 1)]
        public double DiscountFactor { get; private set; }

        public int PositionX { get; private set; }

        public int PositionY { get; private set; }

        public int Width
        { 
            get 
            { 
                return this.grid[1].Length; 
            }
        }

        public int Height 
        { 
            get 
            {
                return this.grid.Length; 
            }
        }

        public int[] this[int row] 
        { 
            get 
            { 
                return this.grid[row]; 
            }
        }

        public Grid()
        {
            DiscountFactor = 0.95;
            GridType = Grid.GridTypeType.Simple;
            this.random = new System.Random();
            this.CurrentState = new MutableState<int>(1);
        }

        public override void StartEpisode()
        {
            if (this.startX >= 0 && this.startY >= 0)
            {
                this.PositionX = this.startX;
                this.PositionY = this.startY;
            }
            else
            {
                int xPos, yPos;
                do
                {
                    xPos = this.random.Next(this.Width - 1);
                    yPos = this.random.Next(this.Height - 1);
                }
                while (this.grid[yPos][xPos] != 0);

                this.PositionX = xPos;
                this.PositionY = yPos;
            }

            UpdateCurrentState();
        }

        public override Reinforcement PerformAction(Action<int> action)
        {
            int newX = PositionX;
            int newY = PositionY;

            switch (action.ActionVector.First())
            {
                case 0:
                    ++newX;
                    break;
                case 1:
                    ++newY;
                    break;
                case 2:
                    --newX;
                    break;
                case 3:
                    --newY;
                    break;
            }

            if (newX < 0
                || newX >= this.Width
                || newY < 0
                || newY >= this.Height
                || this[newY][newX] == 1)
            {
                return -1;
            }

            PositionX = newX;
            PositionY = newY;

            UpdateCurrentState();

            if (this[PositionY][PositionX] == 2)
            {
                return 0;
            }

            return -1;
        }

        public override EnvironmentDescription<int, int> GetEnvironmentDescription()
        {
            SpaceDescription<int> stateSpaceDescription = new SpaceDescription<int>(new[] { 0 }, new[] { (this.Height * this.Width) - 1 });
            SpaceDescription<int> actionSpaceDescription = new SpaceDescription<int>(new[] { 0 }, new[] { 3 });
            DimensionDescription<double> reinforcementSpaceDescription = new DimensionDescription<double>(-1, 10);

            return new EnvironmentDescription<int, int>(stateSpaceDescription, actionSpaceDescription, reinforcementSpaceDescription, DiscountFactor);
        }

        public override void ParametersChanged()
        {
            RebuildGrid();
        }

        private void UpdateCurrentState()
        {
            this.CurrentState.SingleValue = this.PositionY * this.Width + this.PositionX;
            this.CurrentState.IsTerminal = this[this.PositionY][this.PositionX] == 2;
        }

        private void RebuildGrid()
        {
            switch (this.GridType)
            {
                case GridTypeType.Simple:
                    this.grid = new int[][] 
                    {
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 1, 1, 1, 1, 1, 1, 0 }, 
                        new int[] { 0, 1, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 1, 2, 1, 1, 1, 1, 1 }, 
                        new int[] { 0, 1, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 1, 1, 1, 1, 1, 1, 0 }, 
                        new int[] { 0, 1, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 1, 0, 1, 0, 1, 0, 1 }, 
                        new int[] { 0, 1, 0, 1, 0, 1, 0, 0 }, 
                        new int[] { 0, 1, 0, 1, 0, 1, 1, 0 }, 
                        new int[] { 0, 0, 0, 1, 0, 0, 0, 0 } 
                    };
                    this.startX = -1;
                    this.startY = -1;
                    break;
                case GridTypeType.CichoszTtdRbtd:
                    this.grid = new int[][]
                    {
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0 }, 
                        new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0 }, 
                        new int[] { 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1 }, 
                        new int[] { 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 1 }, 
                        new int[] { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, 
                        new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0 }, 
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    this.startX = 1;
                    this.startY = 18;
                    break;
                case GridTypeType.CichoszPhd:
                    this.grid = new int[][] 
                    {
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 1, 1, 1, 0, 0, 1, 1, 1, 0 }, 
                        new int[] { 0, 0, 0, 1, 0, 0, 1, 2, 0, 0 }, 
                        new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 }, 
                        new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 }, 
                        new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 }, 
                        new int[] { 0, 1, 1, 1, 0, 0, 1, 1, 1, 0 }, 
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                    };
                    this.startX = 2;
                    this.startY = 6;
                    break;
                default:
                    throw new System.InvalidOperationException("Unexpected grid type");
            }
        }

        private enum GridTypeType
        {
            Simple,
            CichoszTtdRbtd,
            CichoszPhd
        }

        private System.Random random;
        private int[][] grid;
        private int startX;
        private int startY;
    }
}
