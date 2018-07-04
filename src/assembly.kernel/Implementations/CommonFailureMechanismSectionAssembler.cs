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
            FailureMechanismSectionList commonSections =
                FindGreatestCommonDenominatorSectionsWbi3A1(failureMechanismSections, assessmentSectionLength);

            // step 2: determine assessment results per section for each failure mechanism.
            var failureMechanismResults = new List<FailureMechanismSectionList>();

            foreach (FailureMechanismSectionList failureMechanismSectionList in failureMechanismSections)
            {
                failureMechanismResults.Add(TranslateFailureMechanismResultsToCommonSectionsWbi3B1(failureMechanismSectionList, commonSections));
            }

            // step 3: determine combined result per common section
            var combinedAssessmentResult = new List<FmSectionWithDirectCategory>();

            var commonSectionsArray = commonSections.Results.ToArray();
            foreach (var failureMechanismSectionList in failureMechanismResults)
            {
                for (var i = 0; i < commonSectionsArray.Length; i++)
                {
                    var commonSectionEnd = commonSectionsArray[i].SectionEnd;
                    var commonSectionStart = commonSectionsArray[i].SectionStart;
                    var commonSectionLength = commonSectionEnd - commonSectionStart;

                    var section = failureMechanismSectionList.GetSectionCategoryForPoint(commonSectionEnd - commonSectionLength/2.0);

                    var sectionWithDirectCategory = section as FmSectionWithDirectCategory;
                    if (sectionWithDirectCategory != null)
                    {
                        // step 3: determin combined result for the section using 
                        // the current failure mechanism section result.
                        FmSectionWithDirectCategory combinedFailureMechanismSectionResult;

                        if (i < combinedAssessmentResult.Count)
                        {
                            combinedFailureMechanismSectionResult = combinedAssessmentResult[i];
                        }
                        else
                        {
                            combinedFailureMechanismSectionResult = new FmSectionWithDirectCategory(commonSectionStart,
                                commonSectionEnd, EFmSectionCategory.NotApplicable);
                            combinedAssessmentResult.Add(combinedFailureMechanismSectionResult);
                        }

                        DetermineCombinedCategory(partialAssembly, combinedFailureMechanismSectionResult, sectionWithDirectCategory.Category);
                    }
                }
            }
            return new AssemblyResult(failureMechanismResults, combinedAssessmentResult);
        }

        /// <inheritdoc />
        public FailureMechanismSectionList FindGreatestCommonDenominatorSectionsWbi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var mechanismSectionLists = CheckGreatestCommonDenominatorInput(failureMechanismSectionLists, assessmentSectionLength);

            var sectionLimits = new List<double>();

            foreach (var failureMechanismSectionList in mechanismSectionLists)
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
            var previousSectionEnd = 0.0;
            var commonSections = new List<FailureMechanismSection>();
            foreach (var sectionLimit in sectionLimits)
            {
                commonSections.Add(new FailureMechanismSection(previousSectionEnd,sectionLimit));
                previousSectionEnd = sectionLimit;
            }

            return new FailureMechanismSectionList("Common", commonSections);
        }

        /// <inheritdoc />
        public FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsWbi3B1(FailureMechanismSectionList failureMechanismSectionList,
            FailureMechanismSectionList commonSections)
        {
            CheckResultsToCommonSectionsInput(commonSections, failureMechanismSectionList);

            var commonSectionsArray = commonSections.Results as FailureMechanismSection[] ?? commonSections.Results.ToArray();

            var resultsToCommonSections = new List<FailureMechanismSection>();
            foreach (var commonSection in commonSectionsArray)
            {
                var section = failureMechanismSectionList.GetSectionCategoryForPoint(commonSection.SectionEnd);

                var sectionWithDirectCategory = section as FmSectionWithDirectCategory;
                if (sectionWithDirectCategory != null)
                {
                    resultsToCommonSections.Add(new FmSectionWithDirectCategory(commonSection.SectionStart, commonSection.SectionEnd,
                        sectionWithDirectCategory.Category));
                }
                else
                {
                    resultsToCommonSections.Add(new FmSectionWithIndirectCategory(commonSection.SectionStart, commonSection.SectionEnd,
                        ((FmSectionWithIndirectCategory) section).Category));
                }
            }

            return new FailureMechanismSectionList(failureMechanismSectionList.FailureMechanismId, resultsToCommonSections);
        }

        private static void CheckResultsToCommonSectionsInput(FailureMechanismSectionList commonSections,
            FailureMechanismSectionList failureMechanismSectionList)
        {
            if (commonSections == null || failureMechanismSectionList == null)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (Math.Abs(commonSections.Results.Last().SectionEnd - failureMechanismSectionList.Results.Last().SectionEnd) > 1e-8)
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            var firstResult = failureMechanismSectionList.Results.First();
            if (!(firstResult is FmSectionWithIndirectCategory) && !(firstResult is FmSectionWithDirectCategory))
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.SectionsWithoutCategory);
            }
        }

        private static FailureMechanismSectionList[] CheckGreatestCommonDenominatorInput(IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            if (double.IsNaN(assessmentSectionLength))
            {
                throw new AssemblyException("AssessmentSectionLength",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (assessmentSectionLength <= 0.0)
            {
                throw new AssemblyException("AssessmentSectionLength",
                    EAssemblyErrors.SectionLengthOutOfRange);
            }

            if (failureMechanismSectionLists == null)
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var mechanismSectionLists = failureMechanismSectionLists as FailureMechanismSectionList[] ??
                                        failureMechanismSectionLists.ToArray();
            if (!mechanismSectionLists.Any())
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            return mechanismSectionLists;
        }

        private static void DetermineCombinedCategory(bool partialAssembly,
            FmSectionWithDirectCategory combinedFailureMechanismSectionResult,
            EFmSectionCategory currentCategory)
        {
            var combinedCategory = combinedFailureMechanismSectionResult.Category;
            switch (currentCategory)
            {
                case EFmSectionCategory.Iv:
                case EFmSectionCategory.IIv:
                case EFmSectionCategory.IIIv:
                case EFmSectionCategory.IVv:
                case EFmSectionCategory.Vv:
                case EFmSectionCategory.VIv:
                    if (currentCategory > combinedCategory)
                    {
                        combinedCategory = currentCategory;
                    }

                    break;
                case EFmSectionCategory.VIIv:
                    if (!partialAssembly)
                    {
                        if (currentCategory > combinedCategory)
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

            combinedFailureMechanismSectionResult.Category = combinedCategory;
        }
    }
}