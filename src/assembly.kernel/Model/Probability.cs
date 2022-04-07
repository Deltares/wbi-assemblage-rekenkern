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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// This struct represents a probability. It can be used similar to double, but has a limited
    /// value within the range [0,1].
    /// </summary>
    public struct Probability : IEquatable<Probability>, IEquatable<double>, IFormattable, IComparable, IComparable<Probability>, IComparable<double>
    {
        private static readonly double ToStringPrecision = 1e-100;
        private readonly double value;

        /// <summary>
        /// Represents an undefined probability.
        /// </summary>
        public static readonly Probability Undefined = new Probability(double.NaN);

        /// <summary>
        /// Constructs a <see cref="Probability"/> from a double representing the probability value.
        /// </summary>
        /// <param name="probabilityValue">The value of the probability.</param>
        /// <exception cref="AssemblyException">Thrown whenever <paramref name="probabilityValue"/>&lt;0 or <paramref name="probabilityValue"/>&gt;1 .</exception>
        public Probability(double probabilityValue)
        {
            ValidateProbabilityValueWithinAllowedRange(probabilityValue);
            value = probabilityValue;
        }

        /// <summary>
        /// Represents the return period of this probability.
        /// </summary>
        public int ReturnPeriod => (int) Math.Round(1 / value);

        /// <summary>
        /// Returns a new probability that represents the inverse value of this probability (1-probability).
        /// </summary>
        public Probability Inverse => new Probability(1-value);

        /// <summary>
        /// Specifies whether the probability is defined.
        /// </summary>
        public bool IsDefined => !double.IsNaN(value);

        /// <summary>
        /// Returns whether the difference between two probabilities is negligible based on their reliability indices.
        /// </summary>
        /// <param name="other">The probability to compare with.</param>
        /// <param name="maximumRelativeDifference">The maximum allowed relative difference.</param>
        /// <returns></returns>
        public bool IsNegligibleDifference(Probability other, double maximumRelativeDifference = 1E-6)
        {
            var average = ((double)this + (double)other) * 0.5;
            var absoluteDifference = Math.Abs((double) this - (double) other);
            var relativeDifference = absoluteDifference / average;
            
            return !(relativeDifference < double.PositiveInfinity) | relativeDifference <= maximumRelativeDifference;
        }

        /// <summary>
        /// Validates <paramref name="probability"/> for being a valid probability. This means a double within the range [0-1].
        /// </summary>
        /// <param name="probability">The probability to validate</param>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="probability"/> is smaller than 0</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="probability"/> exceeds 1</exception>
        private static void ValidateProbabilityValueWithinAllowedRange(double probability)
        {
            if (!double.IsNaN(probability) && (probability < 0 || probability > 1))
            {
                throw new AssemblyException(nameof(probability),EAssemblyErrors.FailureProbabilityOutOfRange);
            }
        }

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
            return new Probability(Math.Max(0,left.value - right.value));
        }

        /// <summary>
        /// Specifies the + operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
        public static Probability operator +(Probability left, Probability right)
        {
            return new Probability(Math.Min(1,left.value + right.value));
        }

        /// <summary>
        /// Specifies the * operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>The result of the operation.</returns>
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
        public static Probability operator /(Probability left, Probability right)
        {
            return new Probability(left.value / right.value);
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
        public static explicit operator Probability(double d)
        {
            return new Probability(d);
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
        /// Specifies the <= operator.
        /// </summary>
        /// <param name="left">The probability on the left side of the sign.</param>
        /// <param name="right">The probability on the right side of the sign.</param>
        /// <returns>Whether left is equal to or $lt; right.</returns>
        public static bool operator <=(Probability left, Probability right)
        {
            return left.value <= right.value;
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
        /// Specifies the <= operator.
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
        public static bool operator >=(Probability left, double right)
        {
            return left.value >= right;
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
            return Equals((Probability)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// Check equality of the probability with the specified double.
        /// </summary>
        /// <param name="other">The probability value to compare with.</param>
        /// <returns>A boolean indicating whether the values are equal.</returns>
        public bool Equals(Probability other)
        {
            return other.value.Equals(value);
        }

        /// <summary>
        /// Check equality of the probability with the specified double.
        /// </summary>
        /// <param name="other">The probability value to compare with.</param>
        /// <returns>A boolean indicating whether the values are equal.</returns>
        public bool Equals(double other)
        {
            return value.Equals(other);
        }

        /// <summary>
        /// Translates the probability to a string representation.
        /// </summary>
        /// <returns>A string representation of the probability.</returns>
        public override string ToString()
        {
            return ToString(null, null);
        }

        /// <summary>
        /// Translates the probability to a string representation.
        /// </summary>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> used to translate the probability to a string presentation.</param>
        /// <returns>A string representation of the probability.</returns>
        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

        /// <summary>
        /// Translates the probability to a string representation.
        /// </summary>
        /// <param name="format">The string format used to represent the value.</param>
        /// <param name="formatProvider">The <see cref="IFormatProvider"/> used to translate the probability to a string presentation.</param>
        /// <returns>A string representation of the probability.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (Math.Abs(value) < ToStringPrecision)
            {
                return "0";
            }

            if (Math.Abs(value - 1) < ToStringPrecision)
            {
                return "1";
            }

            if (double.IsNaN(value))
            {
                return "Undefined";
            }

            if (format == null)
            {
                return "1/" + (1 / value).ToString("F0", formatProvider ?? CultureInfo.CurrentCulture);
            }

            return value.ToString(format, formatProvider ?? CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Compares this probability with another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>1 in case the other object equals null of a probability or double $gt; this probability.</item>
        /// <item>0 in case the other object is a Probability or double with equal value.</item>
        /// <item>-1 in case the other object is a Probability or double $lt; this probability.</item>
        /// </list>
        /// </returns>
        /// <exception cref="ArgumentException">Thrown in case the <paramref name="obj"/> is not a double or <see cref="Probability"/>.</exception>
        public int CompareTo(object obj)
        {
            if (obj == null)
            { 
                throw new AssemblyException(nameof(obj), EAssemblyErrors.ValueMayNotBeNull);
            }

            if (obj is Probability probability)
            {
                return CompareTo(probability);
            }

            if (obj is double d)
            {
                return CompareTo(d);
            }

            throw new AssemblyException(nameof(obj), EAssemblyErrors.InvalidArgumentType);
        }

        /// <summary>
        /// Compares this probability with another <see cref="Probability"/>.
        /// </summary>
        /// <param name="other">The <see cref="Probability"/> to compare with.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>1 in case the other probability $gt; this probability.</item>
        /// <item>0 in case the other probability and this probability are equal.</item>
        /// <item>-1 in case the other probability $lt; this probability.</item>
        /// </list>
        /// </returns>
        public int CompareTo(Probability other)
        {
            return value.CompareTo(other.value);
        }

        /// <summary>
        /// Compares this probability with another double.
        /// </summary>
        /// <param name="other">The value to compare with.</param>
        /// <returns>
        /// <list type="bullet">
        /// <item>1 in case the other value $gt; this probability.</item>
        /// <item>0 in case the other value and this probability are equal.</item>
        /// <item>-1 in case the other value $lt; this probability.</item>
        /// </list>
        /// </returns>
        public int CompareTo(double other)
        {
            return value.CompareTo(other);
        }
    }
}
