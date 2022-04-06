#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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

#endregion

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class GreatestCommonDenominatorAssemblyResultTests
    {
        [Test]
        public void CombinedSectionResultNullTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new GreatestCommonDenominatorAssemblyResult(new List<FailureMechanismSectionList>(), null);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void ResultPerFailureMechanismNullTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new GreatestCommonDenominatorAssemblyResult(null, new List<FailureMechanismSectionWithCategory>());
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void ConstructorPassesArguments()
        {
            var resultPerFailureMechanism = new []
            {
                new FailureMechanismSectionList(new []{new FailureMechanismSection(0,10) })
            };
            var combinedSectionResult = new []
            {
                new FailureMechanismSectionWithCategory(0,10,EInterpretationCategory.I)
            };
            var result = new GreatestCommonDenominatorAssemblyResult(resultPerFailureMechanism, combinedSectionResult);

            Assert.AreEqual(resultPerFailureMechanism,result.ResultPerFailureMechanism);
            Assert.AreEqual(combinedSectionResult, result.CombinedSectionResult);
        }
    }
}