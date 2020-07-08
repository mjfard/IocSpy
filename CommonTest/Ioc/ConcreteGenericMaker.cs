using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;

namespace Library.CoreTest.Base
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
