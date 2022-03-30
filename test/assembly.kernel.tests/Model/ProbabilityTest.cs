#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class ProbabilityTest
    {
        [Test]
        public void UndefinedProbabilityReturnsUndefinedProbability()
        {
            var nanValue = Probability.Undefined;

            Assert.IsAssignableFrom<Probability>(nanValue);
            Assert.IsFalse(nanValue.IsDefined);
        }

        [Test]
        public void ConstructorPassesValue()
        {
            var returnPeriod = 1000.0;
            var val = 1.0/returnPeriod;
            var probability = new Probability(val);

            Assert.AreEqual(val, probability);
            Assert.AreEqual(returnPeriod, probability.ReturnPeriod);
        }

        [Test]
        public void ConstructorPassesUndefinedProbability()
        {
            var probabilityNaN = new Probability(double.NaN);
            Assert.IsFalse(probabilityNaN.IsDefined);
        }

        [Test]
        [TestCase(-0.2)]
        [TestCase(10.2)]
        public void ConstructorValidatesValue(double value)
        {
            Assert.Throws<AssemblyException>(() => { new Probability(value); });
        }

        [Test]
        public void OperatorsWork()
        {
            var precision = 1E-10;

            Assert.IsTrue(new Probability(0.1) == (Probability)0.1);
            Assert.IsFalse((Probability)0.01 == (Probability)0.1);
            Assert.IsTrue((Probability)0.01 != (Probability)0.1);
            Assert.IsFalse(new Probability(0.1) != (Probability)0.1);
            Assert.AreEqual((Probability)0.1, (Probability)0.2 - (Probability)0.1);
            Assert.AreEqual((Probability)0.3, (Probability)0.2 + (Probability)0.1,precision);
            Assert.AreEqual((Probability)0.01, (Probability)0.1 * (Probability)0.1, precision);
            Assert.AreEqual((Probability)0.01, (Probability)0.1 * 0.1, precision);
            Assert.AreEqual((Probability)0.01, 0.1 * (Probability)0.1, precision);
            Assert.AreEqual((Probability)0.2, (Probability)0.1 / (Probability)0.5, precision);
            Assert.AreEqual((Probability)0.05, (Probability)0.1 / 2.0, precision);
            Assert.AreEqual((Probability)0.2, 0.1 / (Probability)0.5, precision);
            Assert.AreEqual(0.3, (Probability)0.3, precision);

            Assert.AreEqual((Probability)1.0, (Probability)0.2 + (Probability)0.9, precision);
            Assert.AreEqual((Probability)0.0, (Probability)0.2 - (Probability)0.5, precision);

            Assert.IsTrue((Probability)0.2 > (Probability)0.1);
            Assert.IsFalse((Probability)0.01 > (Probability)0.1);
            Assert.IsTrue((Probability)0.2 >= (Probability)0.1);
            Assert.IsFalse((Probability)0.01 >= (Probability)0.1);
            Assert.IsTrue(new Probability(0.1) >= (Probability)0.1);
            Assert.IsTrue((Probability)0.2 > 0.1);
            Assert.IsFalse((Probability)0.01 > 0.1);
            Assert.IsTrue((Probability)0.2 >= 0.1);
            Assert.IsFalse((Probability)0.01 >= 0.1);
            Assert.IsTrue((Probability)0.1 >= 0.1);
            Assert.IsTrue(0.2 > (Probability)0.1);
            Assert.IsFalse(0.01 > (Probability)0.1);
            Assert.IsTrue(0.2 >= (Probability)0.1);
            Assert.IsFalse(0.01 >= (Probability)0.1);
            Assert.IsTrue(0.1 >= (Probability)0.1);

            Assert.IsFalse((Probability)0.2 < (Probability)0.1);
            Assert.IsTrue((Probability)0.01 < (Probability)0.1);
            Assert.IsFalse((Probability)0.2 <= (Probability)0.1);
            Assert.IsTrue((Probability)0.01 <= (Probability)0.1);
            Assert.IsTrue(new Probability(0.1) <= (Probability)0.1);
            Assert.IsFalse(0.2 < (Probability)0.1);
            Assert.IsTrue(0.01 < (Probability)0.1);
            Assert.IsFalse(0.2 <= (Probability)0.1);
            Assert.IsTrue(0.01 <= (Probability)0.1);
            Assert.IsTrue(0.1 <= (Probability)0.1);
            Assert.IsFalse((Probability)0.2 < 0.1);
            Assert.IsTrue((Probability)0.01 < 0.1);
            Assert.IsFalse((Probability)0.2 <= 0.1);
            Assert.IsTrue((Probability)0.01 <= 0.1);
            Assert.IsTrue((Probability)0.1 <= 0.1);
        }

        [Test]
        [TestCase(10.0,"*")]
        [TestCase(-10.0, "*")]
        [TestCase(-1.0, "*")]
        [TestCase(0.00001, "/")]
        [TestCase(-0.00001, "/")]
        [TestCase(-1.0, "/")]
        public void OperatorThrowsOnInvalidOutcome(double factor, string operation)
        {
            Action target;
            
            switch (operation)
            {
                case "*":
                    target = () =>
                    {
                        var a = (new Probability(0.5)) * factor;
                    };
                    break;
                case "/":
                    target = () =>
                    {
                        var a = (new Probability(0.5)) / factor;
                    };
                    break;
                default:
                    Assert.Fail("No valid test input");
                    return;
            }

            Assert.Throws<AssemblyException>(new TestDelegate(target));
        }
    }
}
