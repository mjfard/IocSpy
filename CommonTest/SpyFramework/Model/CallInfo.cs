using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pendar.CommonTest.SpyFramework.Interface;

namespace Pendar.CommonTest.SpyFramework.Model
{
    public enum MemberType { Method, Getter, Setter}
    public class CallInfo<TInterface> : ICallInfo<TInterface>, IInternalCallInfo
    {
        public MemberType MemberType { get; set; }
        public string MethodOrProp { get; set; }
        public object[] Inputs { get; set; }
        public object[] Outputs { get; set; }
        public Type InterfaceType { get; set; }
        public int? Order { get; set; }
        public object OrigObject => Orig;
        public T Output_0_<T>() => (T) Outputs[0];
        public T Output_1_<T>() => (T) Outputs[1];
        public T Input_0_<T>() => (T) Inputs[0];
        public T Input_1_<T>() => (T) Inputs[1];
        public T Input_2_<T>() => (T) Inputs[2];


        public TInterface Orig { get; set; }

        public IEnumerable<Hook> ElemCheckers { set; get; }

        public CallInfo()
        {
            
        }
        public override string ToString()
        {
            return $"[Method: '{MethodOrProp}' , Type: {InterfaceType.Name}]";
        }

        public T SingleP<T>() => (T) Inputs.Single();

        public void AssertMethodIs<TInterface>(string methodName)
        {
            Assert.AreEqual(methodName, MethodOrProp, "MethodName:");
            Assert.IsTrue(typeof(TInterface).IsAssignableFrom(InterfaceType), "Class:");
        }
    }
}
