namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public class StbuExpectedFailureMechanismResult : ExpectedFailureMechanismResultBase
    {
        /// <summary>
        /// Creates an empty StbuExpectedFailureMechanismResult
        /// </summary>
        public StbuExpectedFailureMechanismResult() : base("Macrostabiliteit buitenwaarts") { }

        public override MechanismType Type => MechanismType.STBU;

        public override int Group => 4;

        /// <summary>
        /// The probability space for this failure mechanism (a number between 0 and 1).
        /// </summary>
        public double FailureMechanismProbabilitySpace { get; set; }

        /// <summary>
        /// The length-effect factor (number >= 1)
        /// </summary>
        public double LengthEffectFactor { get; set; }

        /// <summary>
        /// The expected probability (0 - 1) of the limit between category IIv and Vv.
        /// </summary>
        public double ExpectedSectionsCategoryDivisionProbability { get; set; }

        public bool UseSignallingNorm { get; set; }
    }
}