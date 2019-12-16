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
using System.Diagnostics;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    [TestFixture]
    public class TranslateAssessmentResultPerformanceTests
    {
        private readonly IAssessmentResultsTranslator translator = new AssessmentResultsTranslator();

        private static readonly CategoriesList<FmSectionCategory> AssessmentSectionAmelandDefaultCategories =
            new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(EFmSectionCategory.Iv, 0, 1 / 30.0 * 1 / 1000.0 * 0.04 / 14.4),
                new FmSectionCategory(EFmSectionCategory.IIv, 1 / 30.0 * 1 / 1000.0 * 0.04 / 14.4,
                                      1 / 1000.0 * 0.04 / 14.4),
                new FmSectionCategory(EFmSectionCategory.IIIv, 1 / 1000.0 * 0.04 / 14.4, 1 / 300.0 * 0.04 / 14.4),
                new FmSectionCategory(EFmSectionCategory.IVv, 1 / 300.0 * 0.04 / 14.4, 1 / 300.0),
                new FmSectionCategory(EFmSectionCategory.Vv, 1 / 300.0, 30.0 * 1 / 300.0),
                new FmSectionCategory(EFmSectionCategory.VIv, 30.0 * 1 / 300.0, 1.0)
            });

        [Test]
        public void TranslateAssessmentResults()
        {
            Wbi0E1(EAssessmentResultTypeE1.Fv);
            Wbi0E1(EAssessmentResultTypeE1.Gr);
            Wbi0E1(EAssessmentResultTypeE1.Vb);
            Wbi0E1(EAssessmentResultTypeE1.Nvt);

            Wbi0E2(EAssessmentResultTypeE1.Fv);
            Wbi0E2(EAssessmentResultTypeE1.Gr);
            Wbi0E2(EAssessmentResultTypeE1.Vb);
            Wbi0E2(EAssessmentResultTypeE1.Nvt);

            Wbi0E3(EAssessmentResultTypeE2.Gr);
            Wbi0E3(EAssessmentResultTypeE2.Nvt);
            Wbi0E3(EAssessmentResultTypeE2.Wvt);

            Wbi0E4(EAssessmentResultTypeE2.Gr);
            Wbi0E4(EAssessmentResultTypeE2.Nvt);
            Wbi0E4(EAssessmentResultTypeE2.Wvt);

            Wbi0G1(EAssessmentResultTypeG1.Gr);
            Wbi0G1(EAssessmentResultTypeG1.Ngo);
            Wbi0G1(EAssessmentResultTypeG1.V);
            Wbi0G1(EAssessmentResultTypeG1.Vn);

            Wbi0G2(EAssessmentResultTypeG1.Gr);
            Wbi0G2(EAssessmentResultTypeG1.Ngo);
            Wbi0G2(EAssessmentResultTypeG1.V);
            Wbi0G2(EAssessmentResultTypeG1.Vn);

            Wbi0G3(EAssessmentResultTypeG2.Gr, double.NaN);
            Wbi0G3(EAssessmentResultTypeG2.Ngo, double.NaN);
            Wbi0G3(EAssessmentResultTypeG2.ResultSpecified, 0.002);

            Wbi0G4(EAssessmentResultTypeG2.Gr, null);
            Wbi0G4(EAssessmentResultTypeG2.Ngo, null);
            Wbi0G4(EAssessmentResultTypeG2.ResultSpecified, EFmSectionCategory.Iv);
            Wbi0G4(EAssessmentResultTypeG2.ResultSpecified, EFmSectionCategory.IIv);
            Wbi0G4(EAssessmentResultTypeG2.ResultSpecified, EFmSectionCategory.IIIv);
            Wbi0G4(EAssessmentResultTypeG2.ResultSpecified, EFmSectionCategory.IVv);
            Wbi0G4(EAssessmentResultTypeG2.ResultSpecified, EFmSectionCategory.Vv);
            Wbi0G4(EAssessmentResultTypeG2.ResultSpecified, EFmSectionCategory.VIv);

            Wbi0G5(EAssessmentResultTypeG2.Gr, double.NaN);
            Wbi0G5(EAssessmentResultTypeG2.Ngo, double.NaN);
            Wbi0G5(EAssessmentResultTypeG2.ResultSpecified, 0.002);

            Wbi0G6();

            Wbi0T1(EAssessmentResultTypeT1.Fv);
            Wbi0T1(EAssessmentResultTypeT1.Gr);
            Wbi0T1(EAssessmentResultTypeT1.Ngo);
            Wbi0T1(EAssessmentResultTypeT1.V);
            Wbi0T1(EAssessmentResultTypeT1.Vn);

            Wbi0T2(EAssessmentResultTypeT2.Fv);
            Wbi0T2(EAssessmentResultTypeT2.Gr);
            Wbi0T2(EAssessmentResultTypeT2.Ngo);
            Wbi0T2(EAssessmentResultTypeT2.V);
            Wbi0T2(EAssessmentResultTypeT2.Verd);
            Wbi0T2(EAssessmentResultTypeT2.Vn);

            Wbi0T3(EAssessmentResultTypeT3.Fv, double.NaN);
            Wbi0T3(EAssessmentResultTypeT3.Gr, double.NaN);
            Wbi0T3(EAssessmentResultTypeT3.Ngo, double.NaN);
            Wbi0T3(EAssessmentResultTypeT3.ResultSpecified, 0.005);

            Wbi0T4(EAssessmentResultTypeT3.Fv, null);
            Wbi0T4(EAssessmentResultTypeT3.Gr, null);
            Wbi0T4(EAssessmentResultTypeT3.Ngo, null);
            Wbi0T4(EAssessmentResultTypeT3.ResultSpecified, EFmSectionCategory.Iv);
            Wbi0T4(EAssessmentResultTypeT3.ResultSpecified, EFmSectionCategory.IIv);
            Wbi0T4(EAssessmentResultTypeT3.ResultSpecified, EFmSectionCategory.IIIv);
            Wbi0T4(EAssessmentResultTypeT3.ResultSpecified, EFmSectionCategory.IVv);
            Wbi0T4(EAssessmentResultTypeT3.ResultSpecified, EFmSectionCategory.Vv);
            Wbi0T4(EAssessmentResultTypeT3.ResultSpecified, EFmSectionCategory.VIv);

            Wbi0T5(EAssessmentResultTypeT3.Fv, double.NaN);
            Wbi0T5(EAssessmentResultTypeT3.Gr, double.NaN);
            Wbi0T5(EAssessmentResultTypeT3.Ngo, double.NaN);
            Wbi0T5(EAssessmentResultTypeT3.ResultSpecified, 0.005);

            Wbi0T6(EAssessmentResultTypeT3.Fv);
            Wbi0T6(EAssessmentResultTypeT3.Gr);
            Wbi0T6(EAssessmentResultTypeT3.Ngo);
            Wbi0T6(EAssessmentResultTypeT3.ResultSpecified);

            Wbi0T7(EAssessmentResultTypeT4.Fv, double.NaN);
            Wbi0T7(EAssessmentResultTypeT4.Gr, double.NaN);
            Wbi0T7(EAssessmentResultTypeT4.Ngo, double.NaN);
            Wbi0T7(EAssessmentResultTypeT4.V, double.NaN);
            Wbi0T7(EAssessmentResultTypeT4.Vn, double.NaN);
            Wbi0T7(EAssessmentResultTypeT4.ResultSpecified, 0.3);
        }

        public void Wbi0E1(EAssessmentResultTypeE1 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0E1(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0E1({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0E2(EAssessmentResultTypeE1 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0E2(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0E2({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0E3(EAssessmentResultTypeE2 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0E3(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0E3({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0E4(EAssessmentResultTypeE2 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0E4(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0E4({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0G1(EAssessmentResultTypeG1 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0G1(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0G1({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0G2(EAssessmentResultTypeG1 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0G1(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0G2({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0G3(EAssessmentResultTypeG2 input, double failureProb)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0G3(input, failureProb, AssessmentSectionAmelandDefaultCategories);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0G3({input}; {failureProb}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0G4(EAssessmentResultTypeG2 input, EFmSectionCategory? category)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0G4(input, category);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0G4({input}; {category}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0G5(EAssessmentResultTypeG2 input, double failureProb)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0G5(0.5, input, failureProb,
                                                       AssessmentSectionAmelandDefaultCategories);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0G5({input}; {failureProb}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0G6()
        {
            var compliancyResults = new FmSectionCategoryCompliancyResults()
                                    .Set(EFmSectionCategory.Iv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.IIv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies);

            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0G6(compliancyResults);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0G6: {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T1(EAssessmentResultTypeT1 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0T1(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T1({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T2(EAssessmentResultTypeT2 input)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0T2(input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T2({input}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T3(EAssessmentResultTypeT3 input, double failureProb)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0T3(input, failureProb, AssessmentSectionAmelandDefaultCategories);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T3({input}; {failureProb}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T4(EAssessmentResultTypeT3 input, EFmSectionCategory? category)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0T4(input, category);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T4({input}; {category}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T5(EAssessmentResultTypeT3 input, double failureProb)
        {
            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0T5(0.003, input, failureProb,
                                                       AssessmentSectionAmelandDefaultCategories);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T5({input}; {failureProb}): {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T6(EAssessmentResultTypeT3 input)
        {
            FmSectionCategoryCompliancyResults compliancyResults = null;
            if (input == EAssessmentResultTypeT3.ResultSpecified)
            {
                compliancyResults = new FmSectionCategoryCompliancyResults()
                                    .Set(EFmSectionCategory.Iv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.IIv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.IIIv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.IVv, ECategoryCompliancy.Complies)
                                    .Set(EFmSectionCategory.Vv, ECategoryCompliancy.Complies);
            }

            var watch = Stopwatch.StartNew();
            translator.TranslateAssessmentResultWbi0T6(compliancyResults, input);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T6: {elapsedMs} ms (max: 200 ms)");
        }

        public void Wbi0T7(EAssessmentResultTypeT4 input, double failureProb)
        {
            var failureProbabilitySignallingLimit = 0.0001;
            var lengthEffectFactor = 3.0;
            var failureProbabilityMarginFactor = 0.2;
            var watch = Stopwatch.StartNew();
            var categoryBoundary = failureProbabilitySignallingLimit * failureProbabilityMarginFactor * 10.0 /
                                   lengthEffectFactor;
            CategoriesList<FmSectionCategory> categoriesList = new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(EFmSectionCategory.IIv, 0.0, categoryBoundary),
                new FmSectionCategory(EFmSectionCategory.Vv, categoryBoundary, 1.0)
            });
            translator.TranslateAssessmentResultWbi0T7(input, failureProb, categoriesList);
            watch.Stop();
            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Wbi0T7({input}; {failureProb}): {elapsedMs} ms (max: 200 ms)");
        }
    }
}