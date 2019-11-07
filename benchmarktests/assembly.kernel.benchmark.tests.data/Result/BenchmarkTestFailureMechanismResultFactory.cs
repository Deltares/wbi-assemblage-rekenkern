using System;
using System.Collections.Generic;
using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;

namespace assembly.kernel.benchmark.tests.data.Result
{
    public static class BenchmarkTestFailureMechanismResultFactory
    {
        private static readonly Dictionary<MechanismType, Tuple<string,int>> Infos =
            new Dictionary<MechanismType, Tuple<string, int>>
            {
                {MechanismType.STBI, new Tuple<string, int>("Macrostabiliteit binnenwaarts", 2)},
                {MechanismType.STBU, new Tuple<string, int>("Macrostabiliteit buitenwaarts", 4)},
                {MechanismType.STPH, new Tuple<string, int>("Piping", 2)},
                {MechanismType.STMI, new Tuple<string, int>("Microstabiliteit", 4)},
                {MechanismType.AGK, new Tuple<string, int>("Golfklappen op asfaltbekleding", 3)},
                {MechanismType.AWO, new Tuple<string, int>("Wateroverdruk bij asfaltbekleding", 4)},
                {MechanismType.GEBU, new Tuple<string, int>("Grasbekleding erosie buitentalud", 3)},
                {MechanismType.GABU, new Tuple<string, int>("Grasbekleding afschuiven buitentalud", 4)},
                {MechanismType.GEKB, new Tuple<string, int>("Grasbekleding erosie kruin en binnentalud", 1)},
                {MechanismType.GABI, new Tuple<string, int>("Grasbekleding afschuiven binnentalud", 4)},
                {MechanismType.ZST, new Tuple<string, int>("Stabiliteit steenzetting", 3)},
                {MechanismType.DA, new Tuple<string, int>("Duinafslag", 3)},
                {MechanismType.HTKW, new Tuple<string, int>("Hoogte kunstwerk", 1)},
                {MechanismType.BSKW, new Tuple<string, int>("Betrouwbaarheid sluiting kunstwerk", 1)},
                {MechanismType.PKW, new Tuple<string, int>("Piping bij kunstwerk", 4)},
                {MechanismType.STKWp, new Tuple<string, int>("Sterkte en stabiliteit punconstructies", 1)},
                {MechanismType.STKWl, new Tuple<string, int>("Sterkte en stabiliteit langsconstructies", 4)},
                {MechanismType.VLGA, new Tuple<string, int>("Golfafslag voorland", 5)},
                {MechanismType.VLAF, new Tuple<string, int>("Afschuiving voorland", 5)},
                {MechanismType.VLZV, new Tuple<string, int>("Zettingsvloeiing voorland", 5)},
                {MechanismType.NWObe, new Tuple<string, int>("Bebouwing", 5)},
                {MechanismType.NWObo, new Tuple<string, int>("Begroeiing", 5)},
                {MechanismType.NWOkl, new Tuple<string, int>("Kabels en leidingen", 5)},
                {MechanismType.NWOoc, new Tuple<string, int>("Overige constructies", 5)},
                {MechanismType.HAV, new Tuple<string, int>("Havendammen", 5)},
                {MechanismType.INN, new Tuple<string, int>("Technische innovaties", 4)}
            };

        public static BenchmarkFailureMechanismTestResult CreateFailureMechanismTestResult(MechanismType type)
        {
            if (!Infos.ContainsKey(type))
            {
                throw new InvalidEnumArgumentException();
            }

            var mechanismInformation = Infos[type];
            return new BenchmarkFailureMechanismTestResult(mechanismInformation.Item1, type, mechanismInformation.Item2);
        }

    }
}
