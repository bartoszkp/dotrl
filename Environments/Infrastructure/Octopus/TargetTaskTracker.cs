using System;
using Environments.ContinuousStateContinuousDecision;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class TargetTaskTracker : TimeLimitTaskTracker
    {
        public Octopus Parent { get; set; }

        public TargetTaskTracker(Octopus parent, TargetTaskDef def)
            : base(def)
        {
            Parent = parent;
            objectiveTracker = MakeObjectiveTracker(def.Objective);
        }

        public override void Reset()
        {
            base.Reset();
            objectiveTracker.Reset();
            objectiveTracker.MakeEligible();
        }

        public override void Update()
        {
            base.Update();
            subgoalAchieved = objectiveTracker.Check();
        }

        public override double Reward
        {
            get
            {
                return subgoalAchieved ? reward : base.Reward;
            }
        }

        public void SetReward(double reward)
        {
            this.reward = reward;
        }

        public override bool Terminal
        {
            get
            {
                return base.Terminal || objectiveTracker.Accomplished;
            }
        }

        private ObjectiveTaskTracker objectiveTracker;
        private bool subgoalAchieved;
        private double reward;

        public ObjectiveTaskTracker MakeObjectiveTracker(ObjectiveSpec spec)
        {
            SequenceSpec sequenceSpec = spec as SequenceSpec;
            if (sequenceSpec != null)
            {
                return new SequenceTaskTracker(this, sequenceSpec);
            }
            else
            {
                ChoiceSpec choiceSpec = spec as ChoiceSpec;
                if (choiceSpec != null)
                {
                    return new ChoiceTracker(this, choiceSpec);
                }
                else
                {
                    TargetSpec targetSpec = spec as TargetSpec;
                    if (targetSpec != null)
                    {
                        return new SingleTargetTaskTracker(this, targetSpec);
                    }
                    else
                    {
                        throw new ArgumentException("Unknown objective type.");
                    }
                }
            }
        }
    }
}
