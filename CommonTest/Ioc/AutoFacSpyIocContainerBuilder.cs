using System;
using System.Linq;
using Autofac;
using Autofac.Core;
using Pendar.CommonTest.SpyFramework;
using Pendar.CommonTest.SpyFramework.Interface;

namespace Pendar.CommonTest.Ioc
{
    public class AutoFacSpyIocContainerBuilder : ISpyIocContainerBuilder
    {
        private readonly SpyMaster _master;
        private IContainer _container;
        private readonly ContainerBuilder _builder = new ContainerBuilder();

        public AutoFacSpyIocContainerBuilder(SpyMaster master)
        {
            _master = master;
        }

        public void SingleInstanceNoSpy<TInterface>(TInterface instance) where TInterface : class
        {
            _builder.RegisterInstance(instance).As<TInterface>().SingleInstance();
        }
        public void TransientGeneric(Type spyOpenType, Type intOpenType, ConcreteGenericMaker maker)
        {
            _builder.RegisterGeneric(maker.ConcreteOpenType);

            _builder.RegisterGeneric(spyOpenType)
                .OnActivated(args => ((ISpy)args.Instance)._Init(maker.MakeInstance(args), _master))
                .As(intOpenType);
        }

        public void SinglePerScopeGeneric(Type spyOpenType, Type intOpenType, ConcreteGenericMaker maker)
        {
            _builder.RegisterGeneric(maker.ConcreteOpenType).SingleInstance();

            _builder.RegisterGeneric(spyOpenType)
                .OnActivated(args => ((ISpy)args.Instance)._Init(maker.MakeInstance(args), _master))
                .As(intOpenType)
                .SingleInstance();
        }

        public void SinglePerScope<TSpy>(Type concrete, params Type[] interfaces) where TSpy : ISpy, new()
        {
            _builder.RegisterType(concrete).SingleInstance();

            var spyRegBuilder = _builder.Register(c =>
            {
                var spy = new TSpy();
                spy._Init(c.Resolve(concrete), _master);
                return spy;
            });
            foreach (var @interface in interfaces)
            {
                spyRegBuilder = spyRegBuilder.As(@interface);
            }
            spyRegBuilder.SingleInstance();
        }

        public void SinglePerScopeByInstance<TSpy, TInterface>(TInterface instance) where TSpy : ISpy, TInterface, new()
        {
            _builder.Register<TInterface>(c =>
            {
                var spy = new TSpy();
                spy._Init(instance, _master);
                return spy;
            }).SingleInstance();
        }

        public void SinglePerScope<TSpy, TInterface>(Type concrete) where TSpy : ISpy, TInterface, new()
            => SinglePerScope<TSpy, TInterface>(concrete, null);
        public void SinglePerScope<TSpy, TInterface>(Type concrete, Action<IActivatedEventArgs<object>> onActivated = null) where TSpy : ISpy, TInterface, new()
        {
            var temp = _builder.RegisterType(concrete).SingleInstance();
            if (onActivated != null)
                temp.OnActivated(onActivated);

            _builder.Register<TInterface>(c =>
            {
                var spy = new TSpy();
                spy._Init(c.Resolve(concrete), _master);
                return spy;
            }).SingleInstance();
        }

        public void SinglePerScope<TSpy, TInt1, TInt2>(Type concrete) where TSpy : ISpy, TInt1, TInt2, new()
            => SinglePerScope<TSpy, TInt1, TInt2>(concrete, new Parameter[0]);
        public void SinglePerScope<TSpy, TInt1, TInt2>(Type concrete, params Parameter[] paramList)
            where TSpy : ISpy, TInt1, TInt2, new()
        {
            var temp = _builder.RegisterType(concrete).SingleInstance();
            if (paramList.Any())
                temp.WithParameters(paramList);

            _builder.Register(c =>
            {
                var spy = new TSpy();
                spy._Init(c.Resolve(concrete), _master);
                return spy;
            })
                .As<TInt1>()
                .As<TInt2>()
                .SingleInstance();
        }

        public void Singleton<TSpy, TInt1, TInt2>(Type concrete) where TSpy : ISpy, TInt1, TInt2, new()
            => Singleton<TSpy, TInt1, TInt2>(concrete, new Parameter[0]);
        public void Singleton<TSpy, TInt1, TInt2>(Type concrete, params Parameter[] paramList) where TSpy : ISpy, TInt1, TInt2, new()
        {
            var temp = _builder.RegisterType(concrete).SingleInstance();
            if (paramList.Any())
                temp.WithParameters(paramList);

            _builder.Register(c =>
                {
                    var spy = new TSpy();
                    spy._Init(c.Resolve(concrete), _master);
                    return spy;
                })
                .As<TInt1>()
                .As<TInt2>()
                .SingleInstance();
        }

        public void Singleton<TSpy, TInterface>(Type concrete) where TSpy : ISpy, TInterface, new()
            => Singleton<TSpy, TInterface>(concrete, new Parameter[0]);
        public void Singleton<TSpy, TInterface>(Type concrete, params Parameter[] paramList) where TSpy : ISpy, TInterface, new()
        {
            var temp = _builder.RegisterType(concrete).SingleInstance();
            if (paramList.Any())
                temp.WithParameters(paramList);

            _builder.Register(c =>
                {
                    var spy = new TSpy();
                    spy._Init(c.Resolve(concrete), _master);
                    return spy;
                })
                .As<TInterface>()
                .SingleInstance();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _container.Resolve(t);
        }
        public void Build()
        {
            _container = _builder.Build();
        }
    }

}
