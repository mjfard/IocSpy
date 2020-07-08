using System.Collections.Generic;

namespace Pendar.IocLib.Model
{
    class SpyGenInfo
    {
        public string SpyGenericName;

        public List<string> Usings;

//        public string InterfaceName;
        public List<string> InterfaceList;
        public string GenericConstraints;
        public string FileName;

        public List<IMemberInfo> MethodInfos;
    }
}
