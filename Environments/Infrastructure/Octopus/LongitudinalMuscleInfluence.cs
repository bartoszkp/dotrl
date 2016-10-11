namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class LongitudinalMuscleInfluence : MuscleInfluence
    {
        public LongitudinalMuscleInfluence(ConstantSet constants, Node n1, Node n2, double width)
            : base(constants, n1, n2, width)
        {
            this.ActiveConstant = 0.5 * constants.MuscleActive * width * width / InitialLength;
            this.PassiveConstant = 0.5 * constants.MusclePassive * width * width / InitialLength;
            this.DampingConstant = 0.5 * constants.MuscleDamping * width * width / InitialLength;
        }
    }
}