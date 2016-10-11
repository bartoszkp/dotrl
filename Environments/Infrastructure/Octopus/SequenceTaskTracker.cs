using System.Collections.Generic;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class SequenceTaskTracker : ObjectiveTaskTracker
    {
        public SequenceTaskTracker(TargetTaskTracker parent, SequenceSpec spec)
        {
            subObjectives = new List<ObjectiveTaskTracker>();
            foreach (ObjectiveSpec os in spec.Objective)
            {
                subObjectives.Add(parent.MakeObjectiveTracker(os));
            }

            current = 0;
            accomplished = false;
        }

        public override void Reset()
        {
            foreach (ObjectiveTaskTracker o in subObjectives)
            {
                o.Reset();
            }

            current = 0;
            accomplished = false;
        }

        public override bool Check()
        {
            bool hit = subObjectives[current].Check();
            if (hit && subObjectives[current].Accomplished)
            {
                subObjectives[current].MakeIneligible();
                current++;

                if (current < subObjectives.Count)
                {
                    subObjectives[current].MakeEligible();
                }
                else
                {
                    accomplished = true;
                }
            }

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
            subObjectives[0].MakeEligible();
            for (int i = 1; i < subObjectives.Count; i++)
            {
                subObjectives[i].MakeIneligible();
            }
        }

        public override void MakeIneligible()
        {
            foreach (ObjectiveTaskTracker o in subObjectives)
            {
                o.MakeIneligible();
            }
        }

        private IList<ObjectiveTaskTracker> subObjectives;
        private int current;
        private bool accomplished;
    }
}
