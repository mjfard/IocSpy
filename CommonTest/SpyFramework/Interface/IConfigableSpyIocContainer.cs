using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.CommonIoc.Interface;
using Library.CoreTest.Base;

namespace Pendar.CommonTest.SpyFramework.Interface
{
    public interface IConfigableSpyIocContainer : IIocContainer
    {
        void SingleInstanceNoSpy<TInterface>(TInterface instance) where TInterface : class;
        void ManyGeneric(Type spyOpenType, Type intOpenType, ConcreteGenericMaker maker);
        void SinglePerRequestGeneric(Type spyOpenType, Type intOpenType, ConcreteGenericMaker maker);
        void SinglePerRequestByInstance<TSpy, TInterface>(TInterface instance) where TSpy : ISpy, TInterface, new();
        void SinglePerRequest<TSpy, TInt1, TInt2>(Type concrete) where TSpy : ISpy, TInt1, TInt2, new();
        void SinglePerRequest<TSpy, TInterface>(Type concrete) where TSpy : ISpy, TInterface, new();
    }
}
