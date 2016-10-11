namespace Application.Integration.RLGlue
{
    public interface IRLGlueInterface
    {
        double CurrentReward { get; }

        double AverageReward { get; }
        
        double EpisodeAverageReward { get; }
    }
}
