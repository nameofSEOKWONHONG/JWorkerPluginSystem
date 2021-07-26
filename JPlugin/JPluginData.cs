using System;
using McMaster.NETCore.Plugins;

namespace JPlugin
{
    public class JPluginData
    {
        public PluginLoader Loader { get; set; }
        public string DllName { get; set; }

        public PluginError PluginError { get; set; } = new PluginError();

        public JPluginData(PluginLoader loader, string dllName)
        {
            this.Loader = loader;
            this.DllName = dllName;
        }
    }

    public class PluginError
    {
        public bool HasError { get; set; } = false;
        public string ExceptionMessage { get; set; } = null;    
    }
}