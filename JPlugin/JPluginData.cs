using McMaster.NETCore.Plugins;

namespace JPlugin
{
    public class JPluginData
    {
        public PluginLoader Loader { get; set; }
        public string DllName { get; set; }

        public JPluginData(PluginLoader loader, string dllName)
        {
            this.Loader = loader;
            this.DllName = dllName;
        }
    }
}