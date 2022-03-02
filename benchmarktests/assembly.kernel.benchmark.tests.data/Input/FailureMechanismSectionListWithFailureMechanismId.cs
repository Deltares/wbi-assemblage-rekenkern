using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.data.Input
{
    public class FailureMechanismSectionListWithFailureMechanismId : FailureMechanismSectionList
    {
        public FailureMechanismSectionListWithFailureMechanismId(string failureMechanismId, IEnumerable<FailureMechanismSection> sectionResults) : base(sectionResults)
        {
            FailureMechanismId = failureMechanismId;
        }

        public string FailureMechanismId { get; set; }

    }
}
