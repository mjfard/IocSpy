using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Common.FileSystem;
using Library.Common.ObjectExt;
using Pendar.IocLib.Managers;

namespace Pendar.IocLib.Generators
{
    class IocTypesGenerator
    {
        private readonly string _path;
        private readonly string _ns;
        private readonly NTFSTool _ntfs = new NTFSTool(Console.WriteLine);

        public IocTypesGenerator(string path, string ns)
        {
            _path = path;
            _ns = ns;
        }

        public void GenerateAndStore(List<Type> concreteTypeList)
        {
            var ret = new StringBuilder();

            AppendHeader(ret, concreteTypeList);
            AppendBody(ret, concreteTypeList);
            AppendFooter(ret);

            _ntfs.CreateTextFileForce(_path, new[] {ret.ToString()});
        }

        private void AppendBody(StringBuilder ret, List<Type> concreteTypeList)
        {
            foreach (var type in concreteTypeList)
            {
                ret.Append($"        public readonly Type {type.TypeName()} = typeof({type.NoArgGenericTypeName()});\r\n");
            }
        }

        private void AppendHeader(StringBuilder ret, List<Type> concreteTypeList)
        {
            var usingManager = new UsingManager();
            foreach (var type in concreteTypeList)
            {
                usingManager.AddUsingOf(type);
            }
            foreach (var uzing in usingManager.Result)
                ret.Append($"using {uzing};\r\n");
            ret.Append(
$@"using System;
//########################### this is a generated file ##############################
namespace {_ns}
{{
    public class IocTypes
    {{
");
        }
        private static void AppendFooter(StringBuilder ret)
        {
            ret.Append($@"
    }}
}}
            ");
        }

    }
}
