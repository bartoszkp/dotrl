// Ported from Java version available at: http://www.cs.mcgill.ca/~dprecup/workshops/ICML06/octopus.html

using System.Collections.Generic;
using System.Linq;
using BackwardCompatibility;
using BackwardCompatibility.ODEFramework;
using Core;
using Core.Parameters;
using Environments.Infrastructure.OctopusInfrastructure;

namespace Environments.ContinuousStateContinuousDecision
{
    public class Octopus : Environment<double, double>
    {
        [Parameter(StringParameterType.Multiline, "Inner octopus simulator XML configuration")]
        private string configuration = Octopus.defaultConfiguration;

        public Arm Arm { get; set; }

        public IEnumerable<Target> Targets { get; set; }

        public HashSet<Food> Food { get; private set; }

        public Mouth Mouth { get; set; }

        public bool Terminal
        {
            get
            {
                return taskTracker.Terminal;
            }
        }

        public Octopus()
        {
            CurrentState = new MutableState<double>(12);
        }

        public override void StartEpisode()
        {
            this.envSimulator.SetODEState(0.0, initialState);
            taskTracker.Reset();
            this.UpdateCurrentState();
        }

        public override EnvironmentDescription<double, double> GetEnvironmentDescription()
        {
            double[] averageState = Enumerable.Repeat(0.0, 12).ToArray();
            double[] stdDevState = new double[12];

            averageState[0] = 10;
            stdDevState[0] = 3;
            stdDevState[1] = 1;
            stdDevState[2] = 0.01;
            stdDevState[3] = 0.01;
            int i = 0;
            for (i = 0; i < 3; i++)
            {
                stdDevState[i * 2 + 4] = 3; // r
                stdDevState[i * 2 + 5] = 1; // cos(alpha)
            }

            stdDevState[10] = stdDevState[11] = 1;

            double[] minAction = Enumerable.Repeat(0.0, this.Arm.Compartments.Length * 3).ToArray();
            double[] maxAction = Enumerable.Repeat(1.0, this.Arm.Compartments.Length * 3).ToArray();
            SpaceDescription<double> actionsDescription = new SpaceDescription<double>(minAction, maxAction, null, null);
            SpaceDescription<double> statesDescription = new SpaceDescription<double>(null, null, averageState, stdDevState);
            DimensionDescription<double> rewardDescription = new DimensionDescription<double>(-100, 100);

            return new EnvironmentDescription<double, double>(statesDescription, actionsDescription, rewardDescription, 0.9);
        }

        public override Reinforcement PerformAction(Action<double> action)
        {
            if (Terminal)
            {
                return 0;
            }

            int usedCompartments = System.Math.Min(action.Dimensionality / 3, Arm.Compartments.Length);
            for (int i = 0; i < usedCompartments; i++)
            {
                Arm.Compartments[i].SetAction(action[3 * i], action[3 * i + 1], action[3 * i + 2]);
            }

            double[] stateCopy = this.GetCurrentState().StateVector.ToArray();

            try
            {
                ODEState odeState = envSimulator.CurrentODEState;
                odeState = solver.Solve(envSimulator, odeState, 0, 5, .2);
                envSimulator.SetODEState(.1, odeState);

                taskTracker.Update();

                this.UpdateCurrentState();
            }
            catch (System.ArithmeticException)
            {
                this.CurrentState.StateVector = stateCopy;
                this.CurrentState.IsTerminal = true;
                return -10;
            }

            return taskTracker.Reward;
        }

        private void UpdateCurrentState()
        {
            int[] idx = new int[3];
            idx[0] = 1;
            idx[1] = Arm.Compartments.Length * 4 / 10;
            idx[2] = Arm.Compartments.Length - 1;

            int i = 0;

            Vector2D target = Targets.First().Position;
            for (i = 0; i < 3; i++)
            {
                IEnumerable<Node> nodes = Enumerable.Concat(Arm.UpperNodes.Skip(idx[i]), Arm.LowerNodes.Skip(idx[i]));
                double totalMass = nodes.Sum(v => v.Mass);

                double avx = nodes.Sum(v => v.Position.PositionX * v.Mass) / totalMass;
                double avy = nodes.Sum(v => v.Position.PositionY * v.Mass) / totalMass;

                Vector2D point = new Vector2D(avx, avy);

                CurrentState[i * 2 + 4] = point.Norm - target.Norm;
                CurrentState[i * 2 + 5] = target.AngleTo(point);

                if (i == 0)
                {
                    CurrentState[0] = target.Norm;
                    CurrentState[1] = (new Vector2D(1, 0)).AngleTo(target);
                    Vector2D vel = new Vector2D(
                        nodes.Sum(v => v.Velocity.PositionX) / totalMass,
                        nodes.Sum(v => v.Velocity.PositionY) / totalMass);
                    CurrentState[2] = vel.Norm < 1e-7 ? 0 : vel.Norm * System.Math.Cos(point.AngleTo(vel));
                    CurrentState[3] = vel.Norm < 1e-7 ? 0 : vel.Norm * System.Math.Sin(point.AngleTo(vel));
                }
            }

            CurrentState[10] = System.Math.Sign(CurrentState[8]);
            CurrentState[11] = System.Math.Sign(CurrentState[9]);

            CurrentState.IsTerminal = Terminal;
        }

        public override void ParametersChanged()
        {
            using (var configurationReader = new System.IO.StringReader(configuration))
            {
                System.Xml.Serialization.XmlSerializer serializer
                  = new System.Xml.Serialization.XmlSerializer(typeof(Config));
                Configure((Config)serializer.Deserialize(configurationReader));
            }
        }
  
        internal void Configure(Config configuration)
        {
            Arm = new Arm(configuration.Constants, configuration.Environment.Arm);
            Targets = Enumerable.Empty<Target>();
            Food = new HashSet<Food>();
            Mouth = null;

            taskTracker = MakeTaskTracker(configuration.Environment.Task);
            envSimulator = new EnvironmentSimulator(configuration.Constants, Arm, Food);

            initialState = envSimulator.CurrentODEState;
            solver = new RungeKutta4Solver();
        }

        private ITaskTracker MakeTaskTracker(TaskDef def)
        {
            FoodTaskDef foodTaskDef = def as FoodTaskDef;
            if (foodTaskDef != null)
            {
                return new FoodTaskTracker(this, foodTaskDef);
            }
            else
            {
                TargetTaskDef targetTaskDef = def as TargetTaskDef;
                if (targetTaskDef != null)
                {
                    return new TargetTaskTracker(this, targetTaskDef);
                }
                else
                {
                    PointTaskDef pointTaskDef = def as PointTaskDef;
                    if (pointTaskDef != null)
                    {
                        return new PointTaskTracker(this, pointTaskDef);
                    }
                    else
                    {
                        throw new System.ArgumentException("Unknown task definition given.");
                    }
                }
            }
        }
                
        private ITaskTracker taskTracker;
        private EnvironmentSimulator envSimulator;
        private ODESolver solver;
        private ODEState initialState;

        private static string defaultConfiguration = " <config>\r\n" +
" <constants>\r\n" +
" <!-- On a real octopus, tangential friction is about 50 times less than perpendicular friction -->\r\n" +
" <frictionTangential>0.2</frictionTangential>\r\n" +
" <frictionPerpendicular>1</frictionPerpendicular>\r\n" +
" <pressure>10</pressure>\r\n" +
" <gravity>0.01</gravity>\r\n" +
" <surfaceLevel>12</surfaceLevel>\r\n" +
" <buoyancy>0.08</buoyancy>\r\n" +
" <muscleActive>0.1</muscleActive>\r\n" +
" <musclePassive>0.05</musclePassive>\r\n" +
" <muscleNormalizedMinLength>0.4</muscleNormalizedMinLength>\r\n" +
" <muscleDamping>-0.3</muscleDamping>\r\n" +
" <!-- the values 0.04 and 2.3 produce good behaviour for a 40-compartment arm -->\r\n" +
" <repulsionConstant>.04</repulsionConstant> \r\n" +
" <repulsionPower>2.3</repulsionPower>\r\n" +
" <repulsionThreshold>.7</repulsionThreshold>\r\n" +
" </constants>\r\n" +
"\r\n" +
" <environment>\r\n" +
" <pointTask reward=\"0\" minTargetRadius=\"7.5\" maxTargetRadius=\"9.5\">\r\n" +
" </pointTask>\r\n" +
" <!--\r\n" +
" <foodTask timeLimit=\"1000\" stepReward=\"-0.01\">\r\n" +
" <mouth x=\"5\" y=\"3.5\" width=\"2\" height=\"2\" />\r\n" +
" <food velocity=\"0 0\" position=\"5 3\" mass=\"1\" reward=\"5\" />\r\n" +
" <food velocity=\"0 0\" position=\"6 3\" mass=\"2\" reward=\"7\" />\r\n" +
" </foodTask>\r\n" +
" <targetTask timeLimit=\"10000\" stepReward=\"0\">\r\n" +
" <choice>\r\n" +
" <sequence>\r\n" +
" <target position=\"9 1.5\" reward=\"2\" />\r\n" +
" <target position=\"10.5 1.5\" reward=\"1\" />\r\n" +
" </sequence>\r\n" +
" <target position=\"10 -0.5\" reward=\"3\" />\r\n" +
" </choice>\r\n" +
" </targetTask>\r\n" +
" -->\r\n" +
" <arm>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"0 0.5\" mass=\"1e100\" />\r\n" +
" <lower velocity=\"0 0\" position=\"0 -0.5\" mass=\"1e100\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"1 0.5\" mass=\"0.9900990099\" />\r\n" +
" <lower velocity=\"0 0\" position=\"1 -0.5\" mass=\"0.9900990099\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"2 0.5\" mass=\"0.9803921569\" />\r\n" +
" <lower velocity=\"0 0\" position=\"2 -0.5\" mass=\"0.9803921569\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"3 0.5\" mass=\"0.9708737864\" />\r\n" +
" <lower velocity=\"0 0\" position=\"3 -0.5\" mass=\"0.9708737864\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"4 0.5\" mass=\"0.9615384615\" />\r\n" +
" <lower velocity=\"0 0\" position=\"4 -0.5\" mass=\"0.9615384615\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"5 0.5\" mass=\"0.9523809524\" />\r\n" +
" <lower velocity=\"0 0\" position=\"5 -0.5\" mass=\"0.9523809524\" />\r\n" +
" </nodePair>\r\n" +
"\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"6 0.5\" mass=\"0.8433962264\" />\r\n" +
" <lower velocity=\"0 0\" position=\"6 -0.5\" mass=\"0.8433962264\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"7 0.5\" mass=\"0.8345794393\" />\r\n" +
" <lower velocity=\"0 0\" position=\"7 -0.5\" mass=\"0.8345794393\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"8 0.5\" mass=\"0.8259259259\" />\r\n" +
" <lower velocity=\"0 0\" position=\"8 -0.5\" mass=\"0.8259259259\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"9 0.5\" mass=\"0.8174311927\" />\r\n" +
" <lower velocity=\"0 0\" position=\"9 -0.5\" mass=\"0.8174311927\" />\r\n" +
" </nodePair>\r\n" +
" <nodePair>\r\n" +
" <upper velocity=\"0 0\" position=\"10 0.5\" mass=\"0.7090909091\" />\r\n" +
"\r\n" +
" <lower velocity=\"0 0\" position=\"10 -0.5\" mass=\"0.7090909091\" />\r\n" +
" </nodePair>\r\n" +
" </arm>\r\n" +
" </environment>\r\n" +
" </config>";        
    }
}
