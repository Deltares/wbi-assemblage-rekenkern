using System;
using System.IO;

namespace assembly.kernel.acceptance.tests.io.tests.Readers
{
    public class TestFileReaderTestBase
    {
        public string GetTestDir()
        {
            return Path.Combine(
                Path.GetDirectoryName(
                        Uri.UnescapeDataString(new UriBuilder(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Path))
                    .Replace(@"\bin\Debug", ""),
                "test-data");
        }
    }
}