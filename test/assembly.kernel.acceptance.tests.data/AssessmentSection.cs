using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data.FailureMechanisms;
using Assembly.Kernel.Model;

namespace assembly.kernel.acceptance.tests.data
{
    public class AssessmentSection
    {
        public AssessmentSection()
        {
            SafetyAssessmentAssemblyResult = new SafetyAssessmentAssemblyResult();
        }

        public string Name { get; set; }

        public double Length { get; set; }

        public double SignallingNorm { get; set; }

        public double LowerBoundaryNorm { get; set; }

        public List<IFailureMechanism> FailureMechanisms { get; set; }

        public SafetyAssessmentAssemblyResult SafetyAssessmentAssemblyResult { get; }

        public AssemblyResult ExpectedCommonSectionsResults { get; set; }
    }
}
