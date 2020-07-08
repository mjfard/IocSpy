using System;

namespace Library.CommonIoc.Interface
{
    public interface IIocContainer
    {
        T Resolve<T>();
        object Resolve(Type t);
    }
}
