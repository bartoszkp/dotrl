namespace Core
{
    public class EnvironmentDescription<TStateType, TActionType>
        where TStateType : struct
        where TActionType : struct
    {
        public SpaceDescription<TStateType> StateSpaceDescription { get; private set; }

        public SpaceDescription<TActionType> ActionSpaceDescription { get; private set; }

        public DimensionDescription<double> ReinforcementSpaceDescription { get; private set; }

        public double DiscountFactor { get; private set; }

        public EnvironmentDescription(
            SpaceDescription<TStateType> stateSpaceDescription,
            SpaceDescription<TActionType> actionSpaceDescription,
            DimensionDescription<double> reinforcementSpaceDescription,
            double discountFactor)
        {
            this.ActionSpaceDescription = actionSpaceDescription;
            this.StateSpaceDescription = stateSpaceDescription;
            this.ReinforcementSpaceDescription = reinforcementSpaceDescription;
            this.DiscountFactor = discountFactor;
        }
    }
}
