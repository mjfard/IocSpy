namespace Library.CommonIoc.Helpers
{
    public static class StringHelper
    {
        public static string HeadUntilFirstIndexOrAll(this string src, string seperator)
        {
            var length = src.IndexOf(seperator);
            return length < 0 ? src : src.Substring(0, length);
        }
    }
}
