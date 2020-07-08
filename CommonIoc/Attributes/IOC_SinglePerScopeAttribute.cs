using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.CommonIoc.Enums;
using Library.CommonIoc.Interface;

namespace Library.CommonIoc.Attributes
{
    public class IOC_SinglePerScopeAttribute : Attribute, IIocAttribute
    {
        public LifeTime Lifetime => LifeTime.SinglePerScope;
    }
}
