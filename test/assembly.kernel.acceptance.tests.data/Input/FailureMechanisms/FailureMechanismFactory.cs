using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public static class FailureMechanismFactory
    {
        private static readonly Dictionary<MechanismType, Func<IFailureMechanismResult>> Infos =
            new Dictionary<MechanismType, Func<IFailureMechanismResult>>
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

        public static IFailureMechanismResult CreateFailureMechanism(MechanismType type)
        {
            if (!Infos.ContainsKey(type))
            {
                throw new InvalidEnumArgumentException();
            }

            return Infos[type]();
        }

        #region Group 1

        private static ProbabilisticFailureMechanismResult CreateGEKBFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Grasbekleding erosie kruin en binnentalud", MechanismType.GEKB, 1);
        }

        private static ProbabilisticFailureMechanismResult CreateHTKWFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Hoogte kunstwerk", MechanismType.HTKW, 1);
        }

        private static ProbabilisticFailureMechanismResult CreateBSKWFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Betrouwbaarheid sluiting kunstwerk", MechanismType.BSKW, 1);
        }

        private static ProbabilisticFailureMechanismResult CreateSTKWpFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Sterkte en stabiliteit punconstructies", MechanismType.STKWp, 1);
        }

        #endregion

        #region Group 2

        private static ProbabilisticFailureMechanismResult CreateSTBIFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Macrostabiliteit binnenwaarts", MechanismType.STBI, 2);
        }

        private static ProbabilisticFailureMechanismResult CreateSTPHFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Piping", MechanismType.STPH, 2);
        }
        #endregion

        #region Group 3

        private static Group3FailureMechanismResult CreateAGKFailureMechanism()
        {
            return new Group3FailureMechanismResult("Golfklappen op asfaltbekleding", MechanismType.AGK);
        }

        private static Group3FailureMechanismResult CreateGEBUFailureMechanism()
        {
            return new Group3FailureMechanismResult("Grasbekleding erosie buitentalud", MechanismType.GEBU);
        }

        private static Group3FailureMechanismResult CreateZSTFailureMechanism()
        {
            return new Group3FailureMechanismResult("Stabiliteit steenzetting", MechanismType.ZST);
        }

        private static Group3FailureMechanismResult CreateDAFailureMechanism()
        {
            return new Group3FailureMechanismResult("Duinafslag", MechanismType.DA);
        }

        #endregion

        #region Group 4

        private static StbuFailureMechanismResult CreateSTBUFailureMechanism()
        {
            return new StbuFailureMechanismResult();
        }

        private static Group4Or5FailureMechanismResult CreateSTMIFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Microstabiliteit", MechanismType.STMI, 4);
        }

        private static Group4Or5FailureMechanismResult CreateAWOFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Wateroverdruk bij asfaltbekleding", MechanismType.AWO, 4);
        }

        private static Group4Or5FailureMechanismResult CreateGABUFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Grasbekleding afschuiven buitentalud", MechanismType.GABU, 4);
        }

        private static Group4Or5FailureMechanismResult CreateGABIFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Grasbekleding afschuiven binnentalud", MechanismType.GABI, 4);
        }

        private static Group4Or5FailureMechanismResult CreatePKWFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Piping bij kunstwerk", MechanismType.PKW, 4);
        }

        private static Group4Or5FailureMechanismResult CreateSTKWlFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Sterkte en stabiliteit langsconstructies", MechanismType.STKWl, 4);
        }

        private static Group4Or5FailureMechanismResult CreateINNFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Technische innovaties", MechanismType.INN, 4);
        }
        #endregion

        #region Group 5

        private static Group4Or5FailureMechanismResult CreateVLGAFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Golfafslag voorland", MechanismType.VLGA, 5);
        }

        private static Group4Or5FailureMechanismResult CreateVLAFFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Afschuiving voorland", MechanismType.VLAF, 5);
        }

        private static Group4Or5FailureMechanismResult CreateVLZVFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Zettingsvloeiing voorland", MechanismType.VLZV, 5);
        }

        private static Group4Or5FailureMechanismResult CreateNWObeFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Bebouwing", MechanismType.NWObe, 5);
        }

        private static Group4Or5FailureMechanismResult CreateNWOboFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Begroeiing", MechanismType.NWObo, 5);
        }

        private static Group4Or5FailureMechanismResult CreateNWOklFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Kabels en leidingen", MechanismType.NWOkl, 5);
        }

        private static Group4Or5FailureMechanismResult CreateNWOocFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Overige constructies", MechanismType.NWOoc, 5);
        }

        private static Group4Or5FailureMechanismResult CreateHAVFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Havendammen", MechanismType.HAV, 5);
        }
        #endregion

    }
}
