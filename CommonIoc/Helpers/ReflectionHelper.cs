using System;
using System.Linq;
using System.Reflection;

namespace Library.CommonIoc.Helpers
{
    public static class ReflectionHelper
    {
        public static bool IsUnConstructedGenericType(this Type type)
        {
            if (type.IsGenericType)
                return !type.IsConstructedGenericType;
            return false;
        }

        public static bool IsOfGenericInterfaceDefinition(this Type type, Type def)
        {
            return type.GetInterfaces()
                .Where(t => t.IsGenericType)
                .Select(t => t.GetGenericTypeDefinition())
                .Any(d => d == def);
        }

        public static string TypeGenericName(this Type @interface)
        {
            if (!@interface.IsGenericType)
                return @interface.Name;
            var str = @interface.GetGenericArguments().Select(TypeName).AggregateToString(", ");
            return $"{@interface.Name.HeadUntilFirstIndexOrAll("`")}<{str}>";
        }

        public static string TypeName(this Type type)
        {
            return type.Name.Replace("&", "").HeadUntilFirstIndexOrAll("`");
        }

        public static string MethodGenericName(this MethodInfo m)
        {
            if (!m.IsGenericMethod)
                return m.Name;
            var str = m.GetGenericArguments().Select(TypeName).AggregateToString(", ");
            return $"{m.Name}<{str}>";
        }

        public static string NoArgGenericTypeName(this Type t)
        {
            return t.IsGenericType ? $"{t.TypeName()}<>" : t.TypeName();
        }
    }
}
