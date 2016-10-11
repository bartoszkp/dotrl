using System.Collections.Generic;
using System.Linq;
using Environments.ContinuousStateContinuousDecision;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class FoodTaskTracker : TimeLimitTaskTracker
    {
        public FoodTaskTracker(Octopus parent, FoodTaskDef taskDef)
            : base(taskDef)
        {
            this.parent = parent;
            parent.Mouth = new Mouth(taskDef.Mouth);

            initialFood = new HashSet<Food>();
            initialFood.UnionWith(taskDef.Food.Select(f => new Food(f)));

            parent.Food.UnionWith(initialFood);
        }

        public override void Reset()
        {
            base.Reset();
            parent.Food.UnionWith(initialFood);
        }

        public override void Update()
        {
            base.Update();
            subgoalAchieved = false;
            reward = 0.0;
            foreach (Food f in parent.Food)
            {
                if (parent.Mouth.Contains(f.Position))
                {
                    parent.Food.Remove(f);
                    f.Warp();
                    subgoalAchieved = true;
                    reward += f.Value;
                }
            }
        }

        public override bool Terminal
        {
            get
            {
                return base.Terminal || parent.Food.Count == 0;
            }
        }

        public override double Reward
        {
            get
            {
                return subgoalAchieved ? reward : base.Reward;
            }
        }

        private bool subgoalAchieved;
        private double reward;
        private HashSet<Food> initialFood;
        private Octopus parent;
    }
}
