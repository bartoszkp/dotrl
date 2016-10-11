using System.Collections.Generic;

namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class ChoiceTracker : ObjectiveTaskTracker
    {
        public ChoiceTracker(TargetTaskTracker parent, ChoiceSpec spec)
        {
            subObjectives = new List<ObjectiveTaskTracker>();
            foreach (ObjectiveSpec os in spec.Objective)
            {
                subObjectives.Add(parent.MakeObjectiveTracker(os));
            }

            selected = null;
            accomplished = false;
        }

        public override void Reset()
        {
            foreach (ObjectiveTaskTracker o in subObjectives)
            {
                o.Reset();
            }

            selected = null;
            accomplished = false;
        }

        public override bool Check()
        {
            bool hit = false;
            if (selected == null)
            {
                foreach (ObjectiveTaskTracker o in subObjectives)
                {
                    hit = o.Check();
                    if (hit)
                    {
                        selected = o;
                        break;
                    }
                }

                if (hit)
                {
                    foreach (ObjectiveTaskTracker o in subObjectives)
                    {
                        if (o != selected)
                        {
                            o.MakeIneligible();
                        }
                    }
                }
            }
            else
            {
                hit = selected.Check();
            }

            if (selected != null)
            {
                accomplished = selected.Accomplished;
                if (accomplished)
                {
                    selected.MakeIneligible();
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
            foreach (ObjectiveTaskTracker o in subObjectives)
            {
                o.MakeEligible();
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
        private ObjectiveTaskTracker selected;
        private bool accomplished;
    }
}
