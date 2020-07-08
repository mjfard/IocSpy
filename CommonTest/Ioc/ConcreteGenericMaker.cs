using System;
using Autofac;
using Autofac.Core;

namespace Pendar.CommonTest.Ioc
{
    public class ConcreteGenericMaker
    {
        public Type ConcreteOpenType { get; }

        public ConcreteGenericMaker(Type concreteOpenType)
        {
            ConcreteOpenType = concreteOpenType;
        }
        public object MakeInstance(IActivatedEventArgs<object> args)
        {
            var concreteType = ConcreteOpenType.MakeGenericType(args.Instance.GetType().GetGenericArguments());
            return args.Context.Resolve(concreteType);
        }
    }
}
