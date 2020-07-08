using System;

namespace Library.CommonIoc.Attributes
{
    public enum Count { SinglePerRequest, Many, Singleton }

    public class IOCAttribute:Attribute
    {
        public Count Count { get; }

        public IOCAttribute(Count count = Count.Many)
        {
            Count = count;
        }
    }
}
