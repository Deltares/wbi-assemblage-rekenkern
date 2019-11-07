using System.Collections.Generic;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;

namespace assembly.kernel.acceptance.tests.data.Input
{
    public class BenchmarkTestInput
    {
        public BenchmarkTestInput()
        {
            ExpectedSafetyAssessmentAssemblyResult = new SafetyAssessmentAssemblyResult();
            ExpectedFailureMechanismsResults = new List<IExpectedFailureMechanismResult>();
        }

        public string FileName { get; set; }

        public string TestName { get; set; }

        public string AssessmentSectionId { get; set; }

        public double Length { get; set; }

        public double SignallingNorm { get; set; }

        public double LowerBoundaryNorm { get; set; }

        /// <summary>
        /// The greatest common denominator section results per Failure mechanism.
        /// </summary>
        public IEnumerable<FailureMechanismSectionList> ExpectedCombinedSectionResultPerFailureMechanism { get; set; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechnisms combined.
        /// </summary>
        public IEnumerable<FmSectionWithDirectCategory> ExpectedCombinedSectionResult { get; set; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechnisms combined.
        /// </summary>
        public IEnumerable<FmSectionWithDirectCategory> ExpectedCombinedSectionResultTemporal { get; set; }

        public List<IExpectedFailureMechanismResult> ExpectedFailureMechanismsResults { get; }

        public SafetyAssessmentAssemblyResult ExpectedSafetyAssessmentAssemblyResult { get; }
    }
}
