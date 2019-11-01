using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public class ProbabilisticFailureMechanismResult : FailureMechanismResultBase, IProbabilisticFailureMechanismResult
    {
        public ProbabilisticFailureMechanismResult(string name, MechanismType type, int group) : base(name)
        {
            Type = type;
            Group = group;
        }

        public override MechanismType Type { get; }

        public override int Group { get; }

        public double FailureMechanismProbabilitySpace { get; set; }

        public double ExpectedAssessmentResultProbability { get; set; }

        public double ExpectedTemporalAssessmentResultProbability { get; set; }

        public double LengthEffectFactor { get; set; }

        public CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }

        public CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}