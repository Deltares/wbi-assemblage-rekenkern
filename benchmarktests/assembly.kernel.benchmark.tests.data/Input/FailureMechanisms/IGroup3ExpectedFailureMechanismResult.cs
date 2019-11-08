using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public interface IGroup3ExpectedFailureMechanismResult : IExpectedFailureMechanismResult
    {
        /// <summary>
        /// The probability space for this failure mechanism (a number between 0 and 1).
        /// </summary>
        double FailureMechanismProbabilitySpace { get; set; }

        /// <summary>
        /// The length-effect factor (number >= 1)
        /// </summary>
        double LengthEffectFactor { get; set; }

        /// <summary>
        /// The expected categories for this failure mechanism at failure mechanism section level.
        /// </summary>
        CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}