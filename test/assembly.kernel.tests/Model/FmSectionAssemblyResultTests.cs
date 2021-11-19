#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
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
#endregion

using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.FailurePathSectionResults;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class FmSectionAssemblyResultTests
    {
        [Test]
        [TestCase(1.5,0.4)]
        [TestCase(0.5, 1.4)]
        [TestCase(-0.5, 0.4)]
        [TestCase(0.5, -0.4)]
        [TestCase(-0.5, 10.4)]
        public void FmSectionAssemblyResultConstructorChecksValidProbabilities(double probabilityProfile, double probabilitySection)
        {
            try
            {
                new FailurePathSectionAssemblyResult(probabilityProfile, probabilitySection, EInterpretationCategory.D);
            }
            catch (AssemblyException e)
            {
                CheckException(e);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        public void FmSectionAssemblyResultConstructorPassesArguments()
        {
            var result = new FailurePathSectionAssemblyResult(0.1,0.2,EInterpretationCategory.III);
            Assert.AreEqual(EInterpretationCategory.III, result.InterpretationCategory);
            Assert.AreEqual(0.2, result.ProbabilityProfile);
            Assert.AreEqual(0.1, result.ProbabilitySection);
            Assert.AreEqual(2.0, result.NSection);
        }

        [Test]
        public void FmSectionAssemblyResultToStringTest()
        {
            var result = new FailurePathSectionAssemblyResult(0.2,0.1,EInterpretationCategory.III);

            Assert.AreEqual("FailurePathSectionAssemblyResult [III Pprofile:0.2, Psection:0.1]", result.ToString());
        }

        private static void CheckException(AssemblyException e)
        {
            Assert.NotNull(e.Errors);
            var message = e.Errors.FirstOrDefault();
            Assert.NotNull(message);
            Assert.AreEqual(EAssemblyErrors.FailureProbabilityOutOfRange, message.ErrorCode);
            Assert.Pass();
        }
    }
}