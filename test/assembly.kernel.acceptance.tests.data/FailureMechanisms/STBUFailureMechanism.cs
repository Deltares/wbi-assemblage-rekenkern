namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public class STBUFailureMechanism : FailureMechanismBase
    {
        public STBUFailureMechanism() : base("Macrostabiliteit buitenwaarts") { }

        public override MechanismType Type => MechanismType.STBU;

        public override int Group => 4;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double LengthEffectFactor { get; set; }

        public double ExpectedSctionsCategoryDivisionProbability { get; set; }
    }
}