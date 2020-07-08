using System;

namespace Library.CommonIoc.Interface
{
    public interface IConfigableIocContainer : IIocContainer
    {
        void ManyGeneric(Type intOpenType, Type concreteOpenType);
        void SinglePerRequestGeneric(Type intOpenType, Type concreteOpenType);
        void SinglePerRequestByInstance<TInterface>(TInterface instance) where TInterface : class;
        void SinglePerRequest<TInterface>(Type concreteType);
        void SinglePerRequest<TInt1, TInt2>(Type concreteType);
        void Build();
        void Dispose();
        void Singleton<TInt>(Type concreteType);
        void SinglePerRequestByFactory<TInterface>(Func<TInterface> factory);
    }
}
