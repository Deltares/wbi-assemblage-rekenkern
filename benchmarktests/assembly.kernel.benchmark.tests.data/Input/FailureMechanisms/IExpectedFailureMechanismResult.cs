using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public interface IExpectedFailureMechanismResult
    {
        /// <summary>
        /// Name of the failure mechanism
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of the failure mechanism
        /// </summary>
        MechanismType Type { get; }

        /// <summary>
        /// Assembly group (1, 2, 3, 4 or 5)
        /// </summary>
        int Group { get; }

        /// <summary>
        /// Denotes whether the failure mechanism should be taken into account while performing assembly
        /// </summary>
        /// TODO: Currently not used. Remove this parameter?
        bool AccountForDuringAssembly { get; set; }

        /// <summary>
        /// The expected result (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        object ExpectedAssessmentResult { get; set; }

        /// <summary>
        /// The expected result while performing partial assembly (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        object ExpectedAssessmentResultTemporal { get; set; }

        /// <summary>
        /// A listing of all sections within the failure mechanism
        /// </summary>
        IEnumerable<IFailureMechanismSection> Sections { get; set; }
    }
}