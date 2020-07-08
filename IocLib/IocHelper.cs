using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library.Common.FileSystem;
using Library.CommonIoc.Attributes;
using Pendar.IocLib.Generators;
using Pendar.IocLib.Model;

namespace Pendar.IocLib
{
    public class IocHelper
    {
        private readonly string _containerConfigDir;
        private readonly string _spyContainerConfigDir;
        private readonly string _spiesDir;
        private readonly List<Assembly> _asmList;
        private readonly string _containerNs;
        private readonly string _spyContainerNs;
        private readonly string _spiesNs;

        public IocHelper(
            string containerConfigDir, 
            string spyContainerConfigDir, 
            string spiesDir,
            List<Assembly> asmList,
            string containerNs,
            string spyContainerNs,
            string spiesNs)
        {
            _containerConfigDir = containerConfigDir;
            _spyContainerConfigDir = spyContainerConfigDir;
            _spiesDir = spiesDir;
            _asmList = asmList;
            _containerNs = containerNs;
            _spyContainerNs = spyContainerNs;
            _spiesNs = spiesNs;
        }
        public void Gen(bool genSpy)
        {
            var allGenInfos = GetGenInfos();
            var ntfs = new NTFSTool(Console.WriteLine);
            var iocInfoList = allGenInfos.Where(i => i.Count != null).ToList();
            new IocGenerator($"{_containerConfigDir}\\IocContainerGenConfig.cs", _containerNs).GenerateAndStore(iocInfoList);
            new IocTypesGenerator($"{_containerConfigDir}\\IocTypes.cs", _containerNs).GenerateAndStore(iocInfoList.Select(i => i.Concrete).ToList());
            if (genSpy)
            {
                var extractor = new Extractor(allGenInfos);
                var generator = new SpyGenerator(_spiesDir,_spiesNs);
                ntfs.DeleteDirectoryFiles(_spiesDir);
                var spyInfoList = extractor.Extract();
                foreach (var info in spyInfoList)
                {
                    generator.GenerateAndStore(info);
                }
                new SpyIocGenerator($"{_spyContainerConfigDir}\\SpyIocContainer.cs", _spyContainerNs, _spiesNs).GenerateAndStore(iocInfoList);
            }

        }

        private List<GenInfo> GetGenInfos()
        {
            var ret = new List<GenInfo>();
            foreach (var t in _asmList.SelectMany(asm => asm.DefinedTypes))
            {
                if (t.GetCustomAttributes(typeof(IOCAttribute), false).FirstOrDefault() is IOCAttribute iocAttr)
                { 
                    if (t.IsInterface)
                        throw new Exception($"'IocAttribute can not be applied to interface '{t.Name}'");
                    ret.Add(new GenInfo(t, t.GetInterfaces(), iocAttr.Count));
                }
                if (t.GetCustomAttributes(typeof(SpiedAttribute), false).FirstOrDefault() is SpiedAttribute spiedAttr)
                {
                    var interfaceArray = t.GetInterfaces();
                    if(t.IsInterface)
                        interfaceArray= interfaceArray.Concat(new []{t}).ToArray();

                    ret.Add(new GenInfo(t, interfaceArray, null, spiedAttr.SpyName));
                }
            }

            return ret;
        }
    }
}