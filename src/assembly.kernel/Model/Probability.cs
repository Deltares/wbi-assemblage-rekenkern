#region Copyright (C) Rijkswaterstaat 2019. All rights reserved

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

using System;
using System.Globalization;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    public struct Probability : IEquatable<Probability>, IEquatable<double>, IFormattable, IComparable, IComparable<Probability>, IComparable<double>
    {
        private static readonly double ToStringPrecision = 1e-100;

        /// <summary>
        /// Represents a value that is not a number (NaN). This field is constant.
        /// </summary>
        /// <seealso cref="double.NaN"/>
        public static readonly Probability NaN = new Probability(double.NaN);
        
        public Probability(double probabilityValue)
        {
            ValidateProbabilityValue(probabilityValue);
            Value = probabilityValue;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public double Value { get; }

        public int ReturnPeriod => (int) Math.Round(1 / Value);

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
            return new Probability(Math.Max(0,left.Value - right.Value));
        }

        public static Probability operator +(Probability left, Probability right)
        {
            return new Probability(Math.Min(1,left.Value + right.Value));
        }

        public static Probability operator *(Probability left, double right)
        {
            return new Probability(left.Value * right);
        }

        public static Probability operator *(double left, Probability right)
        {
            return new Probability(left * right.Value);
        }

        public static Probability operator *(Probability left, Probability right)
        {
            return new Probability(left.Value * right.Value);
        }

        public static Probability operator /(Probability left, double right)
        {
            return new Probability(left.Value / right);
        }

        public static Probability operator /(double left, Probability right)
        {
            return new Probability(left / right.Value);
        }

        public static Probability operator /(Probability left, Probability right)
        {
            return new Probability(left.Value / right.Value);
        }

        public static implicit operator double(Probability d)
        {
            return d.Value;
        }

        public static explicit operator Probability(double d)
        {
            return new Probability(d);
        }

        public static bool operator <(Probability left, Probability right)
        {
            return left.Value < right.Value;
        }

        public static bool operator <=(Probability left, Probability right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >(Probability left, Probability right)
        {
            return left.Value > right.Value;
        }

        public static bool operator >=(Probability left, Probability right)
        {
            return left.Value >= right.Value;
        }

        public static bool operator <(Probability left, double right)
        {
            return left.Value < right;
        }

        public static bool operator <=(Probability left, double right)
        {
            return left.Value <= right;
        }

        public static bool operator >(Probability left, double right)
        {
            return left.Value > right;
        }

        public static bool operator >=(Probability left, double right)
        {
            return left.Value >= right;
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
            return Value.GetHashCode();
        }

        public bool Equals(Probability other)
        {
            return other.Value.Equals(Value);
        }

        public bool Equals(double other)
        {
            return Value.Equals(other);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (Math.Abs(Value) < ToStringPrecision)
            {
                return "0";
            }

            if (Math.Abs(Value - 1) < ToStringPrecision)
            {
                return "1";
            }

            if (double.IsNaN(Value))
            {
                return "Onbekend";
            }

            if (format == null)
            {
                return "1/" + (1 / Value).ToString("F0", formatProvider ?? CultureInfo.CurrentCulture);
            }

            return Value.ToString(format, formatProvider ?? CultureInfo.CurrentCulture);
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
            return Value.CompareTo(other.Value);
        }

        public int CompareTo(double other)
        {
            return Value.CompareTo(other);
        }
    }
}
