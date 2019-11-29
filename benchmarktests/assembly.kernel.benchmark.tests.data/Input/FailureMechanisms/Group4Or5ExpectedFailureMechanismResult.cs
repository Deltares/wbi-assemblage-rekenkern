namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public class Group4Or5ExpectedFailureMechanismResult : ExpectedFailureMechanismResultBase
    {
        public Group4Or5ExpectedFailureMechanismResult(string name, MechanismType type, int group) : base(name)
        {
            Type = type;
            Group = group;
        }

        public override MechanismType Type { get; }

        public override int Group { get; }
    }
}