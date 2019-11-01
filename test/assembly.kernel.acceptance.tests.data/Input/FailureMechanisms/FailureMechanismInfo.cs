using System;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public class FailureMechanismInfo
    {
        public FailureMechanismInfo(string name, MechanismType type, int group, Func<IFailureMechanismResult> creationFunc)
        {
            Name = name;
            Type = type;
            Group = group;
            CreationFunc = creationFunc;
        }

        public string Name { get; }

        public MechanismType Type { get; }

        public int Group { get; }

        public Func<IFailureMechanismResult> CreationFunc { get; }
    }
}