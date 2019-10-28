using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    // TODO: Same class as Group2 mechanisms -> merge
    public class Group1FailureMechanism : FailureMechanismBase, IGroup1Or2FailureMechanism
    {
        public Group1FailureMechanism(string name, MechanismType type) : base(name)
        {
            Type = type;
        }

        public override MechanismType Type { get; }

        public override int Group => 1;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double ExpectedAssessmentResultProbability { get; set; }

        public double ExpectedTemporalAssessmentResultProbability { get; set; }

        public double LengthEffectFactor { get; set; }

        public CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }

        public CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}