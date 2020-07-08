using System;
using System.Collections.Generic;
using System.Linq;
using Pendar.CommonTest.SpyFramework.Interface;

namespace Pendar.CommonTest
{
    class DeepCloneManager
    {
        private readonly Dictionary<Type, CloneTarget> _cloneTargets = new Dictionary<Type, CloneTarget>();

        public void AddCloneTarget(Type spyType, string[] mehtods)
        {
            if(spyType.GetInterfaces().All(i => i != typeof(ISpy)))
                throw new Exception("SpyType should be provided as DeepClone target");
            if (_cloneTargets.ContainsKey(spyType))
            {
                var cur = _cloneTargets[spyType];
                cur.Mehtods = cur.Mehtods.Union(mehtods).Distinct().ToList();
            }
            _cloneTargets.Add(spyType, new CloneTarget() { Mehtods = mehtods.ToList(), SpyType = spyType });
        }


    }
}
