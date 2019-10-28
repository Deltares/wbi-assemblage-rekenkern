using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public interface IFailureMechanism
    {
        string Name { get; set; }

        MechanismType Type { get; }

        int Group { get; }

        bool AccountForDuringAssembly { get; set; }

        object ExpectedAssessmentResult { get; set; }

        object ExpectedTemporalAssessmentResult { get; set; }

        TResult GetResult<TResult>(bool temporal);

        IEnumerable<IFailureMechanismSection> Sections { get; set; }

        // TODO: Add properties and generic methods as Assamble() and CheckInput() or WriteTestResult(), HasSimpleAssessment()? etc.methods
    }
}