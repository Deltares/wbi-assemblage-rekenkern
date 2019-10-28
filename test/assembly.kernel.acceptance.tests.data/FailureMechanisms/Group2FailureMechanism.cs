using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public class Group2FailureMechanism : FailureMechanismBase, IGroup1Or2FailureMechanism
    {
        public Group2FailureMechanism(string name, MechanismType type) : base(name)
        {
            Type = type;
        }

        public override MechanismType Type { get; }

        public override int Group => 2;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double ExpectedAssessmentResultProbability { get; set; }

        public double ExpectedTemporalAssessmentResultProbability { get; set; }

        public double LengthEffectFactor { get; set; }

        public CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }

        public CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}