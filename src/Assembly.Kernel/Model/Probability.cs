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
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// This struct represents a probability. It can be used similar to double, but has a limited
    /// value within the range [0,1].
    /// </summary>
    public struct Probability : IEquatable<Probability>, IEquatable<double>,
                                IComparable, IComparable<Probability>, IComparable<double>
    {
        /// <summary>
        /// Represents an undefined probability.
        /// </summary>
        public static readonly Probability Undefined = new Probability(double.NaN);

        private readonly double value;

        /// <summary>
        /// Creates a new instance of <see cref="Probability"/>.
        /// </summary>
        /// <param name="probabilityValue">The value of the probability.</param>
        /// <exception cref="AssemblyException">Thrown when <paramref name="probabilityValue"/> 
        /// is not in range [0.0, 1.0].</exception>
        public Probability(double probabilityValue)
        {
            if (probabilityValue < 0.0 || probabilityValue > 1.0)
            {
                throw new AssemblyException(nameof(probabilityValue), EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            value = probabilityValue;
        }

        /// <summary>
        /// Gets a probability that represents the inverse value of this probability.
        /// </summary>
        public Probability Inverse => new Probability(1 - value);

        /// <summary>
        /// Gets whether the probability is defined.
        /// </summary>
        public bool IsDefined => !double.IsNaN(value);

        /// <summary>
        /// Gets whether the difference between two probabilities is negligible based on their reliability indices.
        /// </summary>
        /// <param name="other">The probability to compare with.</param>
        /// <param name="maximumRelativeDifference">The maximum allowed relative difference.</param>
        /// <returns><c>true</c> in case there is a negligible difference with the specified other probability;
        /// <c>false</c> otherwise.</returns>
        public bool IsNegligibleDifference(Probability other, double maximumRelativeDifference = 1e-6)
        {
            double average = (this + (double) other) * 0.5;
            double absoluteDifference = Math.Abs(this - (double) other);
            double relativeDifference = absoluteDifference / average;

            return double.IsNaN(relativeDifference) || relativeDifference <= maximumRelativeDifference;
        }

        #region Operators

        /// <summary>
        /// Specifies the == operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>A bool indicating whether the two specified <see cref="Probability"/> are equal.</returns>
        public static bool operator ==(Probability left, Probability right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Specifies the != operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>A bool indicating whether the two specified <see cref="Probability"/> are unequal.</returns>
        public static bool operator !=(Probability left, Probability right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Specifies the - operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        public static Probability operator -(Probability left, Probability right)
        {
            return new Probability(Math.Max(0, left.value - right.value));
        }

        /// <summary>
        /// Specifies the + operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        public static Probability operator +(Probability left, Probability right)
        {
            return new Probability(Math.Min(1, left.value + right.value));
        }

        /// <summary>
        /// Specifies the * operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="AssemblyException">Thrown in case the resulting probability &lt; 0 or &gt;1.</exception>
        public static Probability operator *(Probability left, double right)
        {
            return new Probability(left.value * right);
        }

        /// <summary>
        /// Specifies the * operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="AssemblyException">Thrown in case the resulting probability &lt; 0 or &gt;1.</exception>
        public static Probability operator *(double left, Probability right)
        {
            return new Probability(left * right.value);
        }

        /// <summary>
        /// Specifies the * operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        public static Probability operator *(Probability left, Probability right)
        {
            return new Probability(left.value * right.value);
        }

        /// <summary>
        /// Specifies the / operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="AssemblyException">Thrown in case the resulting probability &lt; 0 or &gt;1.</exception>
        public static Probability operator /(Probability left, double right)
        {
            return new Probability(left.value / right);
        }

        /// <summary>
        /// Specifies the / operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="AssemblyException">Thrown in case the resulting probability &lt; 0 or &gt;1.</exception>
        public static Probability operator /(double left, Probability right)
        {
            return new Probability(left / right.value);
        }

        /// <summary>
        /// Specifies the / operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        /// <exception cref="AssemblyException">Thrown in case the resulting probability &lt; 0 or &gt;1.</exception>
        public static Probability operator /(Probability left, Probability right)
        {
            return new Probability(left.value / right.value);
        }

        /// <summary>
        /// Specifies the $lt; operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left $lt; right.</returns>
        public static bool operator <(Probability left, Probability right)
        {
            return left.value < right.value;
        }

        /// <summary>
        /// Specifies the $lt; operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left $lt; right.</returns>
        public static bool operator <(Probability left, double right)
        {
            return left.value < right;
        }

        /// <summary>
        /// Specifies the &lt;= operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left is equal to or $lt; right.</returns>
        public static bool operator <=(Probability left, Probability right)
        {
            return left.value <= right.value;
        }

        /// <summary>
        /// Specifies the &lt;= operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left is equal to or $lt; right.</returns>
        public static bool operator <=(Probability left, double right)
        {
            return left.value <= right;
        }

        /// <summary>
        /// Specifies the > operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left $gt; right.</returns>
        public static bool operator >(Probability left, Probability right)
        {
            return left.value > right.value;
        }

        /// <summary>
        /// Specifies the > operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left $gt; right.</returns>
        public static bool operator >(Probability left, double right)
        {
            return left.value > right;
        }

        /// <summary>
        /// Specifies the >= operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left is equal to or $gt; right.</returns>
        public static bool operator >=(Probability left, Probability right)
        {
            return left.value >= right.value;
        }

        /// <summary>
        /// Specifies the >= operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left is equal to or $gt; right.</returns>
        public static bool operator >=(Probability left, double right)
        {
            return left.value >= right;
        }

        /// <summary>
        /// Facilitates implicit conversion between <see cref="Probability"/> and double.
        /// </summary>
        /// <param name="d">The <see cref="Probability"/> to convert from.</param>
        public static implicit operator double(Probability d)
        {
            return d.value;
        }

        /// <summary>
        /// Facilitates explicit conversion between double and <see cref="Probability"/>.
        /// </summary>
        /// <param name="d">The double to convert from.</param>
        /// <exception cref="AssemblyException">Thrown in case the probability &lt; 0 or &gt;1.</exception>
        public static explicit operator Probability(double d)
        {
            return new Probability(d);
        }

        #endregion

        #region CompareTo

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is Probability probability)
            {
                return CompareTo(probability);
            }

            if (obj is double d)
            {
                return CompareTo(d);
            }

            throw new ArgumentException($"{nameof(obj)} must be {nameof(Double)} or {nameof(Probability)}");
        }

        /// <inheritdoc />
        public int CompareTo(double other)
        {
            return value.CompareTo(other);
        }

        /// <inheritdoc />
        public int CompareTo(Probability other)
        {
            return value.CompareTo(other.value);
        }

        #endregion

        #region Equals

        /// <inheritdoc />
        public bool Equals(double other)
        {
            return value.Equals(other);
        }

        /// <inheritdoc />
        public bool Equals(Probability other)
        {
            return other.value.Equals(value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Probability) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion
    }
}