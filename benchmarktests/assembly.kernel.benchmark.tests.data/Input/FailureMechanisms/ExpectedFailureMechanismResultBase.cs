using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public abstract class ExpectedFailureMechanismResultBase : IExpectedFailureMechanismResult
    {
        protected ExpectedFailureMechanismResultBase(string name)
        {
            Name = name;
            Sections = new List<IFailureMechanismSection>();
        }

        /// <summary>
        /// Name of the failure mechanism
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the failure mechanism
        /// </summary>
        public abstract MechanismType Type { get; }

        /// <summary>
        /// Assembly group (1, 2, 3, 4 or 5)
        /// </summary>
        public abstract int Group { get; }

        /// <summary>
        /// Denotes whether the failure mechanism should be taken into account while performing assembly
        /// </summary>
        public bool AccountForDuringAssembly { get; set; }

        /// <summary>
        /// The expected result (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        public object ExpectedAssessmentResult { get; set; }

        /// <summary>
        /// The expected result while performing partial assembly (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        public object ExpectedAssessmentResultTemporal { get; set; }

        /// <summary>
        /// A listing of all sections within the failure mechanism
        /// </summary>
        public IEnumerable<IFailureMechanismSection> Sections { get; set; }
    }
}