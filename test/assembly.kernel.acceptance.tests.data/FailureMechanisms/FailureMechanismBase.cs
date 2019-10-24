using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public abstract class FailureMechanismBase : IFailureMechanism
    {
        protected FailureMechanismBase(string name)
        {
            Name = name;
            Sections = new List<IFailureMechanismSection>();
        }

        public string Name { get; set; }

        public abstract MechanismType Type { get; }

        public abstract int Group { get; }

        public bool AccountForDuringAssembly { get; set; }

        public object ExpectedAssessmentResult { get; set; }

        public TResult GetResult<TResult>()
        {
            return (TResult) ExpectedAssessmentResult;
        }

        public IEnumerable<IFailureMechanismSection> Sections { get; }
    }
}