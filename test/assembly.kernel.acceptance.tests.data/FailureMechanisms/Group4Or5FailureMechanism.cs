namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public class Group4Or5FailureMechanism : FailureMechanismBase
    {
        public Group4Or5FailureMechanism(string name, MechanismType type) : base(name)
        {
            Type = type;
        }

        public override MechanismType Type { get; }

        public override int Group => 5;
    }
}
