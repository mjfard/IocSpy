using System;
using Autofac;
using Library.CommonIoc.Interface;

namespace Library.CommonIoc.Autofac
{
    public class AutofacIocContainer : IIocContainer
    {
        private readonly IComponentContext _context;

        public AutofacIocContainer(IComponentContext context)
        {
            _context = context.Resolve<IComponentContext>();
        }

        public T Resolve<T>()
        {
            return _context.Resolve<T>();
        }

        public object Resolve(Type t)
        {
            return _context.Resolve(t);
        }
    }
}
