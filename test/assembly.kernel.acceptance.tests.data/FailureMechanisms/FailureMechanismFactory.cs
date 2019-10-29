using System.ComponentModel;
using System.Linq;

namespace assembly.kernel.acceptance.tests.data.FailureMechanisms
{
    public static class FailureMechanismFactory
    {
        // TODO: Remove class and replace with Dictionary<MechanismType, Func<IFailureMechanism>>
        public static readonly FailureMechanismInfo[] Infos = {
            new FailureMechanismInfo("Macrostabiliteit binnenwaarts", MechanismType.STBI, 2, () => CreateSTBIFailureMechanism()),               // Done
            new FailureMechanismInfo("Macrostabiliteit buitenwaarts", MechanismType. STBU, 4, () => CreateSTBUFailureMechanism()),              // Done
            new FailureMechanismInfo("Piping", MechanismType.STPH, 2, () => CreateSTPHFailureMechanism()),                                      // Done
            new FailureMechanismInfo("Microstabiliteit", MechanismType.STMI, 4, () => CreateSTMIFailureMechanism()),                            // Done
            new FailureMechanismInfo("Golfklappen op asfaltbekleding", MechanismType.AGK, 3, () => CreateAGKFailureMechanism()),                // Done
            new FailureMechanismInfo("Wateroverdruk bij asfaltbekleding", MechanismType.AWO, 4, () => CreateAWOFailureMechanism()),             // Done
            new FailureMechanismInfo("Grasbekleding erosie buitentalud", MechanismType.GEBU, 3, () => CreateGEBUFailureMechanism()),            // Done
            new FailureMechanismInfo("Grasbekleding afschuiven buitentalud", MechanismType.GABU, 4, () => CreateGABUFailureMechanism()),        // Done
            new FailureMechanismInfo("Grasbekleding erosie kruin en binnentalud", MechanismType.GEKB, 1, () => CreateGEKBFailureMechanism()),   // Done
            new FailureMechanismInfo("Grasbekleding afschuiven binnentalud", MechanismType.GABI, 4, () => CreateGABIFailureMechanism()),        // Done
            new FailureMechanismInfo("Stabiliteit steenzetting", MechanismType.ZST, 3, () => CreateZSTFailureMechanism()),                      // Done
            new FailureMechanismInfo("Duinafslag", MechanismType.DA, 3, () => CreateDAFailureMechanism()),                                      // Done
            new FailureMechanismInfo("Hoogte kunstwerk", MechanismType.HTKW, 1, () => CreateHTKWFailureMechanism()),                            // Done
            new FailureMechanismInfo("Betrouwbaarheid sluiting kunstwerk", MechanismType.BSKW, 1, () => CreateBSKWFailureMechanism()),          // Done
            new FailureMechanismInfo("Piping bij kunstwerk", MechanismType.PKW, 4, () => CreatePKWFailureMechanism()),                          // Done
            new FailureMechanismInfo("Sterkte en stabiliteit puntconstructies", MechanismType.STKWp, 1, () => CreateSTKWpFailureMechanism()),   // Done
            new FailureMechanismInfo("Sterkte en stabiliteit langsconstructies", MechanismType.STKWl, 4, () => CreateSTKWlFailureMechanism()),  // Done
            new FailureMechanismInfo("Golfafslag voorland", MechanismType.VLGA, 5, () => CreateVLGAFailureMechanism()),                         // Done
            new FailureMechanismInfo("Afschuiving voorland", MechanismType.VLAF, 5, () => CreateVLAFFailureMechanism()),                        // Done
            new FailureMechanismInfo("Zettingsvloeiing voorland", MechanismType.VLZV, 5, () => CreateVLZVFailureMechanism()),                   // Done
            new FailureMechanismInfo("Bebouwing", MechanismType.NWObe, 5, () => CreateNWObeFailureMechanism()),                                 // Done
            new FailureMechanismInfo("Begroeiing", MechanismType.NWObo, 5, () => CreateNWOboFailureMechanism()),                                // Done
            new FailureMechanismInfo("Kabels en leidingen", MechanismType.NWOkl, 5, () => CreateNWOklFailureMechanism()),                       // Done
            new FailureMechanismInfo("Overige constructies", MechanismType.NWOoc, 5, () => CreateNWOocFailureMechanism()),                      // Done
            new FailureMechanismInfo("Havendammen", MechanismType.HAV, 5, () => CreateHAVFailureMechanism()),                                   // Done
            new FailureMechanismInfo("Technische innovatie", MechanismType.INN, 4, () => CreateINNFailureMechanism())                           // Done
        };

        #region Group 1
        public static ProbabilisticFailureMechanism CreateGEKBFailureMechanism()
        {
            return new ProbabilisticFailureMechanism("Grasbekleding erosie kruin en binnentalud", MechanismType.GEKB, 1);
        }

        public static ProbabilisticFailureMechanism CreateHTKWFailureMechanism()
        {
            return new ProbabilisticFailureMechanism("Hoogte kunstwerk", MechanismType.HTKW, 1);
        }

        public static ProbabilisticFailureMechanism CreateBSKWFailureMechanism()
        {
            return new ProbabilisticFailureMechanism("Betrouwbaarheid sluiting kunstwerk", MechanismType.BSKW, 1);
        }

        public static ProbabilisticFailureMechanism CreateSTKWpFailureMechanism()
        {
            return new ProbabilisticFailureMechanism("Sterkte en stabiliteit punconstructies", MechanismType.STKWp, 1);
        }

        #endregion

        #region Group 2
        public static ProbabilisticFailureMechanism CreateSTBIFailureMechanism()
        {
            return new ProbabilisticFailureMechanism("Macrostabiliteit binnenwaarts", MechanismType.STBI, 2);
        }

        public static ProbabilisticFailureMechanism CreateSTPHFailureMechanism()
        {
            return new ProbabilisticFailureMechanism("Piping", MechanismType.STPH, 2);
        }
        #endregion

        #region Group 3

        public static Group3FailureMechanism CreateAGKFailureMechanism()
        {
            return new Group3FailureMechanism("Golfklappen op asfaltbekleding", MechanismType.AGK);
        }

        public static Group3FailureMechanism CreateGEBUFailureMechanism()
        {
            return new Group3FailureMechanism("Grasbekleding erosie buitentalud", MechanismType.GEBU);
        }

        public static Group3FailureMechanism CreateZSTFailureMechanism()
        {
            return new Group3FailureMechanism("Stabiliteit steenzetting", MechanismType.ZST);
        }

        public static Group3FailureMechanism CreateDAFailureMechanism()
        {
            return new Group3FailureMechanism("Duinafslag", MechanismType.DA);
        }

        #endregion

        #region Group 4

        public static STBUFailureMechanism CreateSTBUFailureMechanism()
        {
            return new STBUFailureMechanism();
        }

        public static Group4Or5FailureMechanism CreateSTMIFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Microstabiliteit", MechanismType.STMI, 4);
        }

        public static Group4Or5FailureMechanism CreateAWOFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Wateroverdruk bij asfaltbekleding", MechanismType.AWO, 4);
        }

        public static Group4Or5FailureMechanism CreateGABUFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Grasbekleding afschuiven buitentalud", MechanismType.GABU, 4);
        }

        public static Group4Or5FailureMechanism CreateGABIFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Grasbekleding afschuiven binnentalud", MechanismType.GABI, 4);
        }

        public static Group4Or5FailureMechanism CreatePKWFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Piping bij kunstwerk", MechanismType.PKW, 4);
        }

        public static Group4Or5FailureMechanism CreateSTKWlFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Sterkte en stabiliteit langsconstructies", MechanismType.STKWl, 4);
        }

        public static Group4Or5FailureMechanism CreateINNFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Technische innovaties", MechanismType.INN, 4);
        }
        #endregion

        #region Group 5
        public static Group4Or5FailureMechanism CreateVLGAFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Golfafslag voorland", MechanismType.VLGA, 5);
        }

        public static Group4Or5FailureMechanism CreateVLAFFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Afschuiving voorland", MechanismType.VLAF, 5);
        }

        public static Group4Or5FailureMechanism CreateVLZVFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Zettingsvloeiing voorland", MechanismType.VLZV, 5);
        }

        public static Group4Or5FailureMechanism CreateNWObeFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Bebouwing", MechanismType.NWObe, 5);
        }

        public static Group4Or5FailureMechanism CreateNWOboFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Begroeiing", MechanismType.NWObo, 5);
        }

        public static Group4Or5FailureMechanism CreateNWOklFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Kabels en leidingen", MechanismType.NWOkl, 5);
        }

        public static Group4Or5FailureMechanism CreateNWOocFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Overige constructies", MechanismType.NWOoc, 5);
        }

        public static Group4Or5FailureMechanism CreateHAVFailureMechanism()
        {
            return new Group4Or5FailureMechanism("Havendammen", MechanismType.HAV, 5);
        }
        #endregion

        public static IFailureMechanism CreateFailureMechanism(MechanismType type)
        {
            var info = Infos.FirstOrDefault(i => i.Type == type);
            if (info == null)
            {
                throw new InvalidEnumArgumentException();
            }

            return info.CreationFunc();
        }
    }
}
