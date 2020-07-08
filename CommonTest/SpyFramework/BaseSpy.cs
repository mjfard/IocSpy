using System;
using System.Collections.Generic;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework
{
    public class BaseSpy : ISpy
    {
        protected SpyMaster Master;
        protected Object Orig;
        public void _Init(object orig, SpyMaster master)
        {
            Master = master ?? throw new Exception($"MasterSpy provided to spy of type '{GetType().Name}' is null");
            Orig = orig ?? throw new Exception($"Original object provided to spy of type '{GetType().Name}' is null");
        }



        public ICallInfo _CreateInfo<TInterface>(string methodName, params object[] inputs) => Master._CreateInfo(MemberType.Method, (TInterface)Orig, methodName, inputs);
        public ICallInfo _CreateSetterInfo<TInterface>(string methodName, params object[] inputs) => Master._CreateInfo(MemberType.Setter,(TInterface) Orig, methodName, inputs);
        public ICallInfo _CreateGetterInfo<TInterface>(string methodName, params object[] inputs) => Master._CreateInfo(MemberType.Getter, (TInterface)Orig, methodName, inputs);

        public void _CompleteInfo(ICallInfo info, params object[] outputs) => Master._CompleteInfo(info, outputs);


        private Dictionary<object, object> _subSpyDic;

        public T SubSpy<T>(object orig) where T : ISpy
        {
            if (orig == null)
                return default(T);
            if (orig is ISpy)
                return (T)orig;
            if (_subSpyDic == null)
                _subSpyDic = new Dictionary<object, object>();
            if (_subSpyDic.ContainsKey(orig))
                return (T) _subSpyDic[orig];
            var ret = (T) Activator.CreateInstance(typeof(T));
            ((ISpy)ret)._Init(orig, Master);
            _subSpyDic.Add(orig, ret);
            return ret;
        }
    }
}