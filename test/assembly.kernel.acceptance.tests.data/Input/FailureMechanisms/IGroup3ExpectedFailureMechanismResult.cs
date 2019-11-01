using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public interface IGroup3ExpectedFailureMechanismResult : IExpectedFailureMechanismResult
    {
        double FailureMechanismProbabilitySpace { get; set; }

        double LengthEffectFactor { get; set; }

        CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}