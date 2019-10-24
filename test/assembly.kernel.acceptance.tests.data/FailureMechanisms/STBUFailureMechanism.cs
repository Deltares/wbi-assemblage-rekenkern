using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public class STBUFailureMechanism : FailureMechanismBase
    {
        public STBUFailureMechanism() : base("Macrostabiliteit buitenwaarts") { }

        public override MechanismType Type => MechanismType.STBU;

        public override int Group => 4;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double AFactor { get; set; }

        public double BFactor { get; set; }

        public double LengthEffectFactor { get; set; }

        public double ExpectedCategoryDivisionProbability { get; set; }

        // TODO: Not in benchmark tool?
        // public CategoriesList<FailureMechanismCategory> ExpectedFailureMechanismCategories { get; set; }

        public CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}