using System;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class EFailureMechanismAssemblyMethodTest
    {
        [Test]
        public void EFailureMechanismAssemblyMethodContractTest()
        {
            Assert.AreEqual(2, Enum.GetValues(typeof(EFailureMechanismAssemblyMethod)).Length);
            Assert.AreEqual(1, (int)EFailureMechanismAssemblyMethod.Correlated);
            Assert.AreEqual(2, (int)EFailureMechanismAssemblyMethod.UnCorrelated);
        }
    }
}
