using System.Drawing;
using System.Linq;
using Environments.ContinuousStateDiscreteDecision;

namespace Presenters
{
    public class CartPoleSwingUpPresenter : Presenter
    {
        public CartPoleSwingUpPresenter(CartPoleSwingUp environment)
            : base(-3.0, -1.2, 3.0, 3.6)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            double[] state = this.environment.GetCurrentState().StateVector.ToArray();

            Graphics.Clear(Color.WhiteSmoke);
            
            double middleOfCartX = state[0];
            double middleOfCartY = 0.3;
            double cartLength = 0.5;

            this.DrawLine(Pens.Black, middleOfCartX - cartLength, middleOfCartY, middleOfCartX + cartLength, middleOfCartY);
            this.DrawLine(Pens.Black, middleOfCartX, middleOfCartY, middleOfCartX + state[2], middleOfCartY + state[3]);
            
            this.DrawLine(Pens.Gray, -2.4 - cartLength, -1, -2.4 - cartLength, 3);
            this.DrawLine(Pens.Gray, 2.4 + cartLength, -1, 2.4 + cartLength, 3);
        }

        private CartPoleSwingUp environment;
    }
}
