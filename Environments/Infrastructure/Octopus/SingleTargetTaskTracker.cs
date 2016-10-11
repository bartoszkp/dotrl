using System.Linq;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class SingleTargetTaskTracker : ObjectiveTaskTracker
    {
        public SingleTargetTaskTracker(TargetTaskTracker parent, TargetSpec spec)
        {
            this.parent = parent;
            target = new Target(spec);
            parent.Parent.Targets = parent.Parent.Targets.Concat(Enumerable.Repeat(target, 1)).ToArray();
            accomplished = false;
        }

        public override void Reset()
        {
            accomplished = false;
        }

        public override bool Check()
        {
            bool hit = false;
            foreach (Compartment c in parent.Parent.Arm.Compartments)
            {
                if (c.Contains(target.Position))
                {
                    hit = true;
                    parent.SetReward(target.Value);
                    break;
                }
            }

            accomplished = hit;
            return hit;
        }

        public override bool Accomplished
        {
            get
            {
                return accomplished;
            }
        }

        public override void MakeEligible()
        {
            target.Eligible = true;
        }

        public override void MakeIneligible()
        {
            target.Eligible = false;
        }

        private Target target;
        private bool accomplished;
        private TargetTaskTracker parent;
    }
}
