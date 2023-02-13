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

using System;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Implementations
{
    /// <summary>
    /// Calculator for category limits.
    /// </summary>
    public class CategoryLimitsCalculator : ICategoryLimitsCalculator
    {
        private const double lowerLimit = 0.0;
        private const double upperLimit = 1.0;

        public CategoriesList<InterpretationCategory> CalculateInterpretationCategoryLimitsBoi01(
            AssessmentSection section)
        {
            var sigDiv1000 = new Probability(section.SignalFloodingProbability / 1000.0);
            var sigDiv100 = new Probability(section.SignalFloodingProbability / 100.0);
            var sigDiv10 = new Probability(section.SignalFloodingProbability / 10.0);
            var lowTimes10 = new Probability(Math.Min(upperLimit, (double) section.MaximumAllowableFloodingProbability * 10.0));

            return new CategoriesList<InterpretationCategory>(new[]
            {
                new InterpretationCategory(EInterpretationCategory.III, new Probability(lowerLimit), sigDiv1000),
                new InterpretationCategory(EInterpretationCategory.II, sigDiv1000, sigDiv100),
                new InterpretationCategory(EInterpretationCategory.I, sigDiv100, sigDiv10),
                new InterpretationCategory(EInterpretationCategory.Zero, sigDiv10, section.SignalFloodingProbability),
                new InterpretationCategory(EInterpretationCategory.IMin, section.SignalFloodingProbability, section.MaximumAllowableFloodingProbability),
                new InterpretationCategory(EInterpretationCategory.IIMin, section.MaximumAllowableFloodingProbability, lowTimes10),
                new InterpretationCategory(EInterpretationCategory.IIIMin, lowTimes10, new Probability(upperLimit))
            });
        }
        
        public CategoriesList<AssessmentSectionCategory> CalculateAssessmentSectionCategoryLimitsBoi21(AssessmentSection section)
        {
            var sigDiv30 = new Probability(section.SignalFloodingProbability / 30.0);
            var lowTimes30 = new Probability(Math.Min(upperLimit, (double) section.MaximumAllowableFloodingProbability * 30.0));

            return new CategoriesList<AssessmentSectionCategory>(new[]
            {
                new AssessmentSectionCategory(EAssessmentGrade.APlus, new Probability(lowerLimit), sigDiv30),
                new AssessmentSectionCategory(EAssessmentGrade.A, sigDiv30, section.SignalFloodingProbability),
                new AssessmentSectionCategory(EAssessmentGrade.B, section.SignalFloodingProbability, section.MaximumAllowableFloodingProbability),
                new AssessmentSectionCategory(EAssessmentGrade.C, section.MaximumAllowableFloodingProbability, lowTimes30),
                new AssessmentSectionCategory(EAssessmentGrade.D, lowTimes30, new Probability(upperLimit))
            });
        }
    }
}