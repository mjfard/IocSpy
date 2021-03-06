﻿using System.Collections.Generic;

namespace Pendar.IocSpyGen.Model
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
