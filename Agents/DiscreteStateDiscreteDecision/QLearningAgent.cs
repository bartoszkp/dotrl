using System.Linq;
using Core;
using Core.Parameters;
using Core.Reporting;

namespace Agents.DiscreteStateDiscreteDecision
{
    public class QLearningAgent : Agent<int, int>
    {
        private enum EActionSelection
        {
            EpsilonGreedy,
            Boltzmann
        }
        
        [Parameter(0.0, 10.0, "Learning rate.")]
        private double beta = 0.1;

        [Parameter]
        private EActionSelection actionSelection = EActionSelection.EpsilonGreedy;

        [Parameter(0.0, 0.5, "Used only for Epsilon-greedy action selection scheme.")]
        private double epsilon = 0.1;

        [Parameter(0, null, "Used only for Boltzmann action selection scheme.")]
        private double temperature = 1;

        [ReportedValue]
        private double td;

        public QLearningAgent()
        {
            this.sampler = new System.Random();
            this.q = null;
            this.Action = null;
        }

        public override void ExperimentStarted(EnvironmentDescription<int, int> environmentDescription)
        {
            this.stateCount = 
                environmentDescription.StateSpaceDescription.MaximumValues.Single()
                - environmentDescription.StateSpaceDescription.MinimumValues.Single()
                + 1;
            this.actionCount = 
                environmentDescription.ActionSpaceDescription.MaximumValues.Single()
                - environmentDescription.ActionSpaceDescription.MinimumValues.Single()
                 + 1;
            this.environmentDescription = environmentDescription;
            
            this.q = new double[stateCount][];
            
            for (int i = 0; i < stateCount; ++i)
            {
                this.q[i] = new double[actionCount];
            }

            this.Action = new MutableAction<int>(1);

            this.discountFactor = environmentDescription.DiscountFactor;

            this.actionProbabilities = new double[actionCount];
        }

        public override Action<int> GetActionWhenNotLearning(State<int> currentState)
        {
            return this.GetMaximumAction(currentState);
        }

        public override Action<int> GetActionWhenLearning(State<int> currentState)
        {
            return this.GetAction(currentState);
        }

        public override void Learn(Sample<int, int> sample)
        {
            int previousState = sample.PreviousState.SingleValue;
            int? currentState = sample.CurrentState.IsTerminal ? (int?)null : sample.CurrentState.SingleValue;
            int action = sample.Action.SingleValue;

            double currentQ = currentState.HasValue ? this.q[currentState.Value].Max() : 0;

            this.td = sample.Reinforcement + this.discountFactor * currentQ - this.q[previousState][action];

            this.q[previousState][action] += this.beta * this.td;
        }

        private Action<int> GetAction(State<int> currentState)
        {
            switch (this.actionSelection)
            {
                case EActionSelection.EpsilonGreedy:
                    return GetActionEpsilonGreedy(currentState);
                case EActionSelection.Boltzmann:
                    return GetActionBoltzmann(currentState);
                default:
                    throw new System.InvalidOperationException();
            }
        }

        private Action<int> GetActionEpsilonGreedy(State<int> currentState)
        {
            if (this.sampler.NextDouble() < this.epsilon)
            {
                return GetUniformlyRandomAction();
            }
            else
            {
                return GetMaximumAction(currentState);
            }
        }

        private Action<int> GetMaximumAction(State<int> currentState)
        {
            this.Action.SingleValue = 0;

            for (int i = 1; i < this.actionCount; ++i)
            {
                if (this.q[currentState.SingleValue][i] > this.q[currentState.SingleValue][this.Action.SingleValue])
                {
                    this.Action.SingleValue = i;
                }
            }

            return this.Action;
        }

        private Action<int> GetUniformlyRandomAction()
        {
            this.Action.SingleValue = this.sampler.Next(this.actionCount);

            return this.Action;
        }

        private Action<int> GetActionBoltzmann(State<int> currentState)
        {
            double total = 0;
            for (int i = 0; i < this.actionCount; ++i)
            {
                double p = System.Math.Exp(this.q[currentState.SingleValue][i] / this.temperature);
                this.actionProbabilities[i] = p;

                total += p;
            }

            double randomSample = this.sampler.NextDouble();

            int action = 0;
            double sum = 0;
            for (int i = 0; i < this.actionCount; ++i)
            {
                double current = this.actionProbabilities[i] / total;
                if (randomSample < (sum + current))
                {
                    action = i;
                    break;
                }

                sum += current;
            }

            this.Action.SingleValue = action;

            return this.Action;
        }

        private System.Random sampler;
        private double[][] q;
        private double[] actionProbabilities;
        private EnvironmentDescription<int, int> environmentDescription;
        private int stateCount;
        private int actionCount;
        private double discountFactor;
    }
}