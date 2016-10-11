using System.Drawing;
using System.Linq;
using Environments.ContinuousStateContinuousDecision;

namespace Presenters
{
    public class TripleInvertedPendulumPresenter : Presenter
    {
        public TripleInvertedPendulumPresenter(TripleInvertedPendulum environment)
            : base(-1, -0.5, 1, 1.5)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            Graphics.Clear(Color.WhiteSmoke);
            double[] positions = environment.GetPositions().ToArray();

            MoveTo(positions[0], positions[1]);
            DrawLine(Pens.Black, positions[2], positions[3]);
            DrawLine(Pens.Black, positions[4], positions[5]);
            DrawLine(Pens.Black, positions[6], positions[7]);
        }

        private TripleInvertedPendulum environment;
    }
}
