using System.Collections.Generic;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public abstract class ExpectedFailureMechanismResultBase : IExpectedFailureMechanismResult
    {
        protected ExpectedFailureMechanismResultBase(string name)
        {
            Name = name;
            Sections = new List<IFailureMechanismSection>();
        }

        public string Name { get; set; }

        public abstract MechanismType Type { get; }

        public abstract int Group { get; }

        public bool AccountForDuringAssembly { get; set; }

        public object ExpectedAssessmentResult { get; set; }

        public object ExpectedAssessmentResultTemporal { get; set; }

        public IEnumerable<IFailureMechanismSection> Sections { get; set; }
    }
}