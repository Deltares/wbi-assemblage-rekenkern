using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public interface IProbabilisticFailureMechanism : IGroup3FailureMechanism
    {
        double ExpectedAssessmentResultProbability { get; set; }

        double ExpectedTemporalAssessmentResultProbability { get; set; }

        CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }
    }
}