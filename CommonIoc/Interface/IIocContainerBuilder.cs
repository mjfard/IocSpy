using System;

namespace Library.CommonIoc.Interface
{
    public interface IIocContainerBuilder : INonScopedIocBuilder
    {
        void SinglePerScope(Type implementer);
        void SinglePerScope<TInt>(Type implementer);
        void SinglePerScope<TInt1, TInt2>(Type implementer);

        void SinglePerScope<TImp>();
        void SinglePerScope<TImp, TInt>();
        void SinglePerScope<TImp, TInt1, TInt2>();

        void SinglePerScopeByFactory<T>(Func<IIocContainer, T> factory, params Type[] interfaces) where T : class;

        void SinglePerScopeByFactory<T, TInt>(Func<IIocContainer, T> factory) where T : class;

        void SinglePerScopeByFactory<T, TInt1, TInt2>(Func<IIocContainer, T> factory) where T : class;

        void SinglePerScopeGeneric(Type interfaceOpenType, Type implementerOpenType);
    }
}
