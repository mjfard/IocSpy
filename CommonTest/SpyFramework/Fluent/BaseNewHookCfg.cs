using System;
using Pendar.CommonTest.SpyFramework.Fluent.Model;

namespace Pendar.CommonTest.SpyFramework.Fluent
{
    public class BaseNewHookCfg<TInterface>
    {
        private readonly CfgInfo _cfgInfo;

        internal BaseNewHookCfg(CfgInfo cfgInfo)
        {
            _cfgInfo = cfgInfo;
        }
        public HookInOutCfg<TInterface, TParam> Input_0_<TParam>(Action<TParam> checks) 
            => new InterfaceCfg<TInterface>(_cfgInfo.HookAdder)
            .Method(i => _cfgInfo.Hook.MethodOrProp)
            .Input_0_(checks);
        public HookInOutCfg<TInterface, TParam> Input_1_<TParam>(Action<TParam> checks) 
            => new InterfaceCfg<TInterface>(_cfgInfo.HookAdder)
            .Method(i => _cfgInfo.Hook.MethodOrProp)
            .Input_1_(checks);

        public HookInOutCfg<TInterface, TParam> Output_0_<TParam>(Action<TParam> checks) 
            => new InterfaceCfg<TInterface>(_cfgInfo.HookAdder)
            .Method(i => _cfgInfo.Hook.MethodOrProp)
            .Output_0_(checks);
        public HookInOutCfg<TInterface, TParam> Output_1_<TParam>(Action<TParam> checks) 
            => new InterfaceCfg<TInterface>(_cfgInfo.HookAdder)
            .Method(i => _cfgInfo.Hook.MethodOrProp)
            .Output_1_(checks);

        public MethodCfg<TInterface> Method(Func<TInterface, string> methodProvider) 
            => new InterfaceCfg<TInterface>(_cfgInfo.HookAdder).Method(methodProvider);

    }
}
