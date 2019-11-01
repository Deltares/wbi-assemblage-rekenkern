using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public interface IFailureMechanismResult
    {
        string Name { get; set; }

        MechanismType Type { get; }

        int Group { get; }

        bool AccountForDuringAssembly { get; set; }

        object ExpectedAssessmentResult { get; set; }

        object ExpectedTemporalAssessmentResult { get; set; }

        IEnumerable<IFailureMechanismSection> Sections { get; set; }
    }
}