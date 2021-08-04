using CSScriptPlugin;
using JPlugin;
using NUnit.Framework;

namespace JWorkerPluginSystemTest
{
    public class CSScriptPluginTest
    {
        [Test]
        public void Test1()
        {
            IPlugin plugin = new CSScriptExecutor();
            plugin.Execute();
        }
    }
}