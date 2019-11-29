using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;

namespace assembly.kernel.benchmark.tests.data.Input
{
    public class BenchmarkTestInput
    {
        public BenchmarkTestInput()
        {
            ExpectedSafetyAssessmentAssemblyResult = new SafetyAssessmentAssemblyResult();
            ExpectedFailureMechanismsResults = new List<IExpectedFailureMechanismResult>();
        }

        /// <summary>
        /// The file name that contains the benchmark testdefinition and expected results
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Name of the benchmark test
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Total length of the assessment section. Make sure this length equals the combined length of all sections per failure mechanism
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The signalling norm for this assessment section
        /// </summary>
        public double SignallingNorm { get; set; }

        /// <summary>
        /// The lower boundary norm for this assessment section
        /// </summary>
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

        /// <summary>
        /// Expected input and results per failure mechanism.
        /// </summary>
        public List<IExpectedFailureMechanismResult> ExpectedFailureMechanismsResults { get; }

        /// <summary>
        /// The expected safety assessment result on assessment section level.
        /// </summary>
        public SafetyAssessmentAssemblyResult ExpectedSafetyAssessmentAssemblyResult { get; }
    }
}