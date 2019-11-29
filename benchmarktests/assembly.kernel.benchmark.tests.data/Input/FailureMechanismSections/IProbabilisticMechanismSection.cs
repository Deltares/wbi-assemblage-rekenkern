using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public interface IProbabilisticMechanismSection : IFailureMechanismSection
    {
        /// <summary>
        /// The expected combined result of assembly with method WBI-0A-1.
        /// </summary>
        EFmSectionCategory ExpectedCombinedResult { get; set; }

        /// <summary>
        /// The expected combined result probability (0 - 1) of assembly with method WBI-0A-1.
        /// </summary>
        double ExpectedCombinedResultProbability { get; set; }
    }
}