using System.ComponentModel;
using System.Linq;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanisms
{
    public static class FailureMechanismFactory
    {
        // TODO: Remove class and replace with Dictionary<MechanismType, Func<IFailureMechanismResult>>
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
        public static ProbabilisticFailureMechanismResult CreateGEKBFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Grasbekleding erosie kruin en binnentalud", MechanismType.GEKB, 1);
        }

        public static ProbabilisticFailureMechanismResult CreateHTKWFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Hoogte kunstwerk", MechanismType.HTKW, 1);
        }

        public static ProbabilisticFailureMechanismResult CreateBSKWFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Betrouwbaarheid sluiting kunstwerk", MechanismType.BSKW, 1);
        }

        public static ProbabilisticFailureMechanismResult CreateSTKWpFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Sterkte en stabiliteit punconstructies", MechanismType.STKWp, 1);
        }

        #endregion

        #region Group 2
        public static ProbabilisticFailureMechanismResult CreateSTBIFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Macrostabiliteit binnenwaarts", MechanismType.STBI, 2);
        }

        public static ProbabilisticFailureMechanismResult CreateSTPHFailureMechanism()
        {
            return new ProbabilisticFailureMechanismResult("Piping", MechanismType.STPH, 2);
        }
        #endregion

        #region Group 3

        public static Group3FailureMechanismResult CreateAGKFailureMechanism()
        {
            return new Group3FailureMechanismResult("Golfklappen op asfaltbekleding", MechanismType.AGK);
        }

        public static Group3FailureMechanismResult CreateGEBUFailureMechanism()
        {
            return new Group3FailureMechanismResult("Grasbekleding erosie buitentalud", MechanismType.GEBU);
        }

        public static Group3FailureMechanismResult CreateZSTFailureMechanism()
        {
            return new Group3FailureMechanismResult("Stabiliteit steenzetting", MechanismType.ZST);
        }

        public static Group3FailureMechanismResult CreateDAFailureMechanism()
        {
            return new Group3FailureMechanismResult("Duinafslag", MechanismType.DA);
        }

        #endregion

        #region Group 4

        public static StbuFailureMechanismResult CreateSTBUFailureMechanism()
        {
            return new StbuFailureMechanismResult();
        }

        public static Group4Or5FailureMechanismResult CreateSTMIFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Microstabiliteit", MechanismType.STMI, 4);
        }

        public static Group4Or5FailureMechanismResult CreateAWOFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Wateroverdruk bij asfaltbekleding", MechanismType.AWO, 4);
        }

        public static Group4Or5FailureMechanismResult CreateGABUFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Grasbekleding afschuiven buitentalud", MechanismType.GABU, 4);
        }

        public static Group4Or5FailureMechanismResult CreateGABIFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Grasbekleding afschuiven binnentalud", MechanismType.GABI, 4);
        }

        public static Group4Or5FailureMechanismResult CreatePKWFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Piping bij kunstwerk", MechanismType.PKW, 4);
        }

        public static Group4Or5FailureMechanismResult CreateSTKWlFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Sterkte en stabiliteit langsconstructies", MechanismType.STKWl, 4);
        }

        public static Group4Or5FailureMechanismResult CreateINNFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Technische innovaties", MechanismType.INN, 4);
        }
        #endregion

        #region Group 5
        public static Group4Or5FailureMechanismResult CreateVLGAFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Golfafslag voorland", MechanismType.VLGA, 5);
        }

        public static Group4Or5FailureMechanismResult CreateVLAFFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Afschuiving voorland", MechanismType.VLAF, 5);
        }

        public static Group4Or5FailureMechanismResult CreateVLZVFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Zettingsvloeiing voorland", MechanismType.VLZV, 5);
        }

        public static Group4Or5FailureMechanismResult CreateNWObeFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Bebouwing", MechanismType.NWObe, 5);
        }

        public static Group4Or5FailureMechanismResult CreateNWOboFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Begroeiing", MechanismType.NWObo, 5);
        }

        public static Group4Or5FailureMechanismResult CreateNWOklFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Kabels en leidingen", MechanismType.NWOkl, 5);
        }

        public static Group4Or5FailureMechanismResult CreateNWOocFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Overige constructies", MechanismType.NWOoc, 5);
        }

        public static Group4Or5FailureMechanismResult CreateHAVFailureMechanism()
        {
            return new Group4Or5FailureMechanismResult("Havendammen", MechanismType.HAV, 5);
        }
        #endregion

        public static IFailureMechanismResult CreateFailureMechanism(MechanismType type)
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
