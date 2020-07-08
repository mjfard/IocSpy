using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Common.FileSystem;
using Library.Common.ObjectExt;
using Pendar.IocLib.Managers;
using Pendar.IocLib.Model;

namespace Pendar.IocLib.Generators
{
    class SpyIocGenerator
    {
        private readonly string _filePath;
        private readonly string _spyContainerNs;
        private readonly string _spiedNs;
        private readonly NTFSTool _ntfs = new NTFSTool(Console.WriteLine);

        public SpyIocGenerator(string filePath, string spyContainerNs, string spiedNs)
        {
            _filePath = filePath;
            _spyContainerNs = spyContainerNs;
            _spiedNs = spiedNs;
        }

        public void GenerateAndStore(List<GenInfo> infoList)
        {
            var ret = new StringBuilder();

            AppendHeader(ret, infoList);
            AppendBody(ret, infoList);
            AppendFooter(ret);

            var path = $"{_filePath}";
            _ntfs.CreateTextFileForce(path, new[] { ret.ToString() });
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
            c.{methodName}(typeof({info.SpyName}<>), {interfaceParams}, new ConcreteGenericMaker(t.{info.Concrete.TypeName()}));");
                }
                else //non generic
                {
                    var methodName = info.Count.ToString();
                    var interfaceParams = interfaceList
                        .Select(TypeExt.TypeName)
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
using Library.Core.Common;
using {_spiedNs};

namespace {_spyContainerNs}
{{
    class SpyIocContainerGenConfig
    {{
//########################### this is a generated file ##############################
        public static void Configure(IConfigableSpyIocContainer c, IocTypes t)
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