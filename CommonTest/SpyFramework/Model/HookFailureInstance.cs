using System.Collections.Generic;

namespace Pendar.CommonTest.SpyFramework.Model
{
    class HookFailureInstance
    {
        public List<CheckResult> FailedChecks { set; get; }
        public Hook Hook { set; get; }
        public int CallNo { get; set; }
    }
}
