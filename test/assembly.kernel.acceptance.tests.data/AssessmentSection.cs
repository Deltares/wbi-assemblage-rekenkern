using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data.FailureMechanisms;
using Assembly.Kernel.Model;

namespace assembly.kernel.acceptance.tests.data
{
    public class AssessmentSection
    {
        public string Name { get; set; }

        public double Length { get; set; }

        public double SignallingNorm { get; set; }

        public double LowerBoundaryNorm { get; set; }

        public List<IFailureMechanism> FailureMechanisms { get; set; }

        public SafetyAssessmentAssemblyResult SafetyAssessmentAssemblyResult { get; set; }

        public AssemblyResult CombinedAssessmentSectionsResults { get; set; }
    }
}
