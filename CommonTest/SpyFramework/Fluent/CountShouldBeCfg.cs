using Pendar.CommonTest.SpyFramework.Fluent.Model;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class CountShouldBeCfg<TInterface> : BaseNewHookCfg<TInterface>
    {
        private readonly CfgInfo _cfgInfo;

        internal CountShouldBeCfg(CfgInfo cfgInfo, int expectedCount):base(cfgInfo)
        {
            _cfgInfo = cfgInfo;
            cfgInfo.Hook.CallCount = expectedCount;
        }

    }
}
