using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Common.FileSystem;
using Library.CommonIoc.Attributes;
using Library.Common.ObjectExt;
using Pendar.IocLib.Managers;
using Pendar.IocLib.Model;

namespace Pendar.IocLib.Generators
{
    class IocGenerator
    {
        private readonly string _filePath;
        private readonly string _containerNs;
        private readonly NTFSTool _ntfs = new NTFSTool(Console.WriteLine);

        public IocGenerator(string filePath, string containerNs)
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
            _ntfs.CreateTextFileForce(path, new[] {ret.ToString()});
        }

        private void AppendBody(StringBuilder ret, List<GenInfo> infoList)
        {
            foreach (var info in infoList)
            {
                var interfaceList = info.InterfaceList.Except(info.InterfaceList.SelectMany(i => i.GetInterfaces()));

                if (info.Concrete.IsGenericType)
                {   //generic
                    var methodName = $"{info.Count}Generic";
                    var interfaceParams = interfaceList
                        .Select(t => $"typeof({t.NoArgGenericTypeName()})")
                        .AggregateToString(",");

                    ret.Append($@"
            c.{methodName}({interfaceParams}, t.{info.Concrete.TypeName()});");
                }
                else //non generic
                {
                    var methodName = info.Count.ToString();
                    var interfaceParams = interfaceList
                        .Select(TypeExt.TypeName)
                        .AggregateToString(",");

                    ret.Append($@"
            c.{methodName}<{interfaceParams}>(t.{info.Concrete.Name});");
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
$@"using Library.Common.IOC.Interface;

namespace {_containerNs}
{{
    class IocContainerGenConfig
    {{
//########################### this is a generated file ##############################
        public static void Config(IConfigableIocContainer c, IocTypes t)
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