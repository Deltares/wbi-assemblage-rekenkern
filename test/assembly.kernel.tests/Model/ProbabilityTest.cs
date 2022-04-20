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

using System;
using System.Globalization;
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

        [TestCase(0,0,true)]
        [TestCase(0, 0.2, false)]
        [TestCase(0.2, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(0.001, 0.001 + 1E-40, true)]
        [TestCase(0.001, 0.001 + 1E-8, false)]
        [TestCase(2E-40, 2E-40, true)]
        [TestCase(2E-10, 3E-10, false)]
        public void IsNegligibleDifferenceWorks(double x, double y, bool expectedResult)
        {
            var probabilityX = (Probability) x;
            var probabilityY = (Probability)y;

            Assert.AreEqual(expectedResult, probabilityX.IsNegligibleDifference(probabilityY));
            Assert.AreEqual(expectedResult, probabilityY.IsNegligibleDifference(probabilityX));
        }

        [TestCase(1E-3, true)]
        [TestCase(1E-4, false)]
        public void IsNegligibleDifferenceTakesPrecision(double precision, bool expectedResult)
        {
            var probability = new Probability(1E-20);
            var other = new Probability(1.001E-20);

            Assert.AreEqual(expectedResult, probability.IsNegligibleDifference(other, precision));
        }

        [Test]
        public void InverseWorks()
        {
            var probability = new Probability(0.9);
            Assert.AreEqual(0.1, probability.Inverse,1E-10);
        } 

        [Test]
        public void IsDefinedReturnsCorrectValue()
        {
            Assert.IsTrue(new Probability(0.2).IsDefined);
            Assert.IsFalse(new Probability(double.NaN).IsDefined);
            Assert.IsFalse(((Probability)double.NaN).IsDefined);
            Assert.IsFalse(Probability.Undefined.IsDefined);
        }

        [TestCase(0.0002516, "1/3975")]
        [TestCase(double.NaN, "Undefined")]
        [TestCase(1E-101, "0")]
        [TestCase(1-1E-101, "1")]
        public void ToStringWorks(double value, string expectedString)
        {
            var probability = new Probability(value);
            Assert.AreEqual(expectedString, probability.ToString());
        }

        [Test]
        public void ToStringWorksWithFormatProvider()
        {
            var probability = new Probability(1.425E-15);
            Assert.AreEqual("1/701754385964912", probability.ToString(CultureInfo.InvariantCulture));
        }

        [Test]
        public void ToStringWorksWithFormat()
        {
            var probability = new Probability(1.425E-15);
            Assert.AreEqual("1.42500E-015", probability.ToString("E5", CultureInfo.InvariantCulture));
        }

        [Test]
        public void ConstructorPassesUndefinedProbability()
        {
            var undefinedProbability = new Probability(double.NaN);
            Assert.IsFalse(undefinedProbability.IsDefined);
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

        [Test]
        public void EqualsReturnsExpectedValue()
        {
            var probability = new Probability(0.1);

            Assert.IsFalse(probability.Equals(null));
            Assert.IsFalse(probability.Equals(3));
            Assert.IsFalse(probability.Equals("string"));
            Assert.IsTrue(probability.Equals(probability));
            Assert.IsTrue(probability.Equals(new Probability(0.1)));
        }

        [Test]
        public void GetHashCodeReturnsHashCodeUnderlyingDouble()
        {
            var probabilityValue = 0.0012345;
            var probability = new Probability(probabilityValue);
            Assert.AreEqual(probabilityValue.GetHashCode(), probability.GetHashCode());
        }

        [Test]
        public void CompareToThrowsOnInvalidInput()
        {
            var probability = new Probability(0.31);

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var compareTo = probability.CompareTo(null);
            }, EAssemblyErrors.ValueMayNotBeNull);

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var compareTo = probability.CompareTo("string");
            }, EAssemblyErrors.InvalidArgumentType);
        }

        [TestCase(0.2, 1)]
        [TestCase(0.3, 0)]
        [TestCase(0.4, -1)]
        public void CompareToReturnsCorrectValues(double inputValue, int expectedResult)
        {
            var probability = new Probability(0.3);
            Assert.AreEqual(expectedResult,probability.CompareTo(inputValue));
            Assert.AreEqual(expectedResult, probability.CompareTo((Probability)inputValue));
            Assert.AreEqual(expectedResult, probability.CompareTo((object)inputValue)); 
            Assert.AreEqual(expectedResult, probability.CompareTo((object)(Probability)inputValue));
        }
    }
}
