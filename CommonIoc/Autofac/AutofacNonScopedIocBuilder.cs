using System;
using Autofac;
using Autofac.Core;
using Library.CommonIoc.Interface;

namespace Library.CommonIoc.Autofac
{
    public class AutofacNonScopedIocBuilder : INonScopedIocBuilder
    {
        protected readonly ContainerBuilder Builder = new ContainerBuilder();

        public void Transient(Type implementer)
        {
            Builder.RegisterType(implementer);
        }

        public void Transient<TInt>(Type implementer)
        {
            Builder.RegisterType(implementer)
                .AsSelf()
                .As<TInt>();
        }

        public void Transient<TInt1, TInt2>(Type implementer)
        {
            Builder.RegisterType(implementer)
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>();
        }

        public void Transient<TImp>()
        {
            Builder.RegisterType<TImp>();
        }

        public void Transient<TImp, TInt>()
        {
            Builder.RegisterType<TImp>()
                .AsSelf()
                .As<TInt>();
        }

        public void Transient<TImp, TInt1, TInt2>()
        {
            Builder.RegisterType<TImp>()
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>();
        }

        public void TransientByFactory<T>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)));
        }

        public void TransientByFactory<T, TInt>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .AsSelf()
                .As<TInt>();
        }

        public void TransientByFactory<T, TInt1, TInt2>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>();
        }

        public void TransientGeneric(Type interfaceOpenType, Type implementerOpenType)
        {
            Builder.RegisterGeneric(implementerOpenType)
                .As(interfaceOpenType);
        }

        public void Singleton(Type implementer)
        {
            Builder.RegisterType(implementer)
                .SingleInstance();
        }

        public void Singleton<TInt>(Type implementer)
        {
            Builder.RegisterType(implementer)
                .AsSelf()
                .As<TInt>()
                .SingleInstance();
        }

        public void Singleton<TInt1, TInt2>(Type implementer)
        {
            Builder.RegisterType(implementer)
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>()
                .SingleInstance();
        }

        public void Singleton<TImp>()
        {
            Builder.RegisterType<TImp>()
                .SingleInstance();
        }

        public void Singleton<TImp, TInt>()
        {
            Builder.RegisterType<TImp>()
                .AsSelf()
                .As<TInt>()
                .SingleInstance();
        }

        public void Singleton<TImp, TInt1, TInt2>()
        {
            Builder.RegisterType<TImp>()
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>()
                .SingleInstance();
        }

        public void SingletonByFactory<T>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .SingleInstance();
        }

        public void SingletonByFactory<T, TInt>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .AsSelf()
                .As<TInt>()
                .SingleInstance();
        }

        public void SingletonByFactory<T, TInt1, TInt2>(Func<IIocContainer, T> factory) where T : class
        {
            Builder.Register(c => factory(new AutofacIocContainer(c)))
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>()
                .SingleInstance();
        }

        public void SingletonGeneric(Type interfaceOpenType, Type implementerOpenType)
        {
            Builder.RegisterGeneric(implementerOpenType)
                .As(interfaceOpenType)
                .SingleInstance();
        }

        public void ByInstance<T>(T instance) where T : class
        {
            Builder.RegisterInstance(instance);
        }

        public void ByInstance<T, TInt>(T instance) where T : class
        {
            Builder.RegisterInstance(instance)
                .AsSelf()
                .As<TInt>();
        }

        public void ByInstance<T, TInt1, TInt2>(T instance) where T : class
        {
            Builder.RegisterInstance(instance)
                .AsSelf()
                .As<TInt1>()
                .As<TInt2>();
        }

        public void RegisterSource(IRegistrationSource registrationSource)
        {
            Builder.RegisterSource(registrationSource);
        }

        public IContainer Build()
        {
            return Builder.Build();
        }
    }
}
