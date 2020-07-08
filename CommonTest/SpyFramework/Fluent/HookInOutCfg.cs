using System;
using Pendar.CommonTest.SpyFramework.Fluent.Model;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class HookInOutCfg<TInterface, TParam> : BaseNewHookCfg<TInterface>
    {
        private readonly CfgInfo _cfgInfo;

        internal HookInOutCfg(CfgInfo cfgInfo,  HookType hookType, int paramNo, Action<TParam> assert) : base(cfgInfo)
        {
            _cfgInfo = cfgInfo;
            var hook = cfgInfo.Hook;

            hook.ParamType = typeof(TParam);
            hook.HookType = hookType;
            hook.ParamNo = paramNo;
            hook.ParamDelegate = ToNonGeneric(assert);
        }


        public CountShouldBeCfg<TInterface> CountShouldBe(int expectedCount) => new CountShouldBeCfg<TInterface>(_cfgInfo, expectedCount);
        public WhenCfg<TInterface> When(Func<ICallInfo<TInterface>, bool> cond) => new WhenCfg<TInterface>(_cfgInfo, cond);



        private static Action<object> ToNonGeneric(Action<TParam> paramDelegate)
        {
            if (paramDelegate == null)
                return null;
            return c => paramDelegate((TParam)c);
        }
    }
}
