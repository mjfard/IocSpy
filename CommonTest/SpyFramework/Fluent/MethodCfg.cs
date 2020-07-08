using System;
using Pendar.CommonTest.SpyFramework.Fluent.Model;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class MethodCfg<TInterface> 
    {
        private readonly CfgInfo _cfgInfo;

        internal MethodCfg(CfgInfo cfgInfo, Func<TInterface, string> methodProvider)
        {
            _cfgInfo = cfgInfo;
            var h = cfgInfo.Hook;

            h.MethodOrProp = methodProvider(default(TInterface));
            h.MemberType = MemberType.Method;
            h.HookType = HookType.AfterCall;
        }
        public HookInOutCfg<TInterface, TParam> Input_0_<TParam>(Action<TParam> checks)
            => new HookInOutCfg<TInterface, TParam>(_cfgInfo, HookType.Input, 0, checks);

        public HookInOutCfg<TInterface, TParam> Input_1_<TParam>(Action<TParam> checks)
            => new HookInOutCfg<TInterface, TParam>(_cfgInfo, HookType.Input, 1, checks);

        public HookInOutCfg<TInterface, TParam> Output_0_<TParam>(Action<TParam> checks)
            => new HookInOutCfg<TInterface, TParam>(_cfgInfo, HookType.Output, 0, checks);

        public HookInOutCfg<TInterface, TParam> Output_1_<TParam>(Action<TParam> checks)
            => new HookInOutCfg<TInterface, TParam>(_cfgInfo, HookType.Output, 1, checks);

        public HookCallCfg<TInterface> AfterCall(Action<ICallInfo<TInterface>> checks)
            => new HookCallCfg<TInterface>(_cfgInfo, HookType.AfterCall, checks);



        public WhenCfg<TInterface> When(Func<ICallInfo<TInterface>, bool> cond) => new WhenCfg<TInterface>(_cfgInfo, cond);
        public CountShouldBeCfg<TInterface> CountShouldBe(int expectedCount) => new CountShouldBeCfg<TInterface>(_cfgInfo, expectedCount);

    }
}
