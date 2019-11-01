using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public interface IProbabilisticFailureMechanismResult : IGroup3FailureMechanismResult
    {
        double ExpectedAssessmentResultProbability { get; set; }

        double ExpectedAssessmentResultProbabilityTemporal { get; set; }

        CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }
    }
}