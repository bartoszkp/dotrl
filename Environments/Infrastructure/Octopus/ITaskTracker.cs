namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal interface ITaskTracker
    {
        bool Terminal { get; }

        double Reward { get; }

        void Reset();

        void Update();
    }
}
