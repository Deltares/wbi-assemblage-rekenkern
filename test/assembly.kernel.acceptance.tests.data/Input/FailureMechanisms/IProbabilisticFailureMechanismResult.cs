using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public interface IProbabilisticFailureMechanismResult : IGroup3FailureMechanismResult
    {
        double ExpectedAssessmentResultProbability { get; set; }

        double ExpectedTemporalAssessmentResultProbability { get; set; }

        CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }
    }
}