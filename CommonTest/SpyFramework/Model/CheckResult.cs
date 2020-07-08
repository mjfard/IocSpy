namespace Pendar.CommonTest.SpyFramework.Model
{
    public class CheckResult
    {
        public bool Result { set; get; }
        public string Title { set; get; }
        public object Hint { set; get; }
        public int No { set; get; }
        public CheckResult(bool result, string title, object hint)
        {
            Result = result;
            Title = title;
            Hint = hint;
        }

        public CheckResult()
        {
            
        }
    }
}