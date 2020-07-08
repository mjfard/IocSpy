namespace Pendar.CommonTest.SpyFramework.Interface
{
    public interface ISpy
    {
        void _Init(object orig, SpyMaster master);
        void _CompleteInfo(ICallInfo info, params object[] outputs);
    }
}
