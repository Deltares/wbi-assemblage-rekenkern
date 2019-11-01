namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public class StbuFailureMechanismResult : FailureMechanismResultBase
    {
        public StbuFailureMechanismResult() : base("Macrostabiliteit buitenwaarts") { }

        public override MechanismType Type => MechanismType.STBU;

        public override int Group => 4;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double LengthEffectFactor { get; set; }

        public double ExpectedSectionsCategoryDivisionProbability { get; set; }
    }
}