namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public class Group4Or5FailureMechanismResult : FailureMechanismResultBase
    {
        public Group4Or5FailureMechanismResult(string name, MechanismType type, int group) : base(name)
        {
            Type = type;
            Group = group;
        }

        public override MechanismType Type { get; }

        public override int Group { get; }
    }
}
