using System;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public class FailureMechanismInfo
    {
        public FailureMechanismInfo(string name, MechanismType type, int group, Func<IFailureMechanism> creationFunc)
        {
            Name = name;
            Type = type;
            Group = group;
            CreationFunc = creationFunc;
        }

        public string Name { get; }

        public MechanismType Type { get; }

        public int Group { get; }

        public Func<IFailureMechanism> CreationFunc { get; }
    }
}