#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

namespace Assembly.Kernel.Exceptions
{
    /// <summary>
    /// Error codes of errors which can occur during the assembly process.
    /// </summary>
    public enum EAssemblyErrors
    {
        /// <summary>
        /// The failure probability signalling limit is out of range.
        /// The value is smaller than 0 or greater than 1.
        /// </summary>
        SignallingLimitOutOfRange,

        /// <summary>
        /// The failure probability lower limit is out of range.
        /// The value is smaller than 0 or greater than 1.
        /// </summary>
        LowerLimitOutOfRange,

        /// <summary>
        /// The failure probability margin factor is out of range.
        /// The value is smaller than 0 or greater than 1.
        /// </summary>
        FailurePropbabilityMarginOutOfRange,

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
        /// The probability signalling limit value is above the lower limit value.
        /// Signalling limit (eg 1/10000) should be smaller or equal to the lower limit (1/300).
        /// </summary>
        SignallingLimitAboveLowerLimit,

        /// <summary>
        /// The calculated cross section failure probability signalling limit 
        /// is above or equal to the signalling limit of the section.
        /// </summary>
        PsigDsnAbovePsig,

        /// <summary>
        /// The calculated cross section failure probability lower limit 
        /// is above or equal to the lower limit of the section.
        /// </summary>
        PlowDsnAbovePlow,

        /// <summary>
        /// The lower limit of the category is above the upper limit of the category.
        /// </summary>
        LowerLimitIsAboveUpperLimit,

        /// <summary>
        /// The category lower limit is out of range.
        /// The value is smaller than 0 or greater than 1.
        /// </summary>
        CategoryLowerLimitOutOfRange,

        /// <summary>
        /// The category upper limit is out of range.
        /// The value is smaller than 0 or greater than 1.
        /// </summary>
        CategoryUpperLimitOutOfRange,

        /// <summary>
        /// The input for the translate assessment result method is invalid for the current method.
        /// </summary>
        TranslateAssessmentInvalidInput,

        /// <summary>
        /// The value passed may not be null
        /// </summary>
        ValueMayNotBeNull,

        /// <summary>
        /// The category limit passed to the method is not allowed.
        /// </summary>
        CategoryNotAllowed,

        /// <summary>
        /// A does not comply result is found after a comply result for a higher category.
        /// </summary>
        DoesNotComplyAfterComply,

        /// <summary>
        /// The length field of the failure mechanism section &lt;=0.
        /// Or the calculated section length isn't the same as the provided length.
        /// </summary>
        FmSectionLengthInvalid,

        /// <summary>
        /// The section start or end field of the failure mechanism section &lt;0 
        /// or the section end is defined before the section start.
        /// </summary>
        FmSectionSectionStartEndInvalid,

        /// <summary>
        /// The failure probability is greater than one or below zero. Which is an invalid value.
        /// </summary>
        FailureProbabilityOutOfRange,

        /// <summary>
        /// The results in the list are not all of the same type. 
        /// One of the inputs in the list is either not of the direct or indirect type.
        /// </summary>
        InputNotTheSameType,

        /// <summary>
        /// Input for the assemble failure mechanism method are invalid. This means the list of FmSection assembly results is empty.
        /// </summary>
        FailureMechanismAssemblerInputInvalid,

        /// <summary>
        /// The list of failure mechanism sections is empty or incomplete. 
        /// </summary>
        CommonFailureMechanismSectionsInvalid,

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
        /// Multiple Failure mechanism sections with same start and end are found in failure mechanism sections
        /// </summary>
        FailureMechanismDuplicateSection,

        /// <summary>
        /// The categories passed to the categories list do not cover the full range of probabilities between 0 and 1.
        /// </summary>
        InvalidCategoryLimits
    }
}