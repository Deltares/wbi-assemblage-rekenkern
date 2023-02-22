﻿// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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

using System;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model
{
    [TestFixture]
    public class ESectionInitialMechanismProbabilitySpecificationTest
    {
        [Test]
        public void ESectionInitialMechanismProbabilitySpecification_Always_ExpectedValues()
        {
            Assert.AreEqual(3, Enum.GetValues(typeof(ESectionInitialMechanismProbabilitySpecification)).Length);
            Assert.AreEqual(0, (int) ESectionInitialMechanismProbabilitySpecification.NotRelevant);
            Assert.AreEqual(1, (int) ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification);
            Assert.AreEqual(2, (int) ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification);
        }
    }
}