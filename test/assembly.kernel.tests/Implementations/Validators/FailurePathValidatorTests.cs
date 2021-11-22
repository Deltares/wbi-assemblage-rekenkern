﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations.Validators;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations.Validators
{
    [TestFixture]
    public class FailurePathValidatorTests
    {
        [Test,
         TestCaseSource(typeof(FailurePathValidatorTestData), nameof(FailurePathValidatorTestData.TestCases))]
        public List<EAssemblyErrors> FailurePathCheckTest(double lengthEffectFactor)
        {
            try
            {
                FailurePathValidator.CheckFailurePathInput(lengthEffectFactor);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                return e.Errors.Select(message => message.ErrorCode).ToList();
            }

            return null;
        }
    }

    public class FailurePathValidatorTestData
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(1).Returns(null);
                yield return new TestCaseData(10).Returns(null);
                yield return new TestCaseData(0).Returns(
                    new List<EAssemblyErrors>
                    {
                        EAssemblyErrors.LengthEffectFactorOutOfRange
                    });
                yield return new TestCaseData(-2).Returns(
                    new List<EAssemblyErrors>
                    {
                        EAssemblyErrors.LengthEffectFactorOutOfRange
                    });
            }
        }
    }
}