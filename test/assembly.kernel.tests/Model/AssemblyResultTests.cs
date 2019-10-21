#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class AssemblyResultTests
    {
        [Test]
        public void CombinedSectionResultNullTest()
        {
            try
            {
                new AssemblyResult(new List<FailureMechanismSectionList>(), null);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void ResultPerFailureMechanismNullTest()
        {
            try
            {
                new AssemblyResult(null, new List<FmSectionWithDirectCategory>());
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }
    }
}