using System;

namespace Pendar.CommonTest.SpyFramework.Model
{
    public class FinalChecker
    {
        public Func<bool> Checker { set; get; }
        public string Title { set; get; }
        public Func<string> HintProvider { set; get; }
    }
}
