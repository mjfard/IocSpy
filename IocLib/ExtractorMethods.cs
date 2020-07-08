using Pendar.IocLib.Model;

namespace Pendar.IocLib
{
    internal static class ExtractorMethods
    {
        public static string SpyFileName(GenInfo g)
        {
            var argsCount = g.Concrete.GetGenericArguments().Length;
            if (argsCount == 0)
                return g.SpyName;
            return $"{g.SpyName}({argsCount})";
        }
    }
}