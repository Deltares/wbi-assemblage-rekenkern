using System;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class EAssessmentGradeTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(8, Enum.GetValues(typeof(EAssessmentGrade)).Length);
            Assert.AreEqual(-1, (int) EAssessmentGrade.Nvt);
            Assert.AreEqual(1, (int) EAssessmentGrade.APlus);
            Assert.AreEqual(2, (int) EAssessmentGrade.A);
            Assert.AreEqual(3, (int) EAssessmentGrade.B);
            Assert.AreEqual(4, (int) EAssessmentGrade.C);
            Assert.AreEqual(5, (int) EAssessmentGrade.D);
            Assert.AreEqual(6, (int) EAssessmentGrade.Ngo);
            Assert.AreEqual(7, (int) EAssessmentGrade.Gr);
            Assert.Greater(EAssessmentGrade.B, EAssessmentGrade.A);
        }
    }
}