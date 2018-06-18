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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class CommonFailureMechanismSectionAssembler : ICommonFailureMechanismSectionAssembler
    {
        /// <inheritdoc />
        public AssemblyResult AssembleCommonFailureMechanismSections(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists, double assessmentSectionLength,
            bool partialAssembly)
        {
            List<FailureMechanismSectionList> failureMechanismSections = failureMechanismSectionLists.ToList();

            // step 1: create greatest common denominator list of the failure mechanism sections in the list.
            List<double> sectionLimits =
                FindGreatestCommonDenominatorSections(failureMechanismSections, assessmentSectionLength);

            var failureMechanismResults = new List<FailureMechanismSectionList>();
            var combinedAssessmentResult = new List<FmSectionWithDirectCategory>();

            // step 2: determine assessment results for each section.
            foreach (var failureMechanismSectionList in failureMechanismSections)
            {
                var fmSectionResultList = new List<FmSectionWithCategory>();

                for (var i = 0; i < sectionLimits.Count; i++)
                {
                    var commonSectionEnd = sectionLimits[i];
                    var commonSectionStart = i == 0 ? 0.0 : sectionLimits[i - 1];

                    var section = failureMechanismSectionList.GetSectionCategoryForPoint(commonSectionEnd);

                    if (section.Type == EAssembledAssessmentResultType.IndirectAssessment)
                    {
                        fmSectionResultList.Add(new FmSectionWithIndirectCategory(commonSectionStart, commonSectionEnd,
                            ((FmSectionWithIndirectCategory) section).Category));
                    }
                    else
                    {
                        // first determine the assessment result for the failure mechanism section
                        var currentCategory = ((FmSectionWithDirectCategory) section).Category;
                        fmSectionResultList.Add(new FmSectionWithDirectCategory(commonSectionStart, commonSectionEnd,
                            currentCategory));

                        // step 3: determin combined result for the section using 
                        // the current failure mechanism section result.
                        FmSectionWithDirectCategory combinedSectionResult;

                        if (i < combinedAssessmentResult.Count)
                        {
                            combinedSectionResult = combinedAssessmentResult[i];
                        }
                        else
                        {
                            combinedSectionResult = new FmSectionWithDirectCategory(commonSectionStart,
                                commonSectionEnd, EFmSectionCategory.NotApplicable);
                            combinedAssessmentResult.Add(combinedSectionResult);
                        }

                        DetermineCombinedCategory(partialAssembly, combinedSectionResult, currentCategory);
                    }
                }

                failureMechanismResults.Add(
                    new FailureMechanismSectionList(failureMechanismSectionList.FailureMechanismId,
                        fmSectionResultList));
            }

            return new AssemblyResult(failureMechanismResults, combinedAssessmentResult);
        }

        private static List<double> FindGreatestCommonDenominatorSections(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var sectionLimits = new List<double>();

            foreach (var failureMechanismSectionList in failureMechanismSectionLists)
            {
                var totalFailureMechanismSectionLength = 0.0;
                foreach (var fmSection in failureMechanismSectionList.Results)
                {
                    var sectionEnd = fmSection.SectionEnd;
                    if (!sectionLimits.Contains(sectionEnd))
                    {
                        sectionLimits.Add(sectionEnd);
                    }

                    totalFailureMechanismSectionLength += sectionEnd - fmSection.SectionStart;
                }

                // compare calculated assessment section length with the provided length with a margin of 1 cm.
                if (Math.Abs(totalFailureMechanismSectionLength - assessmentSectionLength) > 0.01)
                {
                    throw new AssemblyException("AssembleCommonFailureMechanismSection",
                        EAssemblyErrors.FmSectionLengthInvalid);
                }
            }

            sectionLimits.Sort();
            return sectionLimits;
        }

        private static void DetermineCombinedCategory(bool partialAssembly,
            FmSectionWithDirectCategory combinedSectionResult,
            EFmSectionCategory currentCategory)
        {
            var combinedCategory = combinedSectionResult.Category;
            switch (currentCategory)
            {
                case EFmSectionCategory.Iv:
                case EFmSectionCategory.IIv:
                case EFmSectionCategory.IIIv:
                case EFmSectionCategory.IVv:
                case EFmSectionCategory.Vv:
                case EFmSectionCategory.VIv:
                    if (currentCategory.IsLowerCategoryThan(combinedCategory))
                    {
                        combinedCategory = currentCategory;
                    }

                    break;
                case EFmSectionCategory.VIIv:
                    if (!partialAssembly)
                    {
                        if (currentCategory.IsLowerCategoryThan(combinedCategory))
                        {
                            combinedCategory = currentCategory;
                        }
                    }

                    break;
                case EFmSectionCategory.Gr:
                    combinedCategory = EFmSectionCategory.Gr;
                    break;
                case EFmSectionCategory.NotApplicable:
                    // ignore not applicable
                    break;
            }

            combinedSectionResult.Category = combinedCategory;
        }
    }
}