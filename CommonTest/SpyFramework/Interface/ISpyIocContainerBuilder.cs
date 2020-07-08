using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.CommonIoc.Interface;
using Pendar.CommonTest.Ioc;

namespace Pendar.CommonTest.SpyFramework.Interface
{
    public interface ISpyIocContainerBuilder : IIocContainer
    {
        void SingleInstanceNoSpy<TInterface>(TInterface instance) where TInterface : class;
        void TransientGeneric(Type spyOpenType, Type intOpenType, ConcreteGenericMaker maker);
        void SinglePerScopeGeneric(Type spyOpenType, Type intOpenType, ConcreteGenericMaker maker);
        void SinglePerScopeByInstance<TSpy, TInterface>(TInterface instance) where TSpy : ISpy, TInterface, new();
        void SinglePerScope<TSpy, TInt1, TInt2>(Type concrete) where TSpy : ISpy, TInt1, TInt2, new();
        void SinglePerScope<TSpy, TInterface>(Type concrete) where TSpy : ISpy, TInterface, new();
        void Singleton<TSpy, TInt1, TInt2>(Type concrete) where TSpy : ISpy, TInt1, TInt2, new();
        void Singleton<TSpy, TInterface>(Type concrete) where TSpy : ISpy, TInterface, new();

        void SinglePerScope<TSpy>(Type concrete, params Type[] interfaces) where TSpy : ISpy, new();
        void SinglePerScopeByInstance<TSpy>(object instance, params Type[] interfaces) where TSpy : ISpy, new();
    }
}
