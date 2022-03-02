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

using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// Base class for a failure mechanism section.
    /// </summary>
    public class ExpectedFailureMechanismSection : IExpectedFailureMechanismSection
    {
        public ExpectedFailureMechanismSection(string sectionName, double start, double end, bool isRelevant, Probability initialMechanismProbabilitySection, ERefinementStatus refinementStatus, Probability refinedProbabilitySection, Probability expectedCombinedProbabilitySection, EInterpretationCategory expectedInterpretationCategory)
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

        public double Start { get; }

        public double End { get; }

        public bool IsRelevant { get; }

        public Probability InitialMechanismProbabilitySection { get; }

        public ERefinementStatus RefinementStatus { get; }

        public Probability RefinedProbabilitySection { get; }

        public Probability ExpectedCombinedProbabilitySection { get; }

        public EInterpretationCategory ExpectedInterpretationCategory { get; }
    }
}