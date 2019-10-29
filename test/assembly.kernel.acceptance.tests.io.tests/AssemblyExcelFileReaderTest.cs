using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using assembly.kernel.acceptance.tests.io.tests.Readers;
using NUnit.Framework;

namespace assembly.kernel.acceptance.tests.io.tests
{
    [TestFixture]
    public class AssemblyExcelFileReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReads()
        {
            var fileName = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");
            var result = AssemblyExcelFileReader.Read(fileName);
            Assert.IsNotNull(result);
        }
    }
}
