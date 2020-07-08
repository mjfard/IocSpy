using System;
using System.Collections.Generic;
using System.Text;
using Library.CommonIoc.Helpers;
using Pendar.IocSpyGen.Managers;

namespace Pendar.IocSpyGen.Generators
{
    class IocTypesGenerator
    {
        private readonly string _path;
        private readonly string _ns;

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

            MemoryDiskHelper.CreateTextFileForce(_path, ret.ToString());
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
    public class IocTypesGen
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
