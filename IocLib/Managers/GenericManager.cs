using System;
using System.Linq;
using System.Reflection;
using Library.Common.ObjectExt;

namespace Pendar.IocLib.Managers
{
    class GenericManager
    {
        private readonly UsingManager _usingManager;

        public GenericManager(UsingManager usingManager)
        {
            _usingManager = usingManager;
        }

        public string TypeGenericName(Type t, System.Reflection.MethodInfo m = null)
        {
            if (t.IsGenericType)
            {
                _usingManager.AddUsingOf(t);
                return $"{t.Name.HeadUntilFirstIndexOrAll("`")}<{t.GenericTypeArguments.Select(a => TypeGenericName(a, m)).AggregateToString(", ")}>";
            }
            _usingManager.AddUsingOf(t);
            return t.Name == "Void"? null : TypeExt.TypeName(t);
        }

        public string TypeGenericArgs(Type t, System.Reflection.MethodInfo m = null)
        {
            if (t.IsGenericType)
            {
                _usingManager.AddUsingOf(t);
                return $"<{t.GenericTypeArguments.Select(a => TypeGenericName(a, m)).AggregateToString(", ")}>";
            }
            _usingManager.AddUsingOf(t);
            return "";
        }

        public string GetTypeConstraints(Type @interface)
        {
            var ret = "";
            foreach (var arg in @interface.GetGenericArguments())
            {
                var consts = arg.GetGenericParameterConstraints();
                if (consts.Length > 0 || arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                {
                    var items = consts.Select(t => TypeGenericName(t)).ToList();
                    if (arg.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                        items.Add("new()");
                    ret += $" where {TypeExt.TypeName(arg)} : {items.AggregateToString(",")}";
                }
            }
            return ret;
        }
    }
}