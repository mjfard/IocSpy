using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Library.CommonIoc.Helpers;
using Pendar.IocSpyGen.Managers;
using Pendar.IocSpyGen.Model;
using MethodInfo = Pendar.IocSpyGen.Model.MethodInfo;

namespace Pendar.IocSpyGen
{
    class Extractor
    {
        private readonly List<GenInfo> _allGenInfos;
        private readonly UsingManager _usingManager = new UsingManager();
        private readonly GenericManager _genericManager;

        public Extractor(List<GenInfo> allGenInfos)
        {
            _allGenInfos = allGenInfos;
            _genericManager = new GenericManager(_usingManager);
        }

        public List<SpyGenInfo> Extract() => _allGenInfos.Select(Exctract).ToList();
        private SpyGenInfo Exctract(GenInfo g)
        {
            _usingManager.Init(g);
            var ret = new SpyGenInfo
            {
                SpyGenericName = g.SpyGenericName,
                InterfaceList = g.InterfaceList.Except(g.InterfaceList.SelectMany(i => i.GetInterfaces())).Select(ReflectionHelper.TypeGenericName).ToList(),
                GenericConstraints = _genericManager.GetTypeConstraints(g.Concrete),
                FileName = ExtractorMethods.SpyFileName(g),
                MethodInfos = new List<IMemberInfo>()
            };
            foreach (var @interface in g.InterfaceList)
            {
                var n = @interface.TypeGenericName();
                ret.MethodInfos.AddRange(ProcessInterface(@interface, n));
            }
            ret.Usings = _usingManager.Result;
            return ret;
        }

        private List<IMemberInfo> ProcessInterface(Type @interface, string interfaceGenericName)
        {
            _usingManager.AddUsingOf(@interface);
            var ret = new List<IMemberInfo>();

            var methods = @interface
                .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m => !m.IsSpecialName).ToList();
            foreach (var m in methods)
                ret.Add(ProcessMethod(m, interfaceGenericName));

            foreach (var prop in @interface.GetProperties())
                ret.Add(ProcessProp(prop, interfaceGenericName));

            return ret;
        }

        private PropInfo ProcessProp(PropertyInfo prop, string interfaceGenericName)
        {
            return new PropInfo()
            {
                RetType = _genericManager.TypeGenericName(prop.PropertyType),
                InterfaceGenericName = interfaceGenericName,
                PropName = prop.Name,
                HasSetter = prop.CanWrite,
            };
        }

        private MethodInfo ProcessMethod(System.Reflection.MethodInfo m, string interfaceGenericName)
        {
            return new MethodInfo()
            {
                InterfaceGenericName = interfaceGenericName,
                MethodGenericName = m.MethodGenericName(),
                MethoName = m.Name,
                Outputs = CreateOutputInfos(m),
                Inputs = CreateInputInfos(m),
            };
        }
        private List<InputInfo> CreateInputInfos(System.Reflection.MethodInfo m)
        {
            return m.GetParameters().Where(i => !i.IsOut).Select(delegate(ParameterInfo p)
            {
                var subSpy = SubSpyFor(p.ParameterType);
                return new InputInfo()
                {
                    Type = _genericManager.TypeGenericName(p.ParameterType, m),
                    Name = p.Name,
                    Provision = Provision(subSpy, p.ParameterType, p.Name),
                };
            }).ToList();
        }

        private string Provision(GenInfo subSpy, Type originalType, string inOrOutName)
        {
            return subSpy == null ? inOrOutName : $"SubSpy<{subSpy.SpyName}{_genericManager.TypeGenericArgs(originalType)}>({inOrOutName})";
        }
        private List<OutPutInfo> CreateOutputInfos(System.Reflection.MethodInfo m)
        {
            var ret = new List<OutPutInfo>();
            if (m.ReturnType.Name != "Void")
            {
                var subSpy = SubSpyFor(m.ReturnType);
                ret.Add(new OutPutInfo()
                {
                    Type = _genericManager.TypeGenericName(m.ReturnType, m),
                    Provision = Provision(subSpy, m.ReturnType, "ret"),
                });
            }
            foreach (var p in m.GetParameters().Where(p => p.IsOut))
            {
                var subSpy = SubSpyFor(p.ParameterType);
                ret.Add(new OutPutInfo()
                {
                    Type = _genericManager.TypeGenericName(p.ParameterType, m),
                    Name = p.Name,
                    Provision = Provision(subSpy, p.ParameterType, p.Name),
                });
            }
            return ret;
        }

        public GenInfo SubSpyFor(Type type)
        {
            if (type.IsGenericType)
                type = type.GetGenericTypeDefinition();
            return _allGenInfos.SingleOrDefault(s => s.InterfaceList.All(i => i.IsAssignableFrom(type)));
        }
    }
}
