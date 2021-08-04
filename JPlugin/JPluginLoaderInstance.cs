using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace JPlugin
{
    public class JPluginLoaderInstance
    {
        private static readonly Lazy<JPluginLoaderInstance> jPluginLoader = new(() => new JPluginLoaderInstance());

        private readonly JPluginLoader _pluginLoader;

        private JPluginLoaderInstance()
        {
            _pluginLoader = new JPluginLoader();
        }

        public static JPluginLoaderInstance PluginLoader => jPluginLoader.Value;

        private bool AddLoader(string dllName)
        {
            return _pluginLoader.AddLoader(dllName);
        }

        private bool IsExistsPlugin(string dllName)
        {
            var exists = _pluginLoader.PluginLoaders.FirstOrDefault(m => m.DllName == dllName);
            if (exists != null) return true;

            return false;
        }

        public void Execute<TRequest>(string dllName, TRequest param, ILogger logger)
        {
            var exists = this.IsExistsPlugin(dllName);
            if (!exists)
            {
                this.AddLoader(dllName);
            }
            _pluginLoader.Execute(dllName, param, logger);
        }

        public async Task ExecuteAsync<TRequest>(string dllName, TRequest param, ILogger logger)
        {
            await _pluginLoader.ExecuteAsync(dllName, param, logger);
        }
    }
}