using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Common.FileSystem;
using Library.Common.ObjectExt;
using Pendar.IocLib.Model;

namespace Pendar.IocLib.Generators
{
    class SpyGenerator
    {
        private readonly string _destDir;
        private readonly string _spiedNs;
        private readonly NTFSTool _ntfs = new NTFSTool(Console.WriteLine);

        public SpyGenerator(string destDir, string spiedNs)
        {
            _destDir = destDir;
            _spiedNs = spiedNs;
        }

        public void GenerateAndStore(SpyGenInfo info)
        {
            var ret = new StringBuilder();

            AppendHeader(ret, info);
            AppendProps(ret, info.MethodInfos.OfType<PropInfo>().ToList());
            AppendMethods(ret, info.MethodInfos.OfType<MethodInfo>().ToList());
            AppendFooter(ret);

            var path = $"{_destDir}\\{info.FileName}.cs";
            _ntfs.CreateTextFileForce(path, new[] {ret.ToString()});
        }

        private static void AppendFooter(StringBuilder ret)
        {
            ret.Append($@"
    }}
}}
            ");
        }

        private void AppendHeader(StringBuilder ret, SpyGenInfo i)
        {
            foreach (var uzing in i.Usings)
            {
                ret.Append($"using {uzing};\r\n");
            }

            ret.Append($@"
using Pendar.CommonTest.SpyFramework;

namespace {_spiedNs}
{{
    public class {i.SpyGenericName} : BaseSpy, {i.InterfaceList.AggregateToString(",")}{i.GenericConstraints}
    {{
            ");
        }

        private void AppendProps(StringBuilder ret, List<PropInfo> props)
        {
            foreach (var p in props)
            {
                ret.Append($@"
        {p.RetType} {p.InterfaceGenericName}.{p.PropName}
        {{
            get
            {{
                var info = _CreateGetterInfo<{p.InterfaceGenericName}>(""{p.PropName}"");
//########################### this is a SPY ##############################
                var ret = (({p.InterfaceGenericName})Orig).{p.PropName};
//########################################################################
                _CompleteInfo(info, ret);
                return ret;
            }}"
                );
                if (p.HasSetter)
                    ret.Append($@"
            set
            {{
                var info = _CreateSetterInfo<{p.InterfaceGenericName}>(""{p.PropName}"", value);
//########################### this is a SPY ##############################
                (({p.InterfaceGenericName})Orig).{p.PropName} = value;
//########################################################################
                _CompleteInfo(info);
            }}"
                    );
                ret.Append($@"
        }}
            ");
            }
        }

        public void AppendMethods(StringBuilder ret, List<MethodInfo> methodItems)
        {
            foreach (var item in methodItems)
            {
                var paramsDeclare = item.Inputs.Select(i => $"{i.Type} {i.Name}")
                    .Union(item.Outputs.Where(o => o.Name != null).Select(o => $"out {o.Type} {o.Name}"))
                    .AggregateToString(", ");

                var infoInputs = item.Inputs.Select(i => $"{i.Provision}").AggregateToString(", ");
                var commaIfHasInputs = item.Inputs.Any() ? ", " : "";

                var infoOutputs = item.Outputs.Select(i => $"{i.Provision??"ret"}").AggregateToString(", ");
                var commaIfHasOutputs = item.Outputs.Any() ? ", " : "";

                var redirectParams = item.Inputs.Select(i => $"{i.Provision}")
                    .Union(item.Outputs.Where(o => o.Name != null).Select(o => $"out {o.Name}"))
                    .AggregateToString(", ");


                var retVar = "";
                var retReturn = "";

                var retInfo = item.Outputs.SingleOrDefault(o => o.Name == null);
                if (retInfo != null)
                {
                    retVar = "var ret = ";
                    retReturn = "\r\n\t\t\treturn ret;";
                }

                ret.Append($@"
        {retInfo?.Type ?? "void"} {item.InterfaceGenericName}.{item.MethodGenericName}({paramsDeclare})
        {{
            var info = _CreateInfo<{item.InterfaceGenericName}>(""{item.MethoName}""{commaIfHasInputs}{infoInputs});
//########################### this is a SPY ##############################
            {retVar}(({item.InterfaceGenericName})Orig).{item.MethodGenericName}({redirectParams});
//########################################################################
            _CompleteInfo(info{commaIfHasOutputs}{infoOutputs});{retReturn}
        }}    
                ");
            }
        }
    }
}