using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library.CommonIoc.Attributes;
using Library.CommonIoc.Interface;
using Pendar.IocSpyGen.Model;

namespace Pendar.IocSpyGen
{
    static class IocHelperMethods
    {
        public static List<GenInfo> GetGenInfoList(Assembly asm)
        {
            var ret = new List<GenInfo>();
            foreach (var t in asm.DefinedTypes)
            {
                if (t.GetCustomAttributes(typeof(IIocAttribute), false).FirstOrDefault() is IIocAttribute iocAttr)
                {
                    if (t.IsInterface)
                        throw new Exception($"'IocAttribute can not be applied to interface '{t.Name}'");
                    ret.Add(new GenInfo(t, t.GetInterfaces(), iocAttr.Lifetime));
                }
                if (t.GetCustomAttributes(typeof(SpiedAttribute), false).FirstOrDefault() is SpiedAttribute spiedAttr)
                {
                    var interfaceArray = t.GetInterfaces();
                    if (t.IsInterface)
                        interfaceArray = interfaceArray.Concat(new[] { t }).ToArray();

                    ret.Add(new GenInfo(t, interfaceArray, null, spiedAttr.SpyName));
                }
            }

            return ret;
        }

    }
}
