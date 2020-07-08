using System;
using Library.CommonIoc.Attributes;

namespace Pendar.IocLib.Model
{
    class GenInfo
    {
        public string SpyName;
        public string SpyGenericName;
        public Type Concrete;
        public Count? Count;
        public Type[] InterfaceList;

        public GenInfo(Type concrete, Type[] interfaceList, Count? count, string spyName = null)
        {
            SpyName = spyName ?? Methods.SpyName(concrete);
            SpyGenericName = Methods.SpyGenericName(concrete, spyName);
            Concrete = concrete;
            Count = count;
            InterfaceList = interfaceList;
        }


    }
}
