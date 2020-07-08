using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pendar.CommonTest.SpyFramework.Fluent;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework
{
    public abstract class SpiedTest
    {
        public Exception SetupException { get; set; }
        public Exception SpySetupException { get; set; }

        protected SpyMaster Master { get; } = new SpyMaster();

        private List<Hook> _hooks = new List<Hook>();

        [TestFixtureSetUp]
        public void BaseSetup()
        {
            try
            {
                SpySetUp();
                _hooks.ForEach(h => Master.AddHook(h));
            }
            catch (Exception e)
            {
                SpySetupException = e;
            }

            try
            {
                Setup();
            }
            catch (Exception e)
            {
                SetupException = e;
            }
        }

        public virtual void SpySetUp()
        {
        }

        public abstract void Setup();

        protected InterfaceCfg<TInterface> Interface<TInterface>() => new InterfaceCfg<TInterface>(h => _hooks.Add(h));
        protected void AfterMethod(Type interfaceType, string methodName, Action<ICallInfo> callDelegate, int? callCount = null, Func<ICallInfo, bool> cond = null)
        {
            _hooks.Add(new Hook()
            {
                HookType = HookType.AfterCall,
                InterfaceType = interfaceType,
                MethodOrProp = methodName,
                Cond = cond,
                CallDelegate = callDelegate,
                CallCount = callCount,
            });
        }

//        protected void AfterMethod<TInterface>(Func<TInterface, string> methodSelector, Action<ICallInfo<TInterface>> callDelegate, int? callCount = null, Func<ICallInfo<TInterface>, bool> cond = null)
//        {
//            Master.AddHook(new Hook()
//            {
//                HookType = HookType.AfterCall,
//                InterfaceType = typeof(TInterface),
//                MethodOrProp = methodSelector(default(TInterface)),
//                Cond = ToNonGeneric(cond),
//                CallDelegate = ToNonGeneric(callDelegate),
//                CallCount = callCount,
//            });
//        }

        protected void SetterInput<TInterface, TProp>(Func<TInterface, string> prop, Action<TProp> assert, int? callCount = null, Func<ICallInfo<TInterface>, bool> cond = null)
        {
            Master.AddHook(new Hook()
            {
                HookType = HookType.Input,
                InterfaceType = typeof(TInterface),
                ParamType = typeof(TProp),
                ParamNo = 0,
                MethodOrProp = prop(default(TInterface)),
                Cond = ToNonGeneric(cond),
                ParamDelegate = o => assert((TProp) o),
                CallCount = callCount,
            });
        }

        protected void GetterOutput<TInterface, TProp>(Func<TInterface, string> prop, Action<TProp> assert, int? callCount = null, Func<ICallInfo<TInterface>, bool> cond = null)
        {
            Master.AddHook(new Hook()
            {
                HookType = HookType.Output,
                InterfaceType = typeof(TInterface),
                ParamType = typeof(TProp),
                ParamNo = 0,
                MethodOrProp = prop(default(TInterface)),
                Cond = ToNonGeneric(cond),
                ParamDelegate = o => assert((TProp) o),
                CallCount = callCount,
            });
        }

        protected void Check(bool checkResult, string title = null, object hint = null)
        {
            Master.Check(checkResult, title, hint);
        }

        protected void FinalCheck(Func<bool> checker, string title = null, Func<string> hintProvider = null)
        {
            Master.AddFinalChecker(new FinalChecker()
            {
                Checker = checker,
                Title = title,
                HintProvider = hintProvider,
            });
        }

        public virtual void __No_error()
        {
            if (SetupException != null)
                throw new Exception("", SetupException);
        }

        public virtual void _NoSpyError()
        {
            if (SpySetupException != null)
                throw new Exception("", SpySetupException);
            Master.AssertSpyResult();
        }


        public virtual void No_SPY_error_has_occured()
        {
            if (SpySetupException != null)
                throw new Exception("", SpySetupException);
        }
        private static Func<ICallInfo, bool> ToNonGeneric<TInterface>(Func<ICallInfo<TInterface>, bool> cond)
        {
            if (cond == null)
                return null;
            return c => cond((ICallInfo<TInterface>)c);
        }



    }
}