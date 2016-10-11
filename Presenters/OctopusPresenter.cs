using System.Drawing;
using Environments.ContinuousStateContinuousDecision;
using Environments.Infrastructure.OctopusInfrastructure;

namespace Presenters
{
    public class OctopusPresenter : Presenter
    {
        public OctopusPresenter(Octopus environment) : base(-2, -6, 10, 6)
        {
            this.environment = environment;
        }

        public override void Draw()
        {
            Graphics.Clear(System.Drawing.Color.WhiteSmoke);
            Node node1 = (environment.Arm.UpperNodes[0]);
            Node node2 = (environment.Arm.LowerNodes[0]);
            this.DrawLine(Pens.Chocolate, node1.Position.PositionX, node1.Position.PositionY, node2.Position.PositionX, node2.Position.PositionY);
            for (int i = 0; i < environment.Arm.UpperNodes.Length && i < environment.Arm.LowerNodes.Length; i++)
            {
                Node node3 = environment.Arm.UpperNodes[i];
                Node node4 = environment.Arm.LowerNodes[i];
                this.DrawLine(Pens.Chocolate, node1.Position.PositionX, node1.Position.PositionY, node3.Position.PositionX, node3.Position.PositionY);
                this.DrawLine(Pens.Chocolate, node2.Position.PositionX, node2.Position.PositionY, node4.Position.PositionX, node4.Position.PositionY);
                this.DrawLine(Pens.Chocolate, node3.Position.PositionX, node3.Position.PositionY, node4.Position.PositionX, node4.Position.PositionY);
                node1 = node3;
                node2 = node4;
            }

            if (environment.Mouth != null)
            {
                this.FillCircle(Brushes.Plum, environment.Mouth.PositionX, environment.Mouth.PositionY, 0.4);
            }

            foreach (Food f in environment.Food)
            {
                this.FillCircle(Brushes.Tomato, f.Position.PositionX, f.Position.PositionY, 0.3);
            }

            foreach (Target t in environment.Targets)
            {
                this.FillCircle(Brushes.DarkSalmon, t.Position.PositionX, t.Position.PositionY, 0.3);
            }
        }

        private Octopus environment;
    }
}
