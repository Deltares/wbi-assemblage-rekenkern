using System;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class EFailureMEchanismCategoryTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(9, Enum.GetValues(typeof(EFailureMechanismCategory)).Length);
            Assert.AreEqual(-1, (int)EFailureMechanismCategory.Nvt);
            Assert.AreEqual(1, (int)EFailureMechanismCategory.It);
            Assert.AreEqual(2, (int)EFailureMechanismCategory.IIt);
            Assert.AreEqual(3, (int)EFailureMechanismCategory.IIIt);
            Assert.AreEqual(4, (int)EFailureMechanismCategory.IVt);
            Assert.AreEqual(5, (int)EFailureMechanismCategory.Vt);
            Assert.AreEqual(6, (int)EFailureMechanismCategory.VIt);
            Assert.AreEqual(7, (int)EFailureMechanismCategory.VIIt);
            Assert.AreEqual(8, (int)EFailureMechanismCategory.Gr);
            Assert.Greater(EFailureMechanismCategory.IIt, EFailureMechanismCategory.It);
        }
    }
}
