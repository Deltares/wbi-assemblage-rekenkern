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

        TResult GetResult<TResult>();

        IEnumerable<IFailureMechanismSection> Sections { get; }

        // TODO: Add properties and generic methods as Assamble() and CheckInput() or WriteTestResult(), HasSimpleAssessment()? etc.methods
    }
}