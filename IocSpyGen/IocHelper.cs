using System.Collections.Generic;
using System.Linq;
using Library.CommonIoc.Helpers;
using Pendar.IocSpyGen.Generators;
using Pendar.IocSpyGen.Model;

namespace Pendar.IocSpyGen
{
    public class IocHelper
    {
        private readonly List<CsProjInfo> _csProjInfoList;
        private readonly string _testProjPath;
        private readonly string _testProjDefaultNs;

        public IocHelper(List<CsProjInfo> csProjInfoList, string testProjPath, string testProjDefaultNs)
        {
            _csProjInfoList = csProjInfoList;
            _testProjPath = testProjPath;
            _testProjDefaultNs = testProjDefaultNs;
        }

        public void Gen(bool genSpy)
        {

            var baseTestPath = $"{_testProjPath}\\_IocSpy";
            var baseTestNs = $"{_testProjDefaultNs}._IocSpy";

            foreach (var proj in _csProjInfoList)
            {
                var baseProjPath = $"{proj.Path}\\_Ioc";
                var projName = proj.DefaultNs.Split('.').Last();

                MemoryDiskHelper.CreateDirIfNotExists(baseProjPath);

                var genInfoList = IocHelperMethods.GetGenInfoList(proj.Asembly);

                var iocInfoList = genInfoList.Where(i => i.LifeTime != null).ToList();

                var iocNs = $"{proj.DefaultNs}._Ioc";

                new IocConfigGenerator($"{baseProjPath}\\IocConfigGen.cs", iocNs)
                    .GenerateAndStore(iocInfoList);

                new IocTypesGenerator($"{baseProjPath}\\IocTypesGen.cs", iocNs)
                    .GenerateAndStore(iocInfoList.Select(i => i.Concrete).ToList());

                if (genSpy)
                {

                    var spiesDir = $"{baseTestPath}\\{projName}\\SpiesGen";
                    var spiesNs = $"{baseTestNs}.{projName}.SpiesGen";

                    MemoryDiskHelper.CreateDirIfNotExists(spiesDir);

                    var extractor = new Extractor(genInfoList);
                    var generator = new SpyGenerator(spiesDir, spiesNs);

                    MemoryDiskHelper.EmptyDirRecursive(spiesDir);

                    var spyInfoList = extractor.Extract();
                    foreach (var info in spyInfoList)
                        generator.GenerateAndStore(info);

                    new SpyIocConfigGenerator(
                        $"{baseTestPath}\\{projName}\\SpyIocConfigGen.cs",
                        $"{baseTestNs}.{projName}",
                        spiesNs,
                        iocNs
                        
                    ).GenerateAndStore(iocInfoList);
                }
            }
        }
    }
}