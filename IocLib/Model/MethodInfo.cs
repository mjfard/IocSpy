using System.Collections.Generic;

namespace Pendar.IocLib.Model
{
    class MethodInfo : IMemberInfo
    {
        public string MethoName;
        public string MethodGenericName;
        public string InterfaceGenericName;
        public List<InputInfo> Inputs;
        public List<OutPutInfo> Outputs;
    }
}
