using System;
using System.Collections.Generic;
using System.Linq;
using Library.CommonIoc.Helpers;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework
{

    public class SpyMaster
    {
        private readonly DeepCloneManager _cloneManager = new DeepCloneManager();
        readonly List<ICallInfo> _calls = new List<ICallInfo>();
        readonly HookRepository _hooks = new HookRepository();
        readonly SpyResultManager _spyResultManager;

        public SpyMaster()
        {
            _spyResultManager = new SpyResultManager(_hooks);
        }

        public void DeepClone<T>(params string[] methods) where T : ISpy
        {
            _cloneManager.AddCloneTarget(typeof(T), methods);
        }
        public void DeepClone(Type interfaceType, params string[] methods) 
        {
            _cloneManager.AddCloneTarget(interfaceType, methods);
        }

        public ICallInfo _CreateInfo<TInterface>(MemberType memberType, TInterface orig, string methodName, params object[] inputs)
        {
            var interfaceType = typeof(TInterface);
            var elemCheckers = _hooks.GetHooks(interfaceType, methodName)
                .Where(ch => ch.MemberType == memberType && 
                             (ch.InterfaceType.IsUnConstructedGenericType() || ch.InterfaceType.IsInstanceOfType(orig)))
                .ToList();

            var ret = new CallInfo<TInterface>()
            {
                MemberType = memberType,
                MethodOrProp = methodName,
                Inputs = inputs,
                InterfaceType = interfaceType,
                Orig = orig,
                ElemCheckers = elemCheckers,
            };

            if (DoSpy)
            {
                foreach (var h in elemCheckers.Where(ch => ch.HookType == HookType.Input))
                    _spyResultManager.ParamHook(h, inputs, ret);

                foreach (var h in elemCheckers.Where(ch => ch.HookType == HookType.BeforeCall))
                    _spyResultManager.CallHook(h, ret);
            }


            _calls.Add(ret);
            return ret;
        }
        public void _CompleteInfo(ICallInfo info, params object[] outputs)
        {
            var internalInfo = (IInternalCallInfo) info;

            internalInfo.Outputs = outputs;

            if (DoSpy)
            {
                foreach (var h in internalInfo.ElemCheckers.Where(ch => ch.HookType == HookType.Output))
                    _spyResultManager.ParamHook(h, outputs, info);

                foreach (var h in internalInfo.ElemCheckers.Where(ch => ch.HookType == HookType.AfterCall))
                    _spyResultManager.CallHook(h , info);
            }
        }


        public void AddHook(Hook hook)
        {
            _hooks.Add(hook);
        }
        public void Check(bool checkResult, string title, object hint)
        {
            _spyResultManager.AddCheckResult(checkResult, title, hint);
        }
        public void AddFinalChecker(FinalChecker finalChecker)
        {
            _spyResultManager.AddFinalChecker(finalChecker);
        }

        public void AssertSpyResult()
        {
            _spyResultManager.AssertSpyResult();
        }

        #region obsolete

        public IEnumerable<ICallInfo> All<TInterface>(string methodName = null, int? paramCount = null)
        {
            var type = typeof(TInterface);
            if (!type.IsInterface)
                throw new Exception("Only interface type is accepted");
            var ret = _calls.Where(c => typeof(TInterface).IsAssignableFrom(c.InterfaceType));
            if (paramCount != null)
                ret = ret.Where(c => c.Inputs.Length == paramCount);
            if (methodName != null)
                ret = ret.Where(c => c.MethodOrProp == methodName);
            return ret;
        }
        public IEnumerable<ICallInfo> All(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
                throw new Exception("Only interface type is accepted");
            if (!interfaceType.IsConstructedGenericType)
            {
                return _calls.Where(c => c.InterfaceType.IsOfGenericInterfaceDefinition(interfaceType));
            }
            return _calls.Where(c => interfaceType.IsAssignableFrom(c.InterfaceType)).ToList();
        }


        public IEnumerable<ICallInfo> All(Type interfaceType, string methodName)
        {
            if (!interfaceType.IsConstructedGenericType)
            {
                return _calls.Where(c => c.InterfaceType.IsOfGenericInterfaceDefinition(interfaceType) && c.MethodOrProp == methodName);
            }
            return _calls.Where(c => c.InterfaceType == interfaceType && c.MethodOrProp == methodName);
        }

        public IEnumerable<ICallInfo> All(string methodName) => _calls.Where(c => c.MethodOrProp == methodName).ToList();

        public ICallInfo Single(string methodName) => _calls.SingleOrDefault(c => c.MethodOrProp == methodName);

        public ICallInfo Single<TInterface>(string methodName) =>
            _calls.Single(c => typeof(TInterface).IsAssignableFrom(c.InterfaceType) && c.MethodOrProp == methodName);

        public T Single_Ret<T>(string methodName)
        {
            var method = _calls.SingleOrDefault(m => m.MethodOrProp == methodName);
            if (method == null)
                throw new Exception($"No method named as '{methodName}' is called");
            return (T)method.Outputs.Single();
        }

        public bool DoSpy { set; get; } = true;

        public virtual void ClearMethodCalls()
        {
            _calls.Clear();
        }
        #endregion

    }
}
