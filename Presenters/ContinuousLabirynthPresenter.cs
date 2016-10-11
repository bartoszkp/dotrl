using System.Linq;
using System.Drawing;

namespace Presenters
{
    public class ContinuousLabirynthPresenter : Presenter
    {
        public ContinuousLabirynthPresenter(Environments.ContinuousStateDiscreteDecision.ContinuousLabyrinth environment)
            :base(0, 0, 110, 110)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            int x = (int)(10 * this.environment.GetCurrentState().StateVector.ElementAt(0));
            int y = (int)(10 * this.environment.GetCurrentState().StateVector.ElementAt(1));

            Graphics.Clear(System.Drawing.Color.Black);
            for (int i = 0; i <= 10; ++i)
            {
                DrawLine(Pens.White, i * 10 + 5, 5, i * 10 + 5, 105);
                DrawLine(Pens.White, 5, i * 10 + 5, 105, i * 10 + 5);
            }

            FillRectangle(Brushes.Green, 85, 85, 10, 10);
            FillRectangle(Brushes.Red, x - 3, y - 3, 3, 3);
        }

        private Environments.ContinuousStateDiscreteDecision.ContinuousLabyrinth environment;
    }
}
