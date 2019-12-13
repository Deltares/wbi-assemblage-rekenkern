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
using System.Collections.Generic;
using System.ComponentModel;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    public static class FailureMechanismFactory
    {
        private static readonly Dictionary<MechanismType, Func<IExpectedFailureMechanismResult>> Infos =
            new Dictionary<MechanismType, Func<IExpectedFailureMechanismResult>>
            {
                {MechanismType.STBI, CreateSTBIFailureMechanism},
                {MechanismType.STBU, CreateSTBUFailureMechanism},
                {MechanismType.STPH, CreateSTPHFailureMechanism},
                {MechanismType.STMI, CreateSTMIFailureMechanism},
                {MechanismType.AGK, CreateAGKFailureMechanism},
                {MechanismType.AWO, CreateAWOFailureMechanism},
                {MechanismType.GEBU, CreateGEBUFailureMechanism},
                {MechanismType.GABU, CreateGABUFailureMechanism},
                {MechanismType.GEKB, CreateGEKBFailureMechanism},
                {MechanismType.GABI, CreateGABIFailureMechanism},
                {MechanismType.ZST, CreateZSTFailureMechanism},
                {MechanismType.DA, CreateDAFailureMechanism},
                {MechanismType.HTKW, CreateHTKWFailureMechanism},
                {MechanismType.BSKW, CreateBSKWFailureMechanism},
                {MechanismType.PKW, CreatePKWFailureMechanism},
                {MechanismType.STKWp, CreateSTKWpFailureMechanism},
                {MechanismType.STKWl, CreateSTKWlFailureMechanism},
                {MechanismType.VLGA, CreateVLGAFailureMechanism},
                {MechanismType.VLAF, CreateVLAFFailureMechanism},
                {MechanismType.VLZV, CreateVLZVFailureMechanism},
                {MechanismType.NWObe, CreateNWObeFailureMechanism},
                {MechanismType.NWObo, CreateNWOboFailureMechanism},
                {MechanismType.NWOkl, CreateNWOklFailureMechanism},
                {MechanismType.NWOoc, CreateNWOocFailureMechanism},
                {MechanismType.HAV, CreateHAVFailureMechanism},
                {MechanismType.INN, CreateINNFailureMechanism}
            };

        /// <summary>
        /// Creates an empty IExpectedFailureMechanismResult based on the specified MechanismType.
        /// </summary>
        /// <param name="type">The mechanism type of the mechanism for which an empty expected result needs to be created</param>
        /// <returns></returns>
        public static IExpectedFailureMechanismResult CreateFailureMechanism(MechanismType type)
        {
            if (!Infos.ContainsKey(type))
            {
                throw new InvalidEnumArgumentException();
            }

            return Infos[type]();
        }

        #region Group 1

        private static ProbabilisticExpectedFailureMechanismResult CreateGEKBFailureMechanism()
        {
            return new ProbabilisticExpectedFailureMechanismResult("Grasbekleding erosie kruin en binnentalud", MechanismType.GEKB, 1);
        }

        private static ProbabilisticExpectedFailureMechanismResult CreateHTKWFailureMechanism()
        {
            return new ProbabilisticExpectedFailureMechanismResult("Hoogte kunstwerk", MechanismType.HTKW, 1);
        }

        private static ProbabilisticExpectedFailureMechanismResult CreateBSKWFailureMechanism()
        {
            return new ProbabilisticExpectedFailureMechanismResult("Betrouwbaarheid sluiting kunstwerk", MechanismType.BSKW, 1);
        }

        private static ProbabilisticExpectedFailureMechanismResult CreateSTKWpFailureMechanism()
        {
            return new ProbabilisticExpectedFailureMechanismResult("Sterkte en stabiliteit punconstructies", MechanismType.STKWp, 1);
        }

        #endregion

        #region Group 2

        private static ProbabilisticExpectedFailureMechanismResult CreateSTBIFailureMechanism()
        {
            return new ProbabilisticExpectedFailureMechanismResult("Macrostabiliteit binnenwaarts", MechanismType.STBI, 2);
        }

        private static ProbabilisticExpectedFailureMechanismResult CreateSTPHFailureMechanism()
        {
            return new ProbabilisticExpectedFailureMechanismResult("Piping", MechanismType.STPH, 2);
        }
        #endregion

        #region Group 3

        private static Group3ExpectedFailureMechanismResult CreateAGKFailureMechanism()
        {
            return new Group3ExpectedFailureMechanismResult("Golfklappen op asfaltbekleding", MechanismType.AGK);
        }

        private static Group3ExpectedFailureMechanismResult CreateGEBUFailureMechanism()
        {
            return new Group3ExpectedFailureMechanismResult("Grasbekleding erosie buitentalud", MechanismType.GEBU);
        }

        private static Group3ExpectedFailureMechanismResult CreateZSTFailureMechanism()
        {
            return new Group3ExpectedFailureMechanismResult("Stabiliteit steenzetting", MechanismType.ZST);
        }

        private static Group3ExpectedFailureMechanismResult CreateDAFailureMechanism()
        {
            return new Group3ExpectedFailureMechanismResult("Duinafslag", MechanismType.DA);
        }

        #endregion

        #region Group 4

        private static StbuExpectedFailureMechanismResult CreateSTBUFailureMechanism()
        {
            return new StbuExpectedFailureMechanismResult();
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateSTMIFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Microstabiliteit", MechanismType.STMI, 4);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateAWOFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Wateroverdruk bij asfaltbekleding", MechanismType.AWO, 4);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateGABUFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Grasbekleding afschuiven buitentalud", MechanismType.GABU, 4);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateGABIFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Grasbekleding afschuiven binnentalud", MechanismType.GABI, 4);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreatePKWFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Piping bij kunstwerk", MechanismType.PKW, 4);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateSTKWlFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Sterkte en stabiliteit langsconstructies", MechanismType.STKWl, 4);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateINNFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Technische innovaties", MechanismType.INN, 4);
        }
        #endregion

        #region Group 5

        private static Group4Or5ExpectedFailureMechanismResult CreateVLGAFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Golfafslag voorland", MechanismType.VLGA, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateVLAFFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Afschuiving voorland", MechanismType.VLAF, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateVLZVFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Zettingsvloeiing voorland", MechanismType.VLZV, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateNWObeFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Bebouwing", MechanismType.NWObe, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateNWOboFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Begroeiing", MechanismType.NWObo, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateNWOklFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Kabels en leidingen", MechanismType.NWOkl, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateNWOocFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Overige constructies", MechanismType.NWOoc, 5);
        }

        private static Group4Or5ExpectedFailureMechanismResult CreateHAVFailureMechanism()
        {
            return new Group4Or5ExpectedFailureMechanismResult("Havendammen", MechanismType.HAV, 5);
        }
        #endregion

    }
}
