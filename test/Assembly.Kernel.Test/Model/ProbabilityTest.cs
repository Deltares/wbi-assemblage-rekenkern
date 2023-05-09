// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model
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
        public void EqualityOperator_TwoEqualProbabilities_ReturnsTrue()
        {
            // Setup
            var probability1 = new Probability(0.0101);
            var probability2 = new Probability(0.0101);

            // Call
            bool isEqual1 = probability1 == probability2;
            bool isEqual2 = probability2 == probability1;

            // Assert
            Assert.IsTrue(isEqual1);
            Assert.IsTrue(isEqual2);
        }

        [Test]
        public void EqualityOperator_TwoUnequalProbabilities_ReturnsFalse()
        {
            // Setup
            var probability1 = new Probability(0.0101);
            var probability2 = new Probability(0.01011);

            // Call
            bool isEqual1 = probability1 == probability2;
            bool isEqual2 = probability2 == probability1;

            // Assert
            Assert.IsFalse(isEqual1);
            Assert.IsFalse(isEqual2);
        }

        [Test]
        public void DoubleEqualityOperator_DoubleIsEqualToProbability_ReturnsTrue()
        {
            // Setup
            const double probabilityValue = 0.0101;
            var probability1 = new Probability(probabilityValue);

            // Call
            bool isEqual1 = probability1 == probabilityValue;
            bool isEqual2 = probabilityValue == probability1;

            // Assert
            Assert.IsTrue(isEqual1);
            Assert.IsTrue(isEqual2);
        }

        [Test]
        public void DoubleEqualityOperator_DoubleIsUnequalToProbability_ReturnsFalse()
        {
            // Setup
            const double probabilityValue = 0.0101;
            var probability1 = new Probability(0.065749);

            // Call
            bool isEqual1 = probability1 == probabilityValue;
            bool isEqual2 = probabilityValue == probability1;

            // Assert
            Assert.IsFalse(isEqual1);
            Assert.IsFalse(isEqual2);
        }

        [Test]
        public void InequalityOperator_TwoEqualProbabilities_ReturnsFalse()
        {
            // Setup
            var probability1 = new Probability(0.0101);
            var probability2 = new Probability(0.0101);

            // Call
            bool isEqual1 = probability1 != probability2;
            bool isEqual2 = probability2 != probability1;

            // Assert
            Assert.IsFalse(isEqual1);
            Assert.IsFalse(isEqual2);
        }

        [Test]
        public void InequalityOperator_TwoUnequalProbabilities_ReturnsTrue()
        {
            // Setup
            var probability1 = new Probability(0.0101);
            var probability2 = new Probability(0.01011);

            // Call
            bool isEqual1 = probability1 != probability2;
            bool isEqual2 = probability2 != probability1;

            // Assert
            Assert.IsTrue(isEqual1);
            Assert.IsTrue(isEqual2);
        }

        [Test]
        public void DoubleInequalityOperator_DoubleIsEqualToProbability_ReturnsFalse()
        {
            // Setup
            const double probabilityValue = 0.0101;
            var probability1 = new Probability(probabilityValue);

            // Call
            bool isEqual1 = probability1 != probabilityValue;
            bool isEqual2 = probabilityValue != probability1;

            // Assert
            Assert.IsFalse(isEqual1);
            Assert.IsFalse(isEqual2);
        }

        [Test]
        public void DoubleInequalityOperator_DoubleIsUnequalToProbability_ReturnsTrue()
        {
            // Setup
            const double probabilityValue = 0.0101;
            var probability1 = new Probability(0.065749);

            // Call
            bool isEqual1 = probability1 != probabilityValue;
            bool isEqual2 = probabilityValue != probability1;

            // Assert
            Assert.IsTrue(isEqual1);
            Assert.IsTrue(isEqual2);
        }

        [Test]
        public void OperatorMinus_OperationResultLessThanZero_ReturnsZero()
        {
            // Setup
            var probability1 = new Probability(0.123);
            var probability2 = new Probability(0.2);

            // Call
            Probability result = probability1 - probability2;

            // Assert
            Assert.AreEqual(0.0, result);
        }

        [Test]
        public void OperatorMinus_OperationResultGreaterThanZero_ReturnsResult()
        {
            // Setup
            var probability1 = new Probability(0.123);
            var probability2 = new Probability(0.2);

            // Call
            Probability result = probability2 - probability1;

            // Assert
            Assert.AreEqual(0.077, result, 1e-3);
        }

        [Test]
        public void OperatorPlus_OperationResultGreaterThenOne_ReturnsOne()
        {
            // Setup
            var probability1 = new Probability(0.123);
            var probability2 = new Probability(0.9);

            // Call
            Probability result1 = probability1 + probability2;
            Probability result2 = probability2 + probability1;

            // Assert
            Assert.AreEqual(1.0, result1);
            Assert.AreEqual(1.0, result2);
        }

        [Test]
        public void OperatorPlus_OperationResultLessThenOne_ReturnsResult()
        {
            // Setup
            var probability1 = new Probability(0.123);
            var probability2 = new Probability(0.2);

            // Call
            Probability result1 = probability1 + probability2;
            Probability result2 = probability2 + probability1;

            // Assert
            Assert.AreEqual(0.323, result1);
            Assert.AreEqual(0.323, result2);
        }

        [Test]
        public void OperatorTimes_OperationResultValid_ReturnsResult()
        {
            // Setup
            const double probabilityValue1 = 0.123;
            const double probabilityValue2 = 0.2;

            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            Probability result1 = probability1 * probability2;
            Probability result2 = probability2 * probability1;
            Probability result3 = probability1 * probabilityValue2;
            Probability result4 = probabilityValue2 * probability1;
            Probability result5 = probabilityValue1 * probability2;
            Probability result6 = probability2 * probabilityValue1;

            // Assert
            const double expectedResult = 0.0246;
            Assert.AreEqual(expectedResult, result1);
            Assert.AreEqual(expectedResult, result2);
            Assert.AreEqual(expectedResult, result3);
            Assert.AreEqual(expectedResult, result4);
            Assert.AreEqual(expectedResult, result5);
            Assert.AreEqual(expectedResult, result6);
        }

        [Test]
        [TestCase(10)]
        [TestCase(-1)]
        public void OperatorTimes_OperationResultInvalid_ThrowsAssemblyException(
            double probabilityValue2)
        {
            // Setup
            var probability = new Probability(0.123);

            // Call
            Probability result;
            void Call1() => result = probability * probabilityValue2;
            void Call2() => result = probabilityValue2 * probability;

            // Assert
            var expectedErrorMessages = new[]
            {
                new AssemblyErrorMessage("probabilityValue", EAssemblyErrors.FailureProbabilityOutOfRange)
            };
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call1, expectedErrorMessages);
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call2, expectedErrorMessages);
        }

        [Test]
        public void OperatorDivision_OperationResultValid_ReturnsResult()
        {
            // Setup
            const double probabilityValue1 = 0.05;
            const double probabilityValue2 = 0.2;
            const double probabilityValue3 = 0.4;
            const double probabilityValue4 = 0.09;

            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);
            var probability3 = new Probability(probabilityValue3);
            var probability4 = new Probability(probabilityValue4);

            // Call
            Probability result1 = probability1 / probability2;
            Probability result2 = probability4 / probability3;
            Probability result3 = probability1 / probabilityValue2;
            Probability result4 = probabilityValue4 / probability3;

            // Assert
            const double expectedResult1 = 0.25;
            const double expectedResult2 = 0.225;
            Assert.AreEqual(expectedResult1, result1, 1e-3);
            Assert.AreEqual(expectedResult2, result2, 1e-3);
            Assert.AreEqual(expectedResult1, result3, 1e-3);
            Assert.AreEqual(expectedResult2, result4, 1e-3);
        }

        [Test]
        public void OperatorDivision_OperationResultInvalid_ThrowsAssemblyException()
        {
            // Setup
            const double probabilityValue = 0.1;
            var probability = new Probability(1.0);

            // Call
            Probability result;
            void Call1() => result = probability / new Probability(probabilityValue);
            void Call2() => result = probability / probabilityValue;
            void Call3() => result = -probabilityValue / probability;

            // Assert
            var expectedErrorMessages = new[]
            {
                new AssemblyErrorMessage("probabilityValue", EAssemblyErrors.FailureProbabilityOutOfRange)
            };
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call1, expectedErrorMessages);
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call2, expectedErrorMessages);
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call3, expectedErrorMessages);
        }

        [Test]
        public void OperatorLessThan_LeftLessThanRight_ReturnsTrue()
        {
            // Setup
            const double probabilityValue1 = 0.8;
            const double probabilityValue2 = 0.8192378;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 < probability2;
            bool isLessThan2 = probability1 < probabilityValue2;

            // Assert
            Assert.IsTrue(isLessThan1);
            Assert.IsTrue(isLessThan2);
        }

        [Test]
        public void OperatorLessThan_LeftGreaterThanRight_ReturnsFalse()
        {
            // Setup
            const double probabilityValue1 = 0.8192378;
            const double probabilityValue2 = 0.8;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 < probability2;
            bool isLessThan2 = probability1 < probabilityValue2;

            // Assert
            Assert.IsFalse(isLessThan1);
            Assert.IsFalse(isLessThan2);
        }

        [Test]
        public void OperatorLessThanOrEqualTo_LeftLessThanOrEqualRight_ReturnsTrue()
        {
            // Setup
            const double probabilityValue1 = 0.8;
            const double probabilityValue2 = 0.8192378;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 <= probability2;
            bool isLessThan2 = probability1 <= probabilityValue2;
            bool isLessThan3 = probability1 <= probability1;
            bool isLessThan4 = probability1 <= probabilityValue1;
            bool isLessThan5 = probability2 <= probability2;
            bool isLessThan6 = probability2 <= probabilityValue2;

            // Assert
            Assert.IsTrue(isLessThan1);
            Assert.IsTrue(isLessThan2);
            Assert.IsTrue(isLessThan3);
            Assert.IsTrue(isLessThan4);
            Assert.IsTrue(isLessThan5);
            Assert.IsTrue(isLessThan6);
        }

        [Test]
        public void OperatorLessThanOrEqualTo_LeftGreaterThanRight_ReturnsFalse()
        {
            // Setup
            const double probabilityValue1 = 0.8192378;
            const double probabilityValue2 = 0.8;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 <= probability2;
            bool isLessThan2 = probability1 <= probabilityValue2;

            // Assert
            Assert.IsFalse(isLessThan1);
            Assert.IsFalse(isLessThan2);
        }

        [Test]
        public void OperatorGreaterThan_LeftGreaterThanRight_ReturnsTrue()
        {
            // Setup
            const double probabilityValue1 = 0.8192378;
            const double probabilityValue2 = 0.8;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 > probability2;
            bool isLessThan2 = probability1 > probabilityValue2;

            // Assert
            Assert.IsTrue(isLessThan1);
            Assert.IsTrue(isLessThan2);
        }

        [Test]
        public void OperatorGreaterThan_LeftLessThanRight_ReturnsFalse()
        {
            // Setup
            const double probabilityValue1 = 0.8;
            const double probabilityValue2 = 0.8192378;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 > probability2;
            bool isLessThan2 = probability1 > probabilityValue2;

            // Assert
            Assert.IsFalse(isLessThan1);
            Assert.IsFalse(isLessThan2);
        }

        [Test]
        public void OperatorGreaterThanOrEqualTo_LeftGreaterThanOrEqualRight_ReturnsTrue()
        {
            // Setup
            const double probabilityValue1 = 0.8192378;
            const double probabilityValue2 = 0.8;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 >= probability2;
            bool isLessThan2 = probability1 >= probabilityValue2;
            bool isLessThan3 = probability1 >= probability1;
            bool isLessThan4 = probability1 >= probabilityValue1;
            bool isLessThan5 = probability2 >= probability2;
            bool isLessThan6 = probability2 >= probabilityValue2;

            // Assert
            Assert.IsTrue(isLessThan1);
            Assert.IsTrue(isLessThan2);
            Assert.IsTrue(isLessThan3);
            Assert.IsTrue(isLessThan4);
            Assert.IsTrue(isLessThan5);
            Assert.IsTrue(isLessThan6);
        }

        [Test]
        public void OperatorGreaterThanOrEqualTo_LeftLessThanRight_ReturnsFalse()
        {
            // Setup
            const double probabilityValue1 = 0.8;
            const double probabilityValue2 = 0.8192378;
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            bool isLessThan1 = probability1 >= probability2;
            bool isLessThan2 = probability1 >= probabilityValue2;

            // Assert
            Assert.IsFalse(isLessThan1);
            Assert.IsFalse(isLessThan2);
        }

        [Test]
        public void CompareTo_Null_ReturnsExpectedResult()
        {
            // Setup
            var probability = new Probability(0.1);

            // Call
            int result = probability.CompareTo(null);

            // Assert
            Assert.AreEqual(1, result);
        }

        [Test]
        public void CompareTo_Object_ThrowsAssemblyException()
        {
            // Setup
            var probability = new Probability(0.1);

            // Call
            void Call() => probability.CompareTo(new object());

            // Assert
            var exception = Assert.Throws<ArgumentException>(Call);
            Assert.AreEqual("obj must be Double or Probability", exception.Message);
        }

        [Test]
        public void CompareTo_Itself_ReturnsZero()
        {
            // Setup
            var probability = new Probability(0.1);

            // Call
            int result = probability.CompareTo(probability);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        [TestCaseSource(nameof(GetCompareToCases))]
        public void CompareTo_ProbabilityToDouble_ReturnsExpectedResult(
            double probabilityValue, double value, int expectedProbabilityIndex)
        {
            // Setup
            var probability = new Probability(probabilityValue);

            // Call
            int probabilityResult = probability.CompareTo(value);
            int doubleResult = value.CompareTo(probability);

            // Assert
            Assert.AreEqual(expectedProbabilityIndex, probabilityResult);
            Assert.AreEqual(-1 * expectedProbabilityIndex, doubleResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCompareToCases))]
        public void CompareTo_ProbabilityToToImplicitDouble_ReturnsExpectedResult(
            double probabilityValue, double value, int expectedProbabilityIndex)
        {
            // Setup
            var probability = new Probability(probabilityValue);

            // Call
            int probabilityResult = probability.CompareTo((object) value);

            // Assert
            Assert.AreEqual(expectedProbabilityIndex, probabilityResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCompareToCases))]
        public void CompareTo_ProbabilityToProbability_ReturnsExpectedResult(
            double probabilityValue1, double probabilityValue2, int expectedProbabilityIndex)
        {
            // Setup
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            int probabilityResult1 = probability1.CompareTo(probability2);
            int probabilityResult2 = probability2.CompareTo(probability1);

            // Assert
            Assert.AreEqual(expectedProbabilityIndex, probabilityResult1);
            Assert.AreEqual(-1 * expectedProbabilityIndex, probabilityResult2);
        }

        [Test]
        [TestCaseSource(nameof(GetCompareToCases))]
        public void CompareTo_ProbabilityToImplicitProbability_ReturnsExpectedResult(
            double probabilityValue1, double probabilityValue2, int expectedProbabilityIndex)
        {
            // Setup
            var probability1 = new Probability(probabilityValue1);
            var probability2 = new Probability(probabilityValue2);

            // Call
            int probabilityResult1 = probability1.CompareTo((object) probability2);
            int probabilityResult2 = probability2.CompareTo((object) probability1);

            // Assert
            Assert.AreEqual(expectedProbabilityIndex, probabilityResult1);
            Assert.AreEqual(-1 * expectedProbabilityIndex, probabilityResult2);
        }

        [Test]
        public void Equals_ToNull_ReturnsFalse()
        {
            // Setup
            var probability = new Probability(0.1);

            // Call
            bool isEqual = probability.Equals(null);

            // Assert
            Assert.IsFalse(isEqual);
        }

        [Test]
        public void Equals_ToSameInstance_ReturnsTrue()
        {
            // Setup
            var probability = new Probability(0.1);

            // Call
            bool isEqual = probability.Equals(probability);

            // Assert
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void Equals_ToSameObjectInstance_ReturnsTrue()
        {
            // Setup
            var probability = new Probability(0.1);

            // Call
            bool isEqual = probability.Equals((object) probability);

            // Assert
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void Equals_ObjectOfSomeOtherType_ReturnsFalse()
        {
            // Setup
            var probability = new Probability(0.1);
            var someOtherObject = new object();

            // Call
            bool isEqual1 = probability.Equals(someOtherObject);
            bool isEqual2 = someOtherObject.Equals(probability);

            // Assert
            Assert.IsFalse(isEqual1);
            Assert.IsFalse(isEqual2);
        }

        [Test]
        public void Equals_ToOtherEqualProbability_ReturnsTrue()
        {
            // Setup
            const double probabilityValue = 0.0012345;
            var baseProbability = new Probability(probabilityValue);
            object comparisonProbability = new Probability(probabilityValue);

            // Call
            bool isEqual1 = baseProbability.Equals(comparisonProbability);
            bool isEqual2 = comparisonProbability.Equals(baseProbability);

            // Assert
            Assert.IsTrue(isEqual1);
            Assert.IsTrue(isEqual2);
        }

        [Test]
        public void Equals_ProbabilityEqualToDouble_ReturnsTrue()
        {
            // Setup
            const double probabilityValue = 0.0012345;
            var probability = new Probability(probabilityValue);

            // Call
            bool isEqual1 = probability.Equals(probabilityValue);
            bool isEqual2 = probabilityValue.Equals(probability);

            // Assert
            Assert.IsTrue(isEqual1);
            Assert.IsTrue(isEqual2);
        }

        [Test]
        public void Equals_ProbabilityNotEqualToDouble_ReturnsFalse()
        {
            // Setup
            var probability = new Probability(0.0012345);
            const double otherValue = 0.0017362;

            // Call
            bool isEqual1 = probability.Equals(otherValue);
            bool isEqual2 = otherValue.Equals(probability);

            // Assert
            Assert.IsFalse(isEqual1);
            Assert.IsFalse(isEqual2);
        }

        [Test]
        public void GetHashCode_TwoEqualInstances_ReturnsSameHash()
        {
            // Setup
            const double probabilityValue = 0.0012345;
            var baseProbability = new Probability(probabilityValue);
            object comparisonProbability = new Probability(probabilityValue);

            // Call
            int hash1 = baseProbability.GetHashCode();
            int hash2 = comparisonProbability.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        [Test]
        public void GetHashCode_ProbabilityEqualToDouble_ReturnsSameHashCode()
        {
            // Setup
            const double probabilityValue = 0.0012345;
            var probability = new Probability(probabilityValue);

            // Call
            int hash1 = probability.GetHashCode();
            int hash2 = probabilityValue.GetHashCode();

            // Assert
            Assert.AreEqual(hash1, hash2);
        }

        private static IEnumerable<TestCaseData> GetCompareToCases()
        {
            yield return new TestCaseData(1.0, 1.0, 0);
            yield return new TestCaseData(1.0, 0.000009, 1);
            yield return new TestCaseData(0.000009, 1.0, -1);
            yield return new TestCaseData(double.NaN, 1.0, -1);
            yield return new TestCaseData(1.0, double.NaN, 1);
            yield return new TestCaseData(double.NaN, double.NaN, 0);
        }
    }
}