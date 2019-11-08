using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public interface IProbabilisticExpectedFailureMechanismResult : IGroup3ExpectedFailureMechanismResult
    {
        /// <summary>
        /// The expected assessment result probability corresponding to the expected assessment result
        /// </summary>
        double ExpectedAssessmentResultProbability { get; set; }

        /// <summary>
        /// The expected assessment result probability corresponding to the expected assessment result as a result of partial assembly.
        /// </summary>
        double ExpectedAssessmentResultProbabilityTemporal { get; set; }

        /// <summary>
        /// The expected categories for this failure mechanism at failure mechanism level.
        /// </summary>
        CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }
    }
}