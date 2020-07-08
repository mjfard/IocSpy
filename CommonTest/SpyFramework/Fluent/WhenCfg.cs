using System;
using Pendar.CommonTest.SpyFramework.Fluent.Model;
using Pendar.CommonTest.SpyFramework.Interface;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class WhenCfg<TInterface> : BaseNewHookCfg<TInterface>
    {
        private readonly CfgInfo _cfgInfo;

        internal WhenCfg(CfgInfo cfgInfo, Func<ICallInfo<TInterface>, bool> cond) : base(cfgInfo)
        {
            _cfgInfo = cfgInfo;
            var hook = cfgInfo.Hook;
            if (cond == null)
                hook.Cond =  null;
            hook.Cond =  c => cond((ICallInfo<TInterface>)c);
        }
        public CountShouldBeCfg<TInterface> CountShouldBe(int expectedCount) => new CountShouldBeCfg<TInterface>(_cfgInfo, expectedCount);

    }
}
