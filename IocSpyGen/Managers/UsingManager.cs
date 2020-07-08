using System;
using System.Collections.Generic;
using System.Linq;
using Pendar.IocSpyGen.Model;
using Library.CommonIoc.Helpers;

namespace Pendar.IocSpyGen.Managers
{
    class UsingManager
    {
        private readonly List<string> _list = new List<string>();
        public void AddUsingOf(Type t)
        {
            var ns = t.Namespace;
            if (!_list.Contains(ns))
                _list.Add(ns);
        }
        public void Init(GenInfo g)
        {
            _list.Clear();
            _list.Add("System");
            _list.Add("System.Reflection");
            g.InterfaceList.ForEach(AddUsingOf);
        }

        public List<string> Result => _list.ToList();
    }
}
