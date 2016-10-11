using System.Drawing;
using System.Linq;
using Environments.ContinuousStateContinuousDecision;

namespace Presenters
{
    public class RobotWeightliftingPresenter : Presenter
    {
        public RobotWeightliftingPresenter(RobotWeightlifting environment)
            : base(-4, -4, 4, 4)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            Graphics.Clear(Color.WhiteSmoke);

            double[] visualState = this.environment.GetJoints().ToArray();

            double x1, y1, x2, y2, x3, y3;
            x1 = visualState[0];
            y1 = visualState[1];
            x2 = visualState[2];
            y2 = visualState[3];
            x3 = visualState[4];
            y3 = visualState[5];

            DrawLine(Pens.Black, 0, 0, x1, (y1));
            DrawLine(Pens.Black, x2, (y2));
            DrawLine(Pens.Black, x3, (y3));

            DrawLine(Pens.Black, -(1.5), -3, -(1.5), 0);
            DrawLine(Pens.Black, -(1.5), 0, 0, 0);
            DrawLine(Pens.Black, -(1.5), -1, -(0.5), 0);
        }

        private RobotWeightlifting environment;
    }
}
