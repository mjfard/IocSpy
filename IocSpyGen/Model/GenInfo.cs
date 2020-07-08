using System;
using Library.CommonIoc.Attributes;
using Library.CommonIoc.Enums;

namespace Pendar.IocSpyGen.Model
{
    class GenInfo
    {
        public string SpyName;
        public string SpyGenericName;
        public Type Concrete;
        public LifeTime? LifeTime;
        public Type[] InterfaceList;

        public GenInfo(Type concrete, Type[] interfaceList, LifeTime? lifeTime, string spyName = null)
        {
            SpyName = spyName ?? Methods.SpyName(concrete);
            SpyGenericName = Methods.SpyGenericName(concrete, spyName);
            Concrete = concrete;
            LifeTime = lifeTime;
            InterfaceList = interfaceList;
        }


    }
}
