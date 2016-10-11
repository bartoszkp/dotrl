using System.Drawing;
using System.Linq;
using Environments.ContinuousStateDiscreteDecision;

namespace Presenters
{
    public class AcrobotPresenter : Presenter
    {
        public AcrobotPresenter(Acrobot environment)
            : base(-2, -2, 2, 2)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            this.Graphics.Clear(Color.WhiteSmoke);
            double[] state = this.environment.GetCurrentState().StateVector.ToArray();

            double sin1 = state[0];
            double cos1 = state[1];
            double sin12 = sin1 * state[4] + cos1 * state[3];
            double cos12 = cos1 * state[4] - sin1 * state[3];
            double r = 1;

            MoveTo(0, 0);
            DrawLine(Pens.Black, (r * (sin1)), (r * cos1));
            DrawLine(Pens.Black, (r * (sin1 + sin12)), (r * (cos1 + cos12)));
        }

        private Acrobot environment;
    }
}
