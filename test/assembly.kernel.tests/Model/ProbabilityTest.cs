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
        [TestCase(-0.000001)]
        [TestCase(1.000001)]
        public void Constructor_InvalidValue_ThrowsAssemblyException(double value)
        {
            // Call
            void Call() => new Probability(value);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("probabilityValue", EAssemblyErrors.FailureProbabilityOutOfRange)
            });
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            const double value = 0.5;

            // Call
            var probability = new Probability(value);

            // Assert
            Assert.IsInstanceOf<IEquatable<Probability>>(probability);
            Assert.IsInstanceOf<IEquatable<double>>(probability);
            Assert.IsInstanceOf<IComparable>(probability);
            Assert.IsInstanceOf<IComparable<Probability>>(probability);
            Assert.IsInstanceOf<IComparable<double>>(probability);
            Assert.IsInstanceOf<IFormattable>(probability);

            Assert.AreEqual(value, probability, 1e-6);
        }

        [Test]
        public void Undefined_Always_ReturnsExpectedValue()
        {
            // Call
            Probability probability = Probability.Undefined;

            // Assert
            Assert.IsNaN(probability);
        }

        [Test]
        public void Inverse_ProbabilityUndefined_ReturnsExpectedValue()
        {
            // Setup
            Probability probability = Probability.Undefined;

            // Call
            Probability inverse = probability.Inverse;

            // Assert
            Assert.AreNotSame(probability, inverse);
            Assert.IsNaN(inverse);
        }

        [Test]
        [TestCase(0.178, 0.822)]
        [TestCase(0.9826, 0.0174)]
        public void Inverse_ProbabilityDefined_ReturnsExpectedValue(double value, double expectedInverse)
        {
            // Setup
            var probability = new Probability(value);

            // Call
            Probability inverse = probability.Inverse;

            // Assert
            Assert.AreNotSame(probability, inverse);
            Assert.AreEqual(expectedInverse, inverse, 1e-6);
        }

        [Test]
        [TestCase(0.3, true)]
        [TestCase(double.NaN, false)]
        public void IsDefined_Always_ReturnsExpectedValue(double value, bool expectedIsDefined)
        {
            // Setup
            var probability = new Probability(value);

            // Call
            bool isDefined = probability.IsDefined;

            // Assert
            Assert.AreEqual(expectedIsDefined, isDefined);
        }
        
        [Test]
        [TestCase(0, 0, true)]
        [TestCase(0, 0.2, false)]
        [TestCase(0.2, 0, false)]
        [TestCase(1, 1, true)]
        [TestCase(0.001, 0.001 + 1e-40, true)]
        [TestCase(0.001, 0.001 + 1e-8, false)]
        [TestCase(2e-40, 2e-40, true)]
        [TestCase(2e-10, 3e-10, false)]
        public void IsNegligibleDifference_DefaultPrecision_ReturnsExpectedResult(double value1, double value2, bool expectedResult)
        {
            // Setup
            var probability1 = new Probability(value1);
            var probability2 = new Probability(value2);

            // Call
            bool isNegligibleDifference1 = probability1.IsNegligibleDifference(probability2);
            bool isNegligibleDifference2 = probability2.IsNegligibleDifference(probability1);
            
            // Assert
            Assert.AreEqual(expectedResult, isNegligibleDifference1);
            Assert.AreEqual(expectedResult, isNegligibleDifference2);
        }

        [Test]
        [TestCase(1e-3, true)]
        [TestCase(1e-4, false)]
        public void IsNegligibleDifference_DifferentPrecisions_ReturnsExpectedResult(double precision, bool expectedResult)
        {
            // Setup
            var probability = new Probability(1e-20);
            var other = new Probability(1.001e-20);

            // Call
            bool isNegligibleDifference = probability.IsNegligibleDifference(other, precision);
            
            // Assert
            Assert.AreEqual(expectedResult, isNegligibleDifference);
        }

        [Test]
        [SetUICulture("nl-NL")]
        [SetCulture("nl-NL")]
        public void ToStringWorksWithFormatProvider()
        {
            var probability = new Probability(1.425e-15);

            Assert.AreEqual("1.4250E-015", probability.ToString("E4", CultureInfo.InvariantCulture));
            Assert.AreEqual("1.4250E-015", probability.ToString("E4", new CultureInfo("en-US")));
            Assert.AreEqual("1,4250E-015", probability.ToString("E4", new CultureInfo("nl-NL")));
        }

        [Test]
        public void ToStringWorksWithFormat()
        {
            var probability = new Probability(1.425e-15);
            Assert.AreEqual("1.42500E-015", probability.ToString("E5", CultureInfo.InvariantCulture));
        }

        [Test]
        public void OperatorsWork()
        {
            const double precision = 1e-10;

            Assert.IsTrue(new Probability(0.1) == (Probability) 0.1);
            Assert.IsFalse((Probability) 0.01 == (Probability) 0.1);
            Assert.IsTrue((Probability) 0.01 != (Probability) 0.1);
            Assert.IsFalse(new Probability(0.1) != (Probability) 0.1);
            Assert.AreEqual((Probability) 0.1, (Probability) 0.2 - (Probability) 0.1);
            Assert.AreEqual((Probability) 0.3, (Probability) 0.2 + (Probability) 0.1, precision);
            Assert.AreEqual((Probability) 0.01, (Probability) 0.1 * (Probability) 0.1, precision);
            Assert.AreEqual((Probability) 0.01, (Probability) 0.1 * 0.1, precision);
            Assert.AreEqual((Probability) 0.01, 0.1 * (Probability) 0.1, precision);
            Assert.AreEqual((Probability) 0.2, (Probability) 0.1 / (Probability) 0.5, precision);
            Assert.AreEqual((Probability) 0.05, (Probability) 0.1 / 2.0, precision);
            Assert.AreEqual((Probability) 0.2, 0.1 / (Probability) 0.5, precision);
            Assert.AreEqual(0.3, (Probability) 0.3, precision);

            Assert.AreEqual((Probability) 1.0, (Probability) 0.2 + (Probability) 0.9, precision);
            Assert.AreEqual((Probability) 0.0, (Probability) 0.2 - (Probability) 0.5, precision);

            Assert.IsTrue((Probability) 0.2 > (Probability) 0.1);
            Assert.IsFalse((Probability) 0.01 > (Probability) 0.1);
            Assert.IsTrue((Probability) 0.2 >= (Probability) 0.1);
            Assert.IsFalse((Probability) 0.01 >= (Probability) 0.1);
            Assert.IsTrue(new Probability(0.1) >= (Probability) 0.1);
            Assert.IsTrue((Probability) 0.2 > 0.1);
            Assert.IsFalse((Probability) 0.01 > 0.1);
            Assert.IsTrue((Probability) 0.2 >= 0.1);
            Assert.IsFalse((Probability) 0.01 >= 0.1);
            Assert.IsTrue((Probability) 0.1 >= 0.1);
            Assert.IsTrue(0.2 > (Probability) 0.1);
            Assert.IsFalse(0.01 > (Probability) 0.1);
            Assert.IsTrue(0.2 >= (Probability) 0.1);
            Assert.IsFalse(0.01 >= (Probability) 0.1);
            Assert.IsTrue(0.1 >= (Probability) 0.1);

            Assert.IsFalse((Probability) 0.2 < (Probability) 0.1);
            Assert.IsTrue((Probability) 0.01 < (Probability) 0.1);
            Assert.IsFalse((Probability) 0.2 <= (Probability) 0.1);
            Assert.IsTrue((Probability) 0.01 <= (Probability) 0.1);
            Assert.IsTrue(new Probability(0.1) <= (Probability) 0.1);
            Assert.IsFalse(0.2 < (Probability) 0.1);
            Assert.IsTrue(0.01 < (Probability) 0.1);
            Assert.IsFalse(0.2 <= (Probability) 0.1);
            Assert.IsTrue(0.01 <= (Probability) 0.1);
            Assert.IsTrue(0.1 <= (Probability) 0.1);
            Assert.IsFalse((Probability) 0.2 < 0.1);
            Assert.IsTrue((Probability) 0.01 < 0.1);
            Assert.IsFalse((Probability) 0.2 <= 0.1);
            Assert.IsTrue((Probability) 0.01 <= 0.1);
            Assert.IsTrue((Probability) 0.1 <= 0.1);
        }

        [Test]
        [TestCase(10.0, "*")]
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
                        var a = new Probability(0.5) * factor;
                    };
                    break;
                case "/":
                    target = () =>
                    {
                        var a = new Probability(0.5) / factor;
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

            Assert.IsFalse(probability.Equals(0.4 - 0.3));
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

        [TestCase(0.0002516, "1/3975")]
        [TestCase(double.NaN, "Undefined")]
        [TestCase(1e-101, "0")]
        [TestCase(1 - 1e-101, "1")]
        public void ToStringWorks(double value, string expectedString)
        {
            var probability = new Probability(value);
            Assert.AreEqual(expectedString, probability.ToString());
        }

        [TestCase(0.2, 1)]
        [TestCase(0.3, 0)]
        [TestCase(0.4, -1)]
        public void CompareToReturnsCorrectValues(double inputValue, int expectedResult)
        {
            var probability = new Probability(0.3);
            Assert.AreEqual(expectedResult, probability.CompareTo(inputValue));
            Assert.AreEqual(expectedResult, probability.CompareTo((Probability) inputValue));
            Assert.AreEqual(expectedResult, probability.CompareTo((object) inputValue));
            Assert.AreEqual(expectedResult, probability.CompareTo((object) (Probability) inputValue));
        }
    }
}