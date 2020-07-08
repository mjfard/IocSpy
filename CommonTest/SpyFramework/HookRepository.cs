using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pendar.CommonTest.SpyFramework.Model;

namespace Pendar.CommonTest.SpyFramework
{
    public class HookRepository
    {
        readonly Dictionary<Type, Dictionary<string, List<Hook>>> _hooks = new Dictionary<Type, Dictionary<string, List<Hook>>>();

        public void Add(Hook ch)
        {
            var membertype = ch.MemberType == MemberType.Method ? MemberTypes.Method : MemberTypes.Property;
            var interfaces = ch.InterfaceType
                .GetInterfaces()
                .Union(new[] {ch.InterfaceType})
                .Where(i => i.GetMember(ch.MethodOrProp).Any(m => m.MemberType == membertype));
            foreach (var intf in interfaces)
            {
                AddToDictionary(intf, ch);
            }
        }

        private void AddToDictionary(Type @interface,Hook ch)
        {
            if (!_hooks.ContainsKey(@interface))
                _hooks.Add(@interface, new Dictionary<string, List<Hook>>());
            var interfaceCheckers = _hooks[@interface];
            if (!interfaceCheckers.ContainsKey(ch.MethodOrProp))
                interfaceCheckers.Add(ch.MethodOrProp, new List<Hook>());
            interfaceCheckers[ch.MethodOrProp].Add(ch);
        }

        public IEnumerable<Hook> GetAllHooks()
        {
            return _hooks.Values.SelectMany(dic => dic.Values).SelectMany(l => l);
        }
        public IEnumerable<Hook> GetHooks(Type interfaceType, string method)
        {
            if (interfaceType.IsGenericType)
            {
                var genericTypeDef = interfaceType.GetGenericTypeDefinition();

                return GetCheckersOfDictionaryKey(interfaceType, method)
                    .Union(GetCheckersOfDictionaryKey(genericTypeDef, method));
            }

            return GetCheckersOfDictionaryKey(interfaceType, method);
        }

        private IEnumerable<Hook> GetCheckersOfDictionaryKey(Type interfaceType, string method)
        {
            if (_hooks.ContainsKey(interfaceType))
            {
                var interfaceCheckers = _hooks[interfaceType];
                if (interfaceCheckers.ContainsKey(method))
                    return interfaceCheckers[method];
            }
            return Enumerable.Empty<Hook>();
        }
    }
}