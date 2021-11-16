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

using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// Group 3 failure mechanism section.
    /// </summary>
    public class Group3FailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        /// <summary>
        /// The result of detailed assessment as input for assembly. This result can be used in WBI-0G-4. In Riskeer the other method for group 3 failure mechanisms is used (WBI-0G-6).
        /// </summary>
        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        /// <summary>
        /// The result of detailed assessment translated to an EFmSectionCategory as input for assembly. This result can be used in WBI-0G-4. In Riskeer the other method for group 3 failure mechanisms is used (WBI-0G-6).
        /// </summary>
        public EFmSectionCategory DetailedAssessmentResultValue { get; set; }

        /// <summary>
        /// The result of tailor made assessment as input for assembly
        /// </summary>
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        /// <summary>
        /// The result of simple assessment translated to an EFmSectionCategory as input for assembly.
        /// </summary>
        public EFmSectionCategory TailorMadeAssessmentResultCategory { get; set; }
    }
}