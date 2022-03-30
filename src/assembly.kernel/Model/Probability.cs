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
using System.Globalization;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// This struct represents a probability. It can be used similar to double, but has a limited value within the range [0,1]. Operations to add or subtract will result in an exception as it is not possible to add or subtract probabilities.
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
        /// <exception cref="AssemblyException">An exception is thrown whenever <see cref="probabilityValue"/>&lt;0 or <see cref="probabilityValue"/>&gt;1 </exception>
        public Probability(double probabilityValue)
        {
            ValidateProbabilityValue(probabilityValue);
            value = probabilityValue;
        }

        /// <summary>
        /// Represents the return period of this probability.
        /// </summary>
        public int ReturnPeriod => (int) Math.Round(1 / value);

        /// <summary>
        /// Returns a new probability that represents the complementary value of this probability.
        /// </summary>
        public Probability Complement => new Probability(1-value);

        /// <summary>
        /// Specifies whether the probability is defined.
        /// </summary>
        public bool IsDefined => !double.IsNaN(value);

        /// <summary>
        /// Validates <paramref name="probability"/> for being a valid probability. This means a double within the range [0-1].
        /// </summary>
        /// <param name="probability">The probability to validate</param>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="probability"/> is smaller than 0</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="probability"/> exceeds 1</exception>
        private static void ValidateProbabilityValue(double probability)
        {
            if (!double.IsNaN(probability) && probability < 0 || !double.IsNaN(probability) && probability > 1)
            {
                throw new AssemblyException("Probability",EAssemblyErrors.FailureProbabilityOutOfRange);
            }
        }

        public static bool operator ==(Probability left, Probability right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Probability left, Probability right)
        {
            return !Equals(left, right);
        }

        public static Probability operator -(Probability left, Probability right)
        {
            return new Probability(Math.Max(0,left.value - right.value));
        }

        public static Probability operator +(Probability left, Probability right)
        {
            return new Probability(Math.Min(1,left.value + right.value));
        }

        public static Probability operator *(Probability left, double right)
        {
            return new Probability(left.value * right);
        }

        public static Probability operator *(double left, Probability right)
        {
            return new Probability(left * right.value);
        }

        public static Probability operator *(Probability left, Probability right)
        {
            return new Probability(left.value * right.value);
        }

        public static Probability operator /(Probability left, double right)
        {
            return new Probability(left.value / right);
        }

        public static Probability operator /(double left, Probability right)
        {
            return new Probability(left / right.value);
        }

        public static Probability operator /(Probability left, Probability right)
        {
            return new Probability(left.value / right.value);
        }

        public static implicit operator double(Probability d)
        {
            return d.value;
        }

        public static explicit operator Probability(double d)
        {
            return new Probability(d);
        }

        public static bool operator <(Probability left, Probability right)
        {
            return left.value < right.value;
        }

        public static bool operator <=(Probability left, Probability right)
        {
            return left.value <= right.value;
        }

        public static bool operator >(Probability left, Probability right)
        {
            return left.value > right.value;
        }

        public static bool operator >=(Probability left, Probability right)
        {
            return left.value >= right.value;
        }

        public static bool operator <(Probability left, double right)
        {
            return left.value < right;
        }

        public static bool operator <=(Probability left, double right)
        {
            return left.value <= right;
        }

        public static bool operator >(Probability left, double right)
        {
            return left.value > right;
        }

        public static bool operator >=(Probability left, double right)
        {
            return left.value >= right;
        }

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

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public bool Equals(Probability other)
        {
            return other.value.Equals(value);
        }

        public bool Equals(double other)
        {
            return value.Equals(other);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return ToString(null, formatProvider);
        }

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
                return "Onbekend";
            }

            if (format == null)
            {
                return "1/" + (1 / value).ToString("F0", formatProvider ?? CultureInfo.CurrentCulture);
            }

            return value.ToString(format, formatProvider ?? CultureInfo.CurrentCulture);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            { 
                return 1;
            }

            if (obj is Probability)
            {
                return CompareTo((Probability)obj);
            }

            if (obj is double)
            {
                return CompareTo((double)obj);
            }

            throw new ArgumentException("Argument must be double or Probability");
        }

        public int CompareTo(Probability other)
        {
            return value.CompareTo(other.value);
        }

        public int CompareTo(double other)
        {
            return value.CompareTo(other);
        }
    }
}
