using System.Drawing;
using System.Linq;
using Environments.DiscreteStateDiscreteDecision;

namespace Presenters
{
    public class GridPresenter : Presenter
    {
        public GridPresenter(Grid gridEnvironmentFlat)
            : base(0, -2, gridEnvironmentFlat.Width, gridEnvironmentFlat.Height)
        {
            this.gridEnvironmentFlat = gridEnvironmentFlat;
        }

        public override void Draw()
        {
            Graphics.Clear(Color.Black);
            this.FillRectangle(Brushes.WhiteSmoke, 0, this.gridEnvironmentFlat.Height - 1, this.gridEnvironmentFlat.Width, this.gridEnvironmentFlat.Height);
            for (int yPos = 0; yPos < gridEnvironmentFlat.Height; yPos++)
            {
                for (int xPos = 0; xPos < this.gridEnvironmentFlat.Width; xPos++)
                {
                    int k = this.gridEnvironmentFlat[yPos][xPos];
                    if (k == 1)
                    {
                        this.FillRectangle(Brushes.Black, xPos, yPos, 1, 1);
                    }
                    else if (k == 2)
                    {
                        this.FillRectangle(Brushes.Red, xPos, yPos, 1, 1);
                    }
                }
            }

            this.FillRectangle(Brushes.Blue, this.gridEnvironmentFlat.PositionX, this.gridEnvironmentFlat.PositionY, 1, 1);
        }

        private Grid gridEnvironmentFlat;
    }
}
