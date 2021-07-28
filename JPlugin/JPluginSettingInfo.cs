using McMaster.NETCore.Plugins;

namespace JPlugin
{
    public class JPluginSettingInfo
    {
        public JPluginSettingInfo(PluginLoader loader, string dllName)
        {
            Loader = loader;
            DllName = dllName;
        }

        public PluginLoader Loader { get; set; }
        public string DllName { get; set; }
    }
}