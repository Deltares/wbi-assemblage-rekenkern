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

using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// Base class for a failure mechanism section.
    /// </summary>
    public class ExpectedFailureMechanismSection : IExpectedFailureMechanismSection
    {
        public ExpectedFailureMechanismSection(string sectionName, double start, double end, bool isRelevant,
                                               Probability initialMechanismProbabilitySection,
                                               ERefinementStatus refinementStatus,
                                               Probability refinedProbabilitySection,
                                               Probability expectedCombinedProbabilitySection,
                                               EInterpretationCategory expectedInterpretationCategory)
        {
            SectionName = sectionName;
            Start = start;
            End = end;
            IsRelevant = isRelevant;
            InitialMechanismProbabilitySection = initialMechanismProbabilitySection;
            RefinementStatus = refinementStatus;
            RefinedProbabilitySection = refinedProbabilitySection;
            ExpectedCombinedProbabilitySection = expectedCombinedProbabilitySection;
            ExpectedInterpretationCategory = expectedInterpretationCategory;
        }

        /// <summary>
        /// The name of the section.
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// Indicates whether the specific failure mechanism is relevant along this section.
        /// </summary>
        public bool IsRelevant { get; }

        /// <summary>
        /// Probability estimation of the initial mechanism for this section.
        /// </summary>
        public Probability InitialMechanismProbabilitySection { get; }

        /// <summary>
        /// Refinement status of this section.
        /// </summary>
        public ERefinementStatus RefinementStatus { get; }

        /// <summary>
        /// The estimated refined probability of the section.
        /// </summary>
        public Probability RefinedProbabilitySection { get; }

        /// <summary>
        /// The expected combined probability of the section.
        /// </summary>
        public Probability ExpectedCombinedProbabilitySection { get; }

        /// <summary>
        /// The expected interpretation category of the section.
        /// </summary>
        public EInterpretationCategory ExpectedInterpretationCategory { get; }

        /// <summary>
        /// Start of the section along the assessment section (in meters).
        /// </summary>
        public double Start { get; }

        /// <summary>
        /// End of the section along the assessment section (in meters).
        /// </summary>
        public double End { get; }
    }
}