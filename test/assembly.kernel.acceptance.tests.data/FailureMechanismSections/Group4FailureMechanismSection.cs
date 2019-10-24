using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    public class Group4FailureMechanismSection : FailureMechanismSectionBase
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG1 DetailedAssessmentResult { get; set; }

        public EAssessmentResultTypeT1 TailorMadeAssessmentResult { get; set; }
    }
}
