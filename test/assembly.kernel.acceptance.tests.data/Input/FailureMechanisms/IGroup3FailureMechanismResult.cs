using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public interface IGroup3FailureMechanismResult : IFailureMechanismResult
    {
        double FailureMechanismProbabilitySpace { get; set; }

        double LengthEffectFactor { get; set; }

        CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}