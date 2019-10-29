using System.Dynamic;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public interface IFailureMechanismSection
    {
        double Start { get; set; }

        double End { get; set; }

        string SectionName { get; set; }

        IFmSectionAssemblyResult ExpectedSimpleAssessmentAssemblyResult { get; set; }

        IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; set; }

        IFmSectionAssemblyResult ExpectedTailorMadeAssessmentAssemblyResult { get; set; }
    }
}