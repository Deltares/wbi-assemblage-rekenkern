using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public interface IGroup3FailureMechanism : IFailureMechanism
    {
        double FailureMechanismProbabilitySpace { get; set; }

        double LengthEffectFactor { get; set; }

        CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}