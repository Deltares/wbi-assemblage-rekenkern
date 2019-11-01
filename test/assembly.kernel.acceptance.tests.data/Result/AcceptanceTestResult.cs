using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.Result
{
    public class AcceptanceTestResult
    {
        public AcceptanceTestResult()
        {
            FailureMechanismResults = new List<AcceptanceTestFailureMechanismResult>();
        }

        public bool AreEqualCategoriesListAssessmentSection { get; set; }

        public IList<AcceptanceTestFailureMechanismResult> FailureMechanismResults { get; }

        public bool AreEqualAssemblyResultGroup1and2 { get; set; }

        public bool AreEqualAssemblyResultGroup3and4 { get; set; }

        public bool AreEqualAssemblyResultGroup5 { get; set; }

        public bool AreEqualAssemblyResultFinalVerdict { get; set; }

        public bool AreEqualAssemblyResultCombinedSectionsAmount { get; set; }

        public bool AreEqualAssemblyResultCombinedSections { get; set; }
    }
}
