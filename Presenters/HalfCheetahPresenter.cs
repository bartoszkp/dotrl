using System.Drawing;
using System.Linq;
using Environments.ContinuousStateContinuousDecision;

namespace Presenters
{
    public class HalfCheetahPresenter : Presenter
    {
        public HalfCheetahPresenter(HalfCheetah environment)
            : base(0, -2, 5, 4)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            double[] state = environment.GetJointPositions().ToArray();

            Graphics.Clear(System.Drawing.Color.WhiteSmoke);
            
            for (int i = 2; i + 1 < state.Length; i += 2)
            {
                this.DrawLine(Pens.Purple, state[i - 2], state[i - 1], state[i], state[i + 1]);
                if (i + 3 < state.Length)
                {
                    this.FillCircle(Brushes.DarkRed, state[i], state[i + 1], 0.022);
                }
            }

            this.DrawLine(Pens.Black, -20, -0.05, 25, -0.05);
        }

        private HalfCheetah environment;
    }
}
