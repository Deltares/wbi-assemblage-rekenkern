﻿using System.Collections.Generic;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public abstract class FailureMechanismResultBase : IFailureMechanismResult
    {
        protected FailureMechanismResultBase(string name)
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