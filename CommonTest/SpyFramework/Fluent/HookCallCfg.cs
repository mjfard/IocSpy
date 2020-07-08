using System;
using Pendar.CommonTest.SpyFramework.Fluent.Model;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class HookCallCfg<TInterface> : BaseNewHookCfg<TInterface>
    {
        private readonly CfgInfo _cfgInfo;

        internal HookCallCfg(
            CfgInfo cfgInfo,
            HookType hookType,
            Action<ICallInfo<TInterface>> callDelegate) : base(cfgInfo)
        {
            _cfgInfo = cfgInfo;
            var hook = cfgInfo.Hook;

            hook.HookType = hookType;
            hook.CallDelegate = ToNonGeneric(callDelegate);
        }

        public CountShouldBeCfg<TInterface> CountShouldBe(int expectedCount) => new CountShouldBeCfg<TInterface>(_cfgInfo, expectedCount);
        public WhenCfg<TInterface> When(Func<ICallInfo<TInterface>, bool> cond) => new WhenCfg<TInterface>(_cfgInfo, cond);

        private static Action<ICallInfo> ToNonGeneric(Action<ICallInfo<TInterface>> callDelegate)
        {
            if (callDelegate == null)
                return null;
            return c => callDelegate((ICallInfo<TInterface>) c);
        }
    }
}