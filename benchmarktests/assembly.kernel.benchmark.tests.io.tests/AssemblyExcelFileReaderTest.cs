using System.IO;
using assembly.kernel.benchmark.tests.io.tests.Readers;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests
{
    [TestFixture]
    public class AssemblyExcelFileReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReads()
        {
            var fileName = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");
            var result = AssemblyExcelFileReader.Read(fileName, "Test");
            Assert.IsNotNull(result);
        }
    }
}
