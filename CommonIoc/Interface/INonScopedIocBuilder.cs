using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.CommonIoc.Interface
{
    public interface INonScopedIocBuilder
    {
        void Transient(Type implementer);
        void Transient<TInt>(Type implementer);
        void Transient<TInt1, TInt2>(Type implementer);

        void Transient<TImp>();
        void Transient<TImp, TInt>();
        void Transient<TImp, TInt1, TInt2>();

        void TransientByFactory<T>(Func<IIocContainer, T> factory) where T : class;
        void TransientByFactory<T, TInt>(Func<IIocContainer, T> factory) where T : class;
        void TransientByFactory<T, TInt1, TInt2>(Func<IIocContainer, T> factory) where T : class;

        void TransientGeneric(Type interfaceOpenType, Type implementerOpenType);

        void Singleton(Type implementer);
        void Singleton<TInt>(Type implementer);
        void Singleton<TInt1, TInt2>(Type implementer);

        void Singleton<TImp>();
        void Singleton<TImp, TInt>();
        void Singleton<TImp, TInt1, TInt2>();

        void SingletonByFactory<T>(Func<IIocContainer, T> factory) where T : class;
        void SingletonByFactory<T, TInt>(Func<IIocContainer, T> factory) where T : class;
        void SingletonByFactory<T, TInt1, TInt2>(Func<IIocContainer, T> factory) where T : class;

        void SingletonGeneric(Type interfaceOpenType, Type implementerOpenType);

        void ByInstance<T>(T instance) where T : class;
        void ByInstance<T, TInt>(T instance) where T : class;
        void ByInstance<T, TInt1, TInt2>(T instance) where T : class;
    }
}
