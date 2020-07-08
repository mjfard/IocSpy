using System;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework.Interface
{
    public interface ICallInfo
    {
        MemberType MemberType { get;  }
        string MethodOrProp { get;  }
        Object[] Inputs { get;  }
        Object[] Outputs { get; }
        Type InterfaceType { get;  }
        int? Order { get; }
        object OrigObject { get; }
        T Output_0_<T>();
        T Output_1_<T>();
        T Input_0_<T>();
        T Input_1_<T>();
        T Input_2_<T>();
    }
    public interface ICallInfo<out TInterface>:ICallInfo
    {
        TInterface Orig { get; }

    }
}
