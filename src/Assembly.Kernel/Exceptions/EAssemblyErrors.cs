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

namespace Assembly.Kernel.Exceptions
{
    /// <summary>
    /// Error codes of errors which can occur during the assembly process.
    /// </summary>
    public enum EAssemblyErrors
    {
        /// <summary>
        /// The length effect factor is out of range.
        /// The value is smaller than 1.
        /// </summary>
        LengthEffectFactorOutOfRange,

        /// <summary>
        /// The length of the section is out of range.
        /// The value is equal or below zero. 
        /// </summary>
        SectionLengthOutOfRange,

        /// <summary>
        /// The signal flooding probability is above the maximum allowable flooding probability.
        /// </summary>
        SignalFloodingProbabilityAboveMaximumAllowableFloodingProbability,

        /// <summary>
        /// The lower limit of the category is above the upper limit of the category.
        /// </summary>
        LowerLimitIsAboveUpperLimit,

        /// <summary>
        /// The length field of the failure mechanism section &lt;=0
        /// or the calculated section length isn't the same as the provided length.
        /// </summary>
        FailureMechanismSectionLengthInvalid,

        /// <summary>
        /// The section start or end field of the failure mechanism section &lt;0 
        /// or the section end is defined before the section start.
        /// </summary>
        FailureMechanismSectionSectionStartEndInvalid,

        /// <summary>
        /// The failure probability is $gt; 1 or $lt; 0. Which is an invalid value.
        /// </summary>
        FailureProbabilityOutOfRange,

        /// <summary>
        /// The results in the list are not all of the same type. 
        /// </summary>
        InputNotTheSameType,

        /// <summary>
        /// The specified result list is empty.
        /// </summary>
        EmptyResultsList,

        /// <summary>
        /// The list of failure mechanism sections is incomplete. 
        /// </summary>
        CommonFailureMechanismSectionsInvalid,

        /// <summary>
        /// The section limits of two lists of common sections are not equal.
        /// </summary>
        CommonFailureMechanismSectionsDoNotHaveEqualSections,

        /// <summary>
        /// Start and end positions of consecutive sections do not match.
        /// </summary>
        CommonFailureMechanismSectionsNotConsecutive,

        /// <summary>
        /// The requested point of the assessment section is not within the range of failure mechanism sections in the 
        /// assessment section.
        /// </summary>
        RequestedPointOutOfRange,

        /// <summary>
        /// The categories passed to the categories list do not cover the full range of probabilities between 0 and 1.
        /// </summary>
        InvalidCategoryLimits,

        /// <summary>
        /// The supplied sections need to have a category result, otherwise the result cannot be translated to other sections.
        /// </summary>
        SectionsWithoutCategory,

        /// <summary>
        /// The provided probability for a profile exceeds the corresponding probability for the related section.
        /// </summary>
        ProfileProbabilityGreaterThanSectionProbability,

        /// <summary>
        /// The provided probability is undefined.
        /// </summary>
        UndefinedProbability,

        /// <summary>
        /// The provided list with section results contains one or more sections without result. Assembly cannot be performed.
        /// </summary>
        EncounteredOneOrMoreSectionsWithoutResult,

        /// <summary>
        /// Incorrect category value is provided.
        /// </summary>
        InvalidCategoryValue,

        /// <summary>
        /// Probabilities for both a profile and section are specified but only one is defined.
        /// </summary>
        ProbabilitiesNotBothDefinedOrUndefined,

        /// <summary>
        /// An invalid enum value is specified.
        /// </summary>
        InvalidEnumValue,

        /// <summary>
        /// One of the specified lists contains more sections than the other specified list(s).
        /// </summary>
        UnequalCommonFailureMechanismSectionLists,

        /// <summary>
        /// The specified common sections do not have interpretation categories.
        /// </summary>
        CommonSectionsWithoutCategoryValues
    }
}