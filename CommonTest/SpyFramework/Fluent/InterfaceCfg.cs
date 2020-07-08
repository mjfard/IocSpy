using System;
using Pendar.CommonTest.SpyFramework.Fluent.Model;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class InterfaceCfg<TInterface>
    {
        private CfgInfo _cfgInfo { get; }

        internal InterfaceCfg(Action<Hook> hookAdder)
        {
            _cfgInfo = new CfgInfo()
            {
                Hook = new Hook()
                {
                    InterfaceType = typeof(TInterface),
                },
                HookAdder = hookAdder,
            };
            hookAdder(_cfgInfo.Hook);
        }

        public MethodCfg<TInterface> Method(Func<TInterface, string> methodProvider) => new MethodCfg<TInterface>(_cfgInfo, methodProvider);


    }
}