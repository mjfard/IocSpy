using System;
using System.Collections.Generic;
using System.Linq;
using Pendar.CommonTest.SpyFramework.Interface;

namespace Pendar.CommonTest.SpyFramework
{
    public static class CallInfoListMethods
    {

        public static string ToDetailString(this List<ICallInfo> list)
        {
            var ret = "";
            foreach (var info in list)
            {
                ret += info.ToString() + "\r\n";
            }

            return ret;
        }

        public static List<ICallInfo> OfCallClass(this IEnumerable<ICallInfo> list, Type t)
        {
            return list.Where(c => c.InterfaceType == t).ToList();
        }
    }
}