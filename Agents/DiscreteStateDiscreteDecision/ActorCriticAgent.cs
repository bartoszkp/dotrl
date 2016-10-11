using System.Linq;
using Core;
using Core.Parameters;

namespace Agents.DiscreteStateDiscreteDecision
{
    public class ActorCriticAgent : Agent<int, int>
    {
        [Parameter(0, 1)]
        private double discount = 0.95;
        [Parameter(0, 5.0)]
        private double betaV = 0.5;
        [Parameter(0, 5.0)]
        private double betaMu = 0.5;
        [Parameter(0, 50)]
        private double maxMu = 3;

        public ActorCriticAgent()
        {
            this.sampler = new System.Random();
        }

        public override void ExperimentStarted(EnvironmentDescription<int, int> environmentDescription)
        {
            int stateCount = environmentDescription.StateSpaceDescription.MaximumValues.First()
                - environmentDescription.StateSpaceDescription.MinimumValues.First()
                + 1;
            int actionCount = environmentDescription.ActionSpaceDescription.MaximumValues.First()
                - environmentDescription.ActionSpaceDescription.MinimumValues.First()
                + 1;

            this.v = new double[stateCount];
            this.mu = new double[stateCount][];
            for (int i = 0; i < stateCount; i++)
            {
                this.mu[i] = new double[actionCount];
            }

            this.expMu = new double[actionCount];

            this.Action = new MutableAction<int>(actionCount);
        }

        public override Action<int> GetActionWhenNotLearning(State<int> currentState)
        {
            int i;
            this.sumExp = 0;
            for (i = 0; i < this.expMu.Length; i++)
            {
                this.sumExp += this.expMu[i] = System.Math.Exp(this.mu[currentState.StateVector.First()][i]);
            }

            double rand = this.sampler.NextDouble() * this.sumExp;
            double sum = 0;
            for (i = 0; i < this.expMu.Length; i++)
            {
                if (rand <= (sum += this.expMu[i]))
                {
                    this.Action.ActionVector[0] = i;
                    break;
                }
            }

            return this.Action;
        }

        public override Action<int> GetActionWhenLearning(State<int> currentState)
        {
            return this.GetActionWhenNotLearning(currentState);
        }

        public override void Learn(Sample<int, int> sample)
        {
            int oldState = sample.PreviousState.StateVector.First();

            double next_v = 0;
            if (!sample.CurrentState.IsTerminal)
            {
                next_v = this.v[sample.CurrentState.StateVector.First()];
            }

            double td = sample.Reinforcement + (this.discount * next_v) - this.v[oldState];

            for (int i = 0; i < this.expMu[i]; i++)
            {
                if (sample.Action.ActionVector.First() == i)
                {
                    this.mu[oldState][i] += this.betaMu * td * (this.sumExp - this.expMu[i]) / this.sumExp;
                }
                else
                {
                    this.mu[oldState][i] += this.betaMu * td * (-this.expMu[i]) / this.sumExp;
                }

                if (this.mu[oldState][i] < -this.maxMu)
                {
                    this.mu[oldState][i] = -this.maxMu;
                }

                if (this.mu[oldState][i] > this.maxMu)
                {
                    this.mu[oldState][i] = this.maxMu;
                }
            }

            this.v[oldState] += this.betaV * td;
        }

        private System.Random sampler;
        private double[] v;
        private double[][] mu;
        private double[] expMu;
        private double sumExp;
    }
}
