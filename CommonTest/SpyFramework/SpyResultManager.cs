using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pendar.CommonTest.SpyFramework.Interface;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework
{
    class SpyResultManager
    {
        private readonly HookRepository _hookRepository;
        private readonly List<CheckResult> _tempCheckResults = new List<CheckResult>();
        private readonly List<HookFailureInstance> _failures = new List<HookFailureInstance>();

        private readonly List<FinalChecker> _finalCheckers = new List<FinalChecker>();
        private readonly List<CheckResult> _finalFailures = new List<CheckResult>();

        public SpyResultManager(HookRepository hookRepository)
        {
            _hookRepository = hookRepository;
        }

        public void AddCheckResult(bool checkResult, string title, object hint)
        {
            _tempCheckResults.Add(new CheckResult(checkResult, title, hint));
        }

        public void AssertSpyResult()
        {
            for (var i = 0; i < _finalCheckers.Count; i++)
            {
                var fch = _finalCheckers[i];
                if (fch.Checker() == false)
                {
                    _finalFailures.Add(new CheckResult()
                    {
                        No = i,
                        Title = fch.Title,
                        Hint = fch.HintProvider?.Invoke(),
                    });
                }
            }

            Assert.IsTrue(_failures.Count == 0 &&
                          _finalFailures.Count == 0 &&
                          _hookRepository.GetAllHooks().All(h => !h.HasCallCountError()), PrepareSpyResult());
        }

        public string PrepareSpyResult()
        {
            var ret = "";
            foreach (var failure in _failures.OrderBy(f => f.Hook.HookType).ThenBy(f => f.Hook.MethodOrProp))
            {
                ret += failure.Hook.ToString();
                ret += $" (Call {failure.CallNo} of {failure.Hook.ActualCount})";
                ret += "\r\n";
                ret += TextOfCheckFailures(failure.FailedChecks, "Check");
                ret += "\r\n";
            }


            foreach (var hook in _hookRepository.GetAllHooks())
            {
                if (hook.HasCallCountError())
                    ret += $"{hook}\r\n\tCall Count => actual: {hook.ActualCount}, expected: {hook.CallCount?.ToString()??"At leest One"}\r\n";
            }

            ret += "\r\n";

            if (_finalFailures.Any())
            {
                ret += "FINAL CHECK FAILURES:\r\n";
                ret += TextOfCheckFailures(_finalFailures, "FinalCheck");
            }

            return ret;
        }


        private static string TextOfCheckFailures(IEnumerable<CheckResult> failedChecks, string checkType)
        {
            var ret = "";
            foreach (var result in failedChecks)
            {
                var title = result.Title != null ? $"\"{result.Title}\"" : "";
                var hint = result.Hint != null ? $": {result.Hint}" : "";
                ret += $"\t{checkType}[{result.No}] {title}{hint}\r\n";
            }

            return ret;
        }

        public void ParamHook(Hook hook, object[] parameters, ICallInfo callInfo)
        {
            if(hook.Cond?.Invoke(callInfo) == false)
                return;
            _tempCheckResults.Clear();
            hook.ParamDelegate?.Invoke(parameters[hook.ParamNo]);
            GatherCallResults(hook);
        }

        public void CallHook(Hook hook, ICallInfo callInfo)
        {
            if (hook.Cond?.Invoke(callInfo) == false)
                return;
            _tempCheckResults.Clear();
            hook.CallDelegate?.Invoke(callInfo);
            GatherCallResults(hook);
        }


        private void GatherCallResults(Hook hook)
        {
            hook.ActualCount++;
            if (_tempCheckResults.Any(r => r.Result == false))
            {
                _failures.Add(new HookFailureInstance()
                {
                    Hook = hook,
                    CallNo = hook.ActualCount,
                    FailedChecks = _tempCheckResults.Select((r, i) =>
                    {
                        r.No = i;
                        return r;
                    }).Where(r => r.Result == false).ToList(),
                });
            }
        }

        public void AddFinalChecker(FinalChecker finalChecker)
        {
            _finalCheckers.Add(finalChecker);
        }
    }
}