using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public interface IGroup1Or2FailureMechanism : IGroup3FailureMechanism
    {
        double ExpectedAssessmentResultProbability { get; set; }

        double ExpectedTemporalAssessmentResultProbability { get; set; }

        CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }
    }
}