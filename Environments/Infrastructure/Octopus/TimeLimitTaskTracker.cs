namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal abstract class TimeLimitTaskTracker : ITaskTracker
    {
        private int timeLimit;
        private double stepReward;

        private int timeLeft;

        protected internal TimeLimitTaskTracker(TaskDef def)
        {
            this.timeLimit = int.Parse(def.TimeLimit, System.Globalization.CultureInfo.CurrentCulture);
            this.stepReward = def.StepReward;
            timeLeft = timeLimit;
        }

        public virtual void Reset()
        {
            timeLeft = timeLimit;
        }

        public virtual void Update()
        {
            --timeLeft;
        }

        public virtual bool Terminal
        {
            get
            {
                return timeLeft == 0;
            }
        }

        public virtual double Reward
        {
            get
            {
                return stepReward;
            }
        }
    }
}
