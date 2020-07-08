using System;
using Library.CommonIoc.Interface;

namespace Library.CommonIoc.Autofac
{
    public class AutofacSingleScopeIocBuilder : AutofacNonScopedIocBuilder, IIocContainerBuilder
    {
        public void SinglePerScope(Type implementer)
        {
            Singleton(implementer);
        }

        public void SinglePerScope<TInt>(Type implementer)
        {
            Singleton<TInt>(implementer);
        }

        public void SinglePerScope<TInt1, TInt2>(Type implementer)
        {
            Singleton<TInt1, TInt2>(implementer);
        }

        public void SinglePerScope<TImp>()
        {
            Singleton<TImp>();
        }

        public void SinglePerScope<TImp, TInt>()
        {
            Singleton<TImp, TInt>();
        }

        public void SinglePerScope<TImp, TInt1, TInt2>()
        {
            Singleton<TImp, TInt1, TInt2>();
        }

        public void SinglePerScopeByFactory<T>(
            Func<IIocContainer, T> factory
        ) where T : class
        {
            SingletonByFactory(factory);
        }

        public void SinglePerScopeByFactory<T, TInt>(
            Func<IIocContainer, T> factory
        ) where T : class
        {
            SingletonByFactory<T, TInt>(factory);
        }

        public void SinglePerScopeByFactory<T, TInt1, TInt2>(
            Func<IIocContainer, T> factory
        ) where T : class
        {
            SingletonByFactory<T, TInt1, TInt2>(factory);
        }

        public void SinglePerScopeGeneric(
            Type interfaceOpenType,
            Type implementerOpenType
        )
        {
            SingletonGeneric(interfaceOpenType, implementerOpenType);
        }
    }
}
