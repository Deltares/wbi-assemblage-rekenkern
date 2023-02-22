// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.

using System.IO;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Acceptance.TestUtil.IO;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.TestUtil.Explicit
{
    [TestFixture]
    public class AssemblyExcelFileReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReads()
        {
            string fileName = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.TestUtil"),
                                           "Benchmartktest - voorbeeld - 83-1.xlsx");
            BenchmarkTestInput result = AssemblyExcelFileReader.Read(fileName);
            Assert.IsNotNull(result);
        }
    }
}