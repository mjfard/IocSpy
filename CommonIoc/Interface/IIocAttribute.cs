using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.CommonIoc.Attributes;
using Library.CommonIoc.Enums;

namespace Library.CommonIoc.Interface
{
    public interface  IIocAttribute
    {
        LifeTime Lifetime { get; }
    }
}
