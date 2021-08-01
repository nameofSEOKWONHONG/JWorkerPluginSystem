using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using McMaster.NETCore.Plugins;
using Microsoft.Extensions.Logging;

namespace JPlugin
{
    internal class JPluginLoader
    {
        public JPluginLoader()
        {
            Init();
        }

        public List<JPluginSettingInfo> PluginLoaders { get; }
            = new();

        private void Init()
        {
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll))
                {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        pluginDll,
                        sharedTypes: new[] {typeof(IPlugin)},
                        isUnloadable: true,
                        configure: config => config.EnableHotReload = true);

                    PluginLoaders.Add(new JPluginSettingInfo(loader, dirName));

                    loader.Reloaded += (s, e) => { Console.WriteLine($"reload : {e.Loader.GetType().Name}"); };
                }
            }
        }

        public bool AddLoader(string dllName)
        {
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            var exists = PluginLoaders.FirstOrDefault(m => m.DllName == dllName);
            if (exists != null) return true;

            foreach (var dir in Directory.GetDirectories(pluginsDir))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName == dllName)
                {
                    var pluginDll = Path.Combine(dir, dirName + ".dll");
                    if (File.Exists(pluginDll))
                    {
                        var loader = PluginLoader.CreateFromAssemblyFile(
                            pluginDll,
                            sharedTypes: new[] {typeof(IPlugin)},
                            isUnloadable: true,
                            configure: config => config.EnableHotReload = true);

                        PluginLoaders.Add(new JPluginSettingInfo(loader, dirName));

                        loader.Reloaded += (s, e) => { Console.WriteLine($"reload : {e.Loader.GetType().Name}"); };

                        return true;
                    }
                }
            }

            return false;
        }

        public void Execute<TRequest>(string dllName, TRequest param, ILogger logger)
        {
            var exists = PluginLoaders.FirstOrDefault(m => m.DllName == dllName);
            if (exists != null)
                try
                {
                    foreach (var pluginType in exists.Loader
                        .LoadDefaultAssembly()
                        .GetTypes()
                        .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                    {
                        // This assumes the implementation of IPlugin has a parameterless constructor
                        var plugin = (IPlugin) Activator.CreateInstance(pluginType, logger);
                        if (plugin != null)
                        {
                            //set request data
                            plugin.SetRequest(param);

                            //validation request data
                            if (plugin.Validate())
                                // pre execute
                                if (plugin.PreExecute())
                                {
                                    // main execute
                                    plugin.Execute();
                                    // after execute (need modify data)
                                    plugin.AfterExecute();
                                }
                        }
                        //Console.WriteLine($"Created plugin instance '{plugin.Run("seokwon")}'.");
                    }
                }
                catch (Exception e)
                {
                    if (logger != null) logger.Log(LogLevel.Error, e.Message, e);
                }

            if (logger != null) logger.Log(LogLevel.Information, $"{dllName} is not found");
        }

        public async Task ExecuteAsync<TRequest>(string dllName, TRequest param, ILogger logger)
        {
            await Task.Run(() => this.Execute(dllName, param, logger));
        }
    }
}