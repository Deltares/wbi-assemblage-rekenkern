using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;

namespace assembly.kernel.acceptance.tests.data.Input
{
    public class AcceptanceTestInput
    {
        public AcceptanceTestInput()
        {
            ExpectedSafetyAssessmentAssemblyResult = new SafetyAssessmentAssemblyResult();
            ExpectedFailureMechanismsResults = new List<IFailureMechanismResult>();
        }

        public string Name { get; set; }

        public double Length { get; set; }

        public double SignallingNorm { get; set; }

        public double LowerBoundaryNorm { get; set; }

        public AssemblyResult ExpectedCommonSectionsResults { get; set; }

        public List<IFailureMechanismResult> ExpectedFailureMechanismsResults { get; }

        public SafetyAssessmentAssemblyResult ExpectedSafetyAssessmentAssemblyResult { get; }
    }
}
