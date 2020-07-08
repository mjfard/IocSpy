using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.CommonIoc.Helpers;
using Pendar.IocSpyGen.Managers;
using Pendar.IocSpyGen.Model;

namespace Pendar.IocSpyGen.Generators
{
    class IocConfigGenerator
    {
        private readonly string _filePath;
        private readonly string _containerNs;

        public IocConfigGenerator(string filePath, string containerNs)
        {
            _filePath = filePath;
            _containerNs = containerNs;
        }

        public void GenerateAndStore(List<GenInfo> infoList)
        {
            var ret = new StringBuilder();

            AppendHeader(ret, infoList);
            AppendBody(ret, infoList);
            AppendFooter(ret);

            var path = $"{_filePath}";
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
            b.{methodName}({interfaceParams}, t.{info.Concrete.TypeName()});");
                }
                else //non generic
                {
                    var methodName = info.LifeTime.ToString();
                    var interfaceParams = interfaceList
                        .Select(ReflectionHelper.TypeName)
                        .AggregateToString(",");

                    ret.Append($@"
            b.{methodName}<{interfaceParams}>(t.{info.Concrete.Name});");
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

            ret.Append(
$@"using Library.CommonIoc.Interface;

namespace {_containerNs}
{{
    class IocConfigGen
    {{
//########################### this is a generated file ##############################
        public static void Config(IIocContainerBuilder b, IocTypesGen t)
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