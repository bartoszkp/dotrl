using System.Linq;
using BackwardCompatibility;
using Core;
using Core.Parameters;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Agents.ContinuousStateDiscreteDecision
{
    public class ActorCriticAgent : Agent<double, int>
    {
        [Parameter(1, 100)]
        private int actorNetworkSize = 20;
        [Parameter(1, 100)]
        private int valueNetworkSize = 50;
        [Parameter(0, 1)]
        private double aStepSize = 0.1;
        [Parameter(0, 1)]
        private double vStepSize = 0.1;
        [Parameter(0, 2)]
        private double rScalator = 1;
        [Parameter(0, 1)]
        private double discount = 0.95;
        [Parameter(0, 1)]
        private double lambda = 0.5;
        [Parameter(0, null)]
        private double maxTheta = 1;
        [Parameter(0, null)]
        private double overMaxThetaPenalty = 0.03;
        [Parameter(0, 100)]
        private double radius = 1;
 
        public override void ExperimentStarted(EnvironmentDescription<double, int> environmentDescription)
        {
            int actionCount
                = environmentDescription.ActionSpaceDescription.MaximumValues.First()
                - environmentDescription.ActionSpaceDescription.MinimumValues.First()
                + 1;

            this.network = new ANeuralAprx();
            this.sampler = new ASampler();

            this.network.Init(
                this.actorNetworkSize,
                actionCount,
                CellType.Arcustangent,
                environmentDescription.StateSpaceDescription.AverageValues.ToArray(),
                environmentDescription.StateSpaceDescription.StandardDeviations.ToArray());

            this.networkOutput = new double[actionCount];
            this.neuralAction = -1;
            this.prop = new DenseVector(actionCount);
            this.derivativeLnDensity = new DenseVector(actionCount);

            this.valuesNetwork = new ANeuralAprx();

            this.valuesNetwork.Init(
                this.valueNetworkSize,
                1,
                CellType.Arcustangent,
                environmentDescription.StateSpaceDescription.AverageValues.ToArray(),
                environmentDescription.StateSpaceDescription.StandardDeviations.ToArray());

            this.actorDSum = new DenseVector(this.network.GetParamDim());
            this.valueDSum = new DenseVector(this.valuesNetwork.GetParamDim());

            this.Action = new MutableAction<int>(1);

            this.densityDTheta = new DenseVector(this.network.GetParamDim());
            this.dVParam = new DenseVector(this.valuesNetwork.GetParamDim());
        }

        public override Action<int> GetActionWhenNotLearning(State<double> currentState)
        {
            this.Action.ActionVector[0] = this.GenerateActionWhenNotLearning(currentState.StateVector.ToArray());
            return this.Action;
        }

        public override void EpisodeStarted(State<double> currentState)
        {
            this.actorDSum.Clear();
            this.valueDSum.Clear();
        }

        public override Action<int> GetActionWhenLearning(State<double> currentState)
        {
            this.Action.ActionVector[0] = this.GenerateAction(currentState.StateVector.ToArray());
            return this.Action;
        }

        public override void Learn(Sample<double, int> sample)
        {
            double td = 0;
            double[] previousState = sample.PreviousState.StateVector.ToArray();

            if (!sample.CurrentState.IsTerminal)
            {
                double[] currentState = sample.CurrentState.StateVector.ToArray();
                double v1 = this.valuesNetwork.GetOutput(currentState);
                double v0 = this.valuesNetwork.GetOutput(previousState);
                td = (sample.Reinforcement * this.rScalator) + (this.discount * v1) - v0;
            }
            else
            {
                double v0 = this.valuesNetwork.GetOutput(previousState);
                td = (sample.Reinforcement * this.rScalator) - v0;
            }

            this.dVParam.SetValues(this.valuesNetwork.GetGradient(1));
            this.valueDSum = (this.valueDSum * (this.discount * this.lambda)) + this.dVParam;
            this.valuesNetwork.AddToParam(this.valueDSum, this.vStepSize * td);

            this.densityDTheta.SetValues(this.Get_dLnDensity_dTheta());
            this.actorDSum = (this.actorDSum * (this.discount * this.lambda)) + this.densityDTheta;
            this.network.AddToParam(this.actorDSum, this.aStepSize * td);

            for (int i = 0; i < this.networkOutput.Length; i++)
            {
                this.derivativeLnDensity[i]
                    = this.networkOutput[i] < -this.maxTheta ? this.overMaxThetaPenalty
                    : this.networkOutput[i] > this.maxTheta ? -this.overMaxThetaPenalty
                    : 0;
            }

            this.densityDTheta.SetValues(this.network.GetGradient(this.derivativeLnDensity));
            this.network.AddToParam(this.densityDTheta, this.aStepSize);
        }

        private int GenerateAction(double[] state)
        {
            this.networkOutput = this.network.Approximate(state);
            this.totalProp = 0;
            for (int i = 0; i < this.networkOutput.Length; i++)
            {
                this.totalProp += (this.prop[i] = System.Math.Exp(this.networkOutput[i] / this.radius));
            }

            double rnd = this.sampler.NextDouble() * this.totalProp;
            double total_prop = 0;
            this.neuralAction = this.networkOutput.Length - 1;
            for (int i = 0; i < this.networkOutput.Length; i++)
            {
                if (rnd <= (total_prop += this.prop[i]))
                {
                    this.neuralAction = i;
                    break;
                }
            }

            return this.neuralAction;
        }

        private int GenerateActionWhenNotLearning(double[] state)
        {
            this.networkOutput = this.network.Approximate(state);
            this.totalProp = 0;
            int maxIndex = 0;
            double max = double.NegativeInfinity;
            for (int i = 0; i < this.networkOutput.Length; i++)
            {
                double current = System.Math.Exp(this.networkOutput[i] / this.radius);
                if (current > max)
                {
                    max = current;
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        private double[] Get_dLnDensity_dTheta()
        {
            for (int i = 0; i < this.prop.Count; i++)
            {
                this.derivativeLnDensity[i] = -this.prop[i] / this.totalProp / this.radius;
            }

            this.derivativeLnDensity[this.neuralAction] += 1.0 / this.radius;
            return this.network.GetGradient(this.derivativeLnDensity);
        }

        private Vector<double> densityDTheta;
        private Vector<double> dVParam;
        private ANeuralAprx network;
        private ASampler sampler;
        private double[] networkOutput;
        private Vector<double> prop;
        private double totalProp;
        private int neuralAction;
        private Vector<double> derivativeLnDensity;
        private ANeuralAprx valuesNetwork;
        private MathNet.Numerics.LinearAlgebra.Generic.Vector<double> actorDSum;
        private MathNet.Numerics.LinearAlgebra.Generic.Vector<double> valueDSum;
    }
}