using System;
using System.Collections.Generic;
using System.Linq;
using Pendar.IocLib.Model;
using Library.Common.ObjectExt;

namespace Pendar.IocLib.Managers
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
            _list.Add("Library.CoreTest.Base");
            g.InterfaceList.ForEach(AddUsingOf);
        }

        public List<string> Result => _list.ToList();
    }
}
