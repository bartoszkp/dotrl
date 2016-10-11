using System.Linq;
using BackwardCompatibility;
using Core;
using Core.Parameters;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Agents.ContinuousStateContinuousDecision
{
    public class ActorCriticAgent : Agent<double, double>
    {
        [Parameter(1, 300)]
        private int asize = 20;
        [Parameter(1, 300)]
        private int vsize = 50;
        [Parameter(0.1, 100.0)]
        private double actionDiffusion = 2.0;
        [Parameter(0.00000001, 1)]
        private double actorStepSize = 0.003;
        [Parameter(0.00000001, 1)]
        private double criticStepSize = 0.003;
        [Parameter(0.1, 100)]
        private double rewardScalator = 1;
        [Parameter(0.25, 1)]
        private double discount = 0.95;
        [Parameter(0.1, 10)]
        private double lambda = 0.5;
        [Parameter(0.0000001, 100000)]
        private double actionRadius = 30;

        public override void ExperimentStarted(EnvironmentDescription<double, double> environmentDescription)
        {
            actionAverage = Enumerable.Zip(
                environmentDescription.ActionSpaceDescription.MinimumValues,
                environmentDescription.ActionSpaceDescription.MaximumValues,
                (minimum, maximum) => 0.5 * (maximum + minimum))
                .ToArray();

            actionScaler = Enumerable.Zip(
                environmentDescription.ActionSpaceDescription.MinimumValues,
                environmentDescription.ActionSpaceDescription.MaximumValues,
                (minimum, maximum) => 0.5 * (maximum - minimum) / actionRadius)
                .ToArray();

            lActor = new CCNeuralNormalPolicy();
            lActor.Init(
                environmentDescription.ActionSpaceDescription.Dimensionality,
                asize,
                environmentDescription.StateSpaceDescription.AverageValues.ToArray(),
                environmentDescription.StateSpaceDescription.StandardDeviations.ToArray());

            lCritic = new ANeuralAprx();
            lCritic.Init(
                vsize, 
                1,
                CellType.Arcustangent,
                environmentDescription.StateSpaceDescription.AverageValues.ToArray(),
                environmentDescription.StateSpaceDescription.StandardDeviations.ToArray());

            actorDSum = new DenseVector(lActor.GetThetaDim());
            valuesDSum = new DenseVector(lCritic.GetParamDim());

            lActor.SetStdDev(actionDiffusion);

            dVdParam = new DenseVector(lCritic.GetParamDim());
            dLnDensitydTheta = new DenseVector(lActor.GetThetaDim());

            this.Action = new MutableAction<double>(actionAverage.Length);
        }

        public override Action<double> GetActionWhenNotLearning(State<double> currentState)
        {
            this.Action.ActionVector = lActor.GenerateActionWithoutNoise(currentState.StateVector.ToArray());

            for (int i = 0; i < actionAverage.Length; i++)
            {
                this.Action.ActionVector[i] *= actionScaler[i];
                this.Action.ActionVector[i] += actionAverage[i];
            }

            return this.Action;
        }

        public override void EpisodeStarted(State<double> currentState)
        {
            actorDSum.Clear();
            valuesDSum.Clear();
        }

        public override Action<double> GetActionWhenLearning(State<double> currentState)
        {
            this.Action.ActionVector = lActor.GenerateActionWithNoise(currentState.StateVector.ToArray());

            for (int i = 0; i < actionAverage.Length; i++)
            {
                this.Action.ActionVector[i] *= actionScaler[i];
                this.Action.ActionVector[i] += actionAverage[i];
            }

            return this.Action;
        }

        public override void Learn(Sample<double, double> sample)
        {
            double td = 0;
            double[] previousState = sample.PreviousState.StateVector.ToArray();

            if (!sample.CurrentState.IsTerminal)
            {
                double v1 = lCritic.GetOutput(sample.CurrentState.StateVector.ToArray());
                double v0 = lCritic.GetOutput(previousState);
                td = sample.Reinforcement * rewardScalator + discount * v1 - v0;
            }
            else
            {
                double v0 = lCritic.GetOutput(previousState);
                td = sample.Reinforcement * rewardScalator - v0;
            }

            dVdParam.SetValues(lCritic.GetGradient(1));
            valuesDSum = valuesDSum * (discount * lambda) + dVdParam;
            lCritic.AddToParam(valuesDSum, criticStepSize * td);

            dLnDensitydTheta.SetValues(lActor.Get_dLnDensity_dTheta());
            actorDSum = actorDSum * (discount * lambda) + dLnDensitydTheta;
            lActor.AddToTheta(actorDSum, actorStepSize * td);
        }

        private Vector<double> actorDSum;
        private Vector<double> dLnDensitydTheta;
        private Vector<double> valuesDSum;
        private Vector<double> dVdParam;
        private double[] actionAverage;
        private double[] actionScaler; 
        private CCNeuralNormalPolicy lActor; 
        private ANeuralAprx lCritic;
    }
}
