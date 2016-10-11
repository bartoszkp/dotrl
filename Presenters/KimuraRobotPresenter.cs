using System.Drawing;
using System.Linq;
using Environments.ContinuousStateDiscreteDecision;

namespace Presenters
{
    public class KimuraRobotPresenter : Presenter
    {
        public KimuraRobotPresenter(KimurasRobot robotModel)
            : base(-60, -25, 40, 75)
        {
            this.robotModel = robotModel;
        }

        public override void Draw()
        {
            this.Graphics.Clear(Color.WhiteSmoke);
            DrawLine(Pens.Black, -50, -0, -50, 40);
            DrawLine(Pens.Black, -50, 40, -10, 40);
            DrawLine(Pens.Black, -10, 40, -10, 0);
            DrawLine(Pens.Black, -10, 0, -50, 0);

            DrawLine(Pens.Black, -10, 0, -50, 0);

            MoveTo(this.robotModel.MBodyX.Last(), this.robotModel.MBodyY.Last());
            for (int i = 0; i < 4; ++i)
            {
                DrawLine(Pens.Black, this.robotModel.MBodyX[i], this.robotModel.MBodyY[i]);
            }

            MoveTo(this.robotModel.MArmX.First(), this.robotModel.MArmY.First());
            for (int i = 0; i < 3; ++i)
            {
                DrawLine(Pens.Black, this.robotModel.MArmX[i], this.robotModel.MArmY[i]);
            }
        }

        private KimurasRobot robotModel;
    }
}
