namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal abstract class ObjectiveTaskTracker
    {
        public abstract void Reset();

        public abstract bool Check();

        public abstract bool Accomplished { get; }

        /* for display purposes */
        public abstract void MakeEligible();

        public abstract void MakeIneligible();
    }
}
