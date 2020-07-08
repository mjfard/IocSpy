using System;
using Autofac;
using Library.CommonIoc.Interface;

namespace Library.CommonIoc.Autofac
{
    public class AutoFacMultiScopeIocBuilder : AutofacNonScopedIocBuilder, IIocContainerBuilder
    {
        public void SinglePerScope(Type implementer)
        {
            Builder.RegisterType(implementer)
                .InstancePerLifetimeScope();
        }

        public void SinglePerScope<TInt>(Type implementer)
        {
            Builder.RegisterType(implementer)
                .AsSelf()
                .As<TInt>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScope<TInt1, TInt2>(Type implementer)
        {
            Builder.RegisterType(implementer)
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScope<TImp>()
        {
            Builder.RegisterType<TImp>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScope<TImp, TInt>()
        {
            Builder.RegisterType<TImp>()
                .AsSelf()
                .As<TInt>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScope<TImp, TInt1, TInt2>()
        {
            Builder.RegisterType<TImp>()
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScopeByFactory<T>(Func<IIocContainer, T> factory, params Type[] interfaces) where T : class
        {
            var regBuilder = Builder.Register(c => factory(new AutofacIocContainer(c)));
            foreach (var @interface in interfaces)
                regBuilder = regBuilder.As(@interface);
            regBuilder.InstancePerLifetimeScope();
        }

        public void SinglePerScopeByFactory<T, TInt>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .AsSelf()
                .As<TInt>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScopeByFactory<T, TInt1, TInt2>(
            Func<IIocContainer, T> factory
        ) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>()
                .InstancePerLifetimeScope();
        }

        public void SinglePerScopeGeneric(
            Type interfaceOpenType,
            Type implementerOpenType
        )
        {
            Builder.RegisterGeneric(implementerOpenType)
                .As(interfaceOpenType)
                .InstancePerLifetimeScope();
        }
    }
}