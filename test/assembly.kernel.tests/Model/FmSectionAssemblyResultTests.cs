#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class FmSectionAssemblyResultTests
    {
        private static void CheckException(AssemblyException e)
        {
            Assert.NotNull(e.Errors);
            var message = e.Errors.FirstOrDefault();
            Assert.NotNull(message);
            Assert.AreEqual(EAssemblyErrors.FailureProbabilityOutOfRange, message.ErrorCode);
            Assert.Pass();
        }

        [Test]
        public void DirectFailureProbAboveOne()
        {
            try
            {
                new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Iv, 1.5);
            }
            catch (AssemblyException e)
            {
                CheckException(e);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void DirectFailureProbBelowZero()
        {
            try
            {
                new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Iv, -1.0);
            }
            catch (AssemblyException e)
            {
                CheckException(e);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void DirectToStringTest()
        {
            var result = new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv);

            Assert.AreEqual($"FmSectionAssemblyDirectResult [Iv]", result.ToString());
        }

        [Test]
        public void DirectWithProbabilityToStringTest()
        {
            const double FailureProb = 0.2;
            var result = new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Iv, FailureProb);

            Assert.AreEqual($"FmSectionAssemblyDirectResultWithProbability [Iv P: {FailureProb}]", result.ToString());
        }

        [Test]
        public void IndirectToStringTest()
        {
            var result = new FmSectionAssemblyIndirectResult(EIndirectAssessmentResult.FvEt);

            Assert.AreEqual("FmSectionAssemblyIndirectResult [FvEt]", result.ToString());
        }
    }
}