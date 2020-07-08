using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.CommonIoc.Helpers;
using Pendar.IocSpyGen.Managers;
using Pendar.IocSpyGen.Model;

namespace Pendar.IocSpyGen.Generators
{
    class SpyIocConfigGenerator
    {
        private readonly string _spyIocConfigPath;
        private readonly string _spyIocConfigNs;
        private readonly string _spiesNs;
        private readonly string _iocTypesNs;

        public SpyIocConfigGenerator(string spyIocConfigPath, string spyIocConfigNs, string spiesNs, string iocTypesNs)
        {
            _spyIocConfigPath = spyIocConfigPath;
            _spyIocConfigNs = spyIocConfigNs;
            _spiesNs = spiesNs;
            _iocTypesNs = iocTypesNs;
        }

        public void GenerateAndStore(List<GenInfo> infoList)
        {
            var ret = new StringBuilder();

            AppendHeader(ret, infoList);
            AppendBody(ret, infoList);
            AppendFooter(ret);

            var path = $"{_spyIocConfigPath}";
            MemoryDiskHelper.CreateTextFileForce(path, ret.ToString());
        }




        private void AppendBody(StringBuilder ret, List<GenInfo> infoList)
        {
            foreach (var info in infoList)
            {
                var interfaceList = info.InterfaceList.Except(info.InterfaceList.SelectMany(i => i.GetInterfaces()));

                if (info.Concrete.IsGenericType)
                {   //generic
                    var methodName = $"{info.LifeTime}Generic";
                    var interfaceParams = interfaceList
                        .Select(t => $"typeof({t.NoArgGenericTypeName()})")
                        .AggregateToString(",");

                    ret.Append($@"
            c.{methodName}(typeof({info.SpyName}<>), {interfaceParams}, new ConcreteGenericMaker(t.{info.Concrete.TypeName()}));");
                }
                else //non generic
                {
                    var methodName = info.LifeTime.ToString();
                    var interfaceParams = interfaceList
                        .Select(ReflectionHelper.TypeName)
                        .AggregateToString(",");

                    ret.Append($@"
            c.{methodName}<{info.SpyName},  {interfaceParams}>(t.{info.Concrete.TypeName()});");
                }
            }

            ret.Append("\r\n");
        }

        private void AppendHeader(StringBuilder ret, List<GenInfo> infoList)
        {
            var usingManager = new UsingManager();
            foreach (var info in infoList)
            {
                usingManager.AddUsingOf(info.Concrete);
                info.InterfaceList.ForEach(usingManager.AddUsingOf);
            }

            foreach (var uzing in usingManager.Result)
                ret.Append($"using {uzing};\r\n");
            ret.Append($@"
using Pendar.CommonTest.SpyFramework;
using Pendar.CommonTest.SpyFramework.Interface;
using {_iocTypesNs};
using {_spiesNs};
using Pendar.CommonTest.Ioc;

namespace {_spyIocConfigNs}
{{
    class SpyIocConfigGen
    {{
//########################### this is a generated file ##############################
        public static void Configure(ISpyIocContainerBuilder c, IocTypesGen t)
        {{
            ");
        }
        private static void AppendFooter(StringBuilder ret)
        {
            ret.Append($@"
        }}
    }}
}}
            ");
        }
    }
}