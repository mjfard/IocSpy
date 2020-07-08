using System;
using System.Linq;
using Library.Common.ObjectExt;

namespace Pendar.IocLib
{
    class Methods
    {
        public static string SpyName(Type t)
        {
            var typeName = t.Name.HeadUntilFirstIndexOrAll("`");
            var ret = $"{typeName}Spy";
            if (ret.StartsWith("I"))
                ret = ret.Substring(1);
            return ret;
        }

        public static string SpyGenericName(Type t, string spyName = null)
        {
            if (spyName == null)
                spyName = SpyName(t);
            if(t.IsGenericType)
                return $"{spyName}<{t.GetGenericArguments().Select(TypeExt.TypeName).AggregateToString(", ")}>";
            return spyName;
        }



    }
}
