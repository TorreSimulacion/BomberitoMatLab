using System.Diagnostics;
using System.Threading;

namespace OT.Ternium.Unity.MathModels
{
    public class DebugLog : ICustomLog
    {
        public void WriteLine(string message)
        {
            Debug.WriteLine(string.Format("ThreadId: {0}, message: {1}", Thread.CurrentThread.ManagedThreadId, message), "[FireController]");
        }
    }
}
