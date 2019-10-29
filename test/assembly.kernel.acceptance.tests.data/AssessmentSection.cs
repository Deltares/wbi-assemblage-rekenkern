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
            FailureMechanisms = new List<IFailureMechanism>();
        }

        public string Name { get; set; }

        public double Length { get; set; }

        public double SignallingNorm { get; set; }

        public double LowerBoundaryNorm { get; set; }

        public AssemblyResult ExpectedCommonSectionsResults { get; set; }

        public List<IFailureMechanism> FailureMechanisms { get; }

        public SafetyAssessmentAssemblyResult SafetyAssessmentAssemblyResult { get; }
    }
}
