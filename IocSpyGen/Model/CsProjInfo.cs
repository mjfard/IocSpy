using System.Reflection;

namespace Pendar.IocSpyGen.Model
{
    public class CsProjInfo
    {
        public Assembly Asembly { get; }
        public string Path { get; }
        public string DefaultNs { get; }

        public CsProjInfo(Assembly asembly, string path, string defaultNs)
        {
            Asembly = asembly;
            Path = path;
            DefaultNs = defaultNs;
        }
    }
}
