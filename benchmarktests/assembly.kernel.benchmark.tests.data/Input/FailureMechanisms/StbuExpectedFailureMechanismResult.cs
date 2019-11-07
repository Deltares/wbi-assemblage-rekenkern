namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public class StbuExpectedFailureMechanismResult : ExpectedFailureMechanismResultBase
    {
        public StbuExpectedFailureMechanismResult() : base("Macrostabiliteit buitenwaarts") { }

        public override MechanismType Type => MechanismType.STBU;

        public override int Group => 4;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double LengthEffectFactor { get; set; }

        public double ExpectedSectionsCategoryDivisionProbability { get; set; }
    }
}