using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Environments;

namespace Presenters
{
    public class BasicPresenter<TStateSpaceType, TActionSpaceType> : Presenter
        where TStateSpaceType : struct
        where TActionSpaceType : struct
    {
        public BasicPresenter(Environment<TStateSpaceType, TActionSpaceType> environment)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            const int MaxStates = 18;
            var currentState = environment.GetCurrentState();

            Font font = new System.Drawing.Font("Helvetica", 10, FontStyle.Italic);
            Brush brush = new SolidBrush(System.Drawing.Color.Red);

            if (currentState == null)
            {
                Graphics.DrawString("?", font, brush, 20, 26);
                return;
            }

            double[] state = environment.GetCurrentState().StateVector.Select(v => Convert.ToDouble(v, CultureInfo.InvariantCulture)).ToArray();
            Graphics.Clear(System.Drawing.Color.LightGray);
            for (int i = 0; i < MaxStates && i < state.Length; i++)
            {
                Graphics.DrawString("State[" + (i + 1) + "/" + (state.Length > MaxStates ? MaxStates : state.Length) + "]: " + state[i], font, brush, 20, 13 * (i + 2));
            }
        }

        private Environment<TStateSpaceType, TActionSpaceType> environment;
    }
}
