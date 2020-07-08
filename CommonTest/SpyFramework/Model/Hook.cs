using System;
using Library.Common.ObjectExt;
using Pendar.CommonTest.SpyFramework.Interface;

namespace Pendar.CommonTest.SpyFramework.Model
{
    public enum HookType
    {
        Input,
        Output,
        AfterCall,
        BeforeCall,
    };
    public class Hook
    {
        public MemberType MemberType { get; set;}
        public Type InterfaceType { get; set;}
        public Type ParamType { get; set;}
        public HookType HookType { get; set;}
        public int ParamNo { get; set;}
        public string MethodOrProp { get; set;}
        public Func<ICallInfo, bool> Cond { get; set;}
        public Action<object> ParamDelegate { get; set;}
        public Action<ICallInfo> CallDelegate { get; set;}
        public int? CallCount { get; set; }

        public int ActualCount = 0;

        public override string ToString()
        {
            var temp = $"{InterfaceType.TypeGenericName()}.{MethodOrProp} => {HookType}";
            if (HookType != HookType.AfterCall)
                temp += $"_{ParamNo}_<{ParamType.TypeGenericName()}>";
            return temp;
        }

        public bool HasCallCountError()
        {
            return CallCount != null && CallCount != ActualCount || CallCount == null && ActualCount == 0;
        }
    }
}