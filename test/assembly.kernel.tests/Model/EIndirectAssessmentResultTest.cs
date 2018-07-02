using System;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class EIndirectAssessmentResultTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(7, Enum.GetValues(typeof(EIndirectAssessmentResult)).Length);
            Assert.AreEqual(1, (int)EIndirectAssessmentResult.Nvt);
            Assert.AreEqual(2, (int)EIndirectAssessmentResult.FvEt);
            Assert.AreEqual(3, (int)EIndirectAssessmentResult.FvGt);
            Assert.AreEqual(4, (int)EIndirectAssessmentResult.FvTom);
            Assert.AreEqual(5, (int)EIndirectAssessmentResult.FactoredInOtherFailureMechanism);
            Assert.AreEqual(6, (int)EIndirectAssessmentResult.Ngo);
            Assert.AreEqual(7, (int)EIndirectAssessmentResult.Gr);
        }
    }
}
