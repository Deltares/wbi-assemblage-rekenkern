using System.Collections.Generic;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.data.Input
{
    /// <summary>
    /// List of failure mechanism sections that includes the FailureMechanism ID.
    /// </summary>
    public class FailureMechanismSectionListWithFailureMechanismId : FailureMechanismSectionList
    {
        public FailureMechanismSectionListWithFailureMechanismId(string failureMechanismId, IEnumerable<FailureMechanismSection> sectionResults) : base(sectionResults)
        {
            FailureMechanismId = failureMechanismId;
        }

        /// <summary>
        /// ID of the failure mechanism
        /// </summary>
        public string FailureMechanismId { get; }

    }
}
