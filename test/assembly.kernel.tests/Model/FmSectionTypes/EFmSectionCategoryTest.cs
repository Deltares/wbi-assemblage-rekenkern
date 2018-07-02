using System;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FmSectionTypes
{
    [TestFixture]
    public class EFmSectionCategoryTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(9, Enum.GetValues(typeof(EFmSectionCategory)).Length);
            Assert.AreEqual(-1, (int)EFmSectionCategory.NotApplicable);
            Assert.AreEqual(1, (int)EFmSectionCategory.Iv);
            Assert.AreEqual(2, (int)EFmSectionCategory.IIv);
            Assert.AreEqual(3, (int)EFmSectionCategory.IIIv);
            Assert.AreEqual(4, (int)EFmSectionCategory.IVv);
            Assert.AreEqual(5, (int)EFmSectionCategory.Vv);
            Assert.AreEqual(6, (int)EFmSectionCategory.VIv);
            Assert.AreEqual(7, (int)EFmSectionCategory.VIIv);
            Assert.AreEqual(8,(int)EFmSectionCategory.Gr);
            Assert.Greater(EFmSectionCategory.IIv,EFmSectionCategory.Iv);
        }
    }
}
