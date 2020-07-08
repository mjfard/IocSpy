using System;
using System.Linq;
using Library.CommonIoc.Helpers;

namespace Pendar.IocSpyGen
{
    class Methods
    {
        public static string SpyName(Type t)
        {
            var typeName = t.Name.HeadUntilFirstIndexOrAll("`");
            return $"{typeName}Spy";
        }

        public static string SpyGenericName(Type t, string spyName = null)
        {
            if (spyName == null)
                spyName = SpyName(t);
            if(t.IsGenericType)
                return $"{spyName}<{t.GetGenericArguments().Select(ReflectionHelper.TypeName).AggregateToString(", ")}>";
            return spyName;
        }



    }
}
