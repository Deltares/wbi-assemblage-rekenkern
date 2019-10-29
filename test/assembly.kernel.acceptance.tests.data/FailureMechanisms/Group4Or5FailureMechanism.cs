namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public class Group4Or5FailureMechanism : FailureMechanismBase
    {
        public Group4Or5FailureMechanism(string name, MechanismType type, int group) : base(name)
        {
            Type = type;
            Group = group;
        }

        public override MechanismType Type { get; }

        public override int Group { get; }
    }
}
