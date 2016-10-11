namespace Environments.Infrastructure.OctopusInfrastructure
{
    internal class TransversalMuscleInfluence : MuscleInfluence
    {
        public TransversalMuscleInfluence(ConstantSet constants, Node n1, Node n2, double width)
            : base(constants, n1, n2, width)
        {
            this.ActiveConstant = constants.MuscleActive * this.InitialLength;
            this.PassiveConstant = constants.MusclePassive * this.InitialLength;
            this.DampingConstant = constants.MuscleDamping * this.InitialLength;
        }
    }
}