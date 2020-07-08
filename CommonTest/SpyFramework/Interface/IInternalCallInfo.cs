using System;
using System.Collections.Generic;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework.Interface
{
    public interface IInternalCallInfo
    {
        IEnumerable<Hook> ElemCheckers { get; }
        Object[] Outputs { get; set; }

    }
}
