using System;

namespace Library.CommonIoc.Attributes
{
    public class SpiedAttribute : Attribute
    {
        public string SpyName { get; }

        public SpiedAttribute(string spyName = null)
        {
            SpyName = spyName;
        }
    }
}
