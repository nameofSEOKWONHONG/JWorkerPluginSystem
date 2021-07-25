using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using McMaster.NETCore.Plugins;

namespace JPlugin
{
    public class JPluginLoaderInstance
    {
        private static Lazy<JPluginLoaderInstance> jPluginLoader = new Lazy<JPluginLoaderInstance>(() => new JPluginLoaderInstance());

        public static JPluginLoaderInstance PluginLoader
        {
            get
            {
                return jPluginLoader.Value;
            }
        }

        private readonly JPluginLoader _pluginLoader;

        public JPluginLoaderInstance()
        {
            this._pluginLoader = new JPluginLoader();
        }

        public bool AddLoader(string dllName)
        {
            return this._pluginLoader.AddLoader(dllName);
        }

        public object Execute<TRequest>(string dllName, TRequest param)
        {
            return this._pluginLoader.Execute<TRequest>(dllName, param);
        }

    }
    
    internal class JPluginLoader
    {
        public List<JPluginData> PluginLoaders { get; private set; }
            = new List<JPluginData>();

        public JPluginLoader()
        {
            Init();
        }

        private void Init()
        {
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            foreach (var dir in Directory.GetDirectories(pluginsDir)) {
                var dirName = Path.GetFileName(dir);
                var pluginDll = Path.Combine(dir, dirName + ".dll");
                if (File.Exists(pluginDll)) {
                    var loader = PluginLoader.CreateFromAssemblyFile(
                        assemblyFile:pluginDll,
                        sharedTypes:new[] {typeof(IJPlugin)},
                        isUnloadable:true,
                        configure:config => config.EnableHotReload = true);
                    
                    PluginLoaders.Add(new JPluginData(loader, dirName));

                    loader.Reloaded += (s, e) =>
                    {
                        Console.WriteLine($"reload : {e.Loader.GetType().Name}");
                    };
                }
            }
        }

        public bool AddLoader(string dllName)
        {
            var pluginsDir = Path.Combine(AppContext.BaseDirectory, "plugins");
            var exists = this.PluginLoaders.FirstOrDefault(m => m.DllName == dllName);
            if (exists != null) return true;
            
            foreach (var dir in Directory.GetDirectories(pluginsDir)) {
                var dirName = Path.GetFileName(dir);
                if (dirName == dllName)
                {
                    var pluginDll = Path.Combine(dir, dirName + ".dll");
                    if (File.Exists(pluginDll)) {
                        var loader = PluginLoader.CreateFromAssemblyFile(
                            assemblyFile:pluginDll,
                            sharedTypes:new[] {typeof(IJPlugin)},
                            isUnloadable:true,
                            configure:config => config.EnableHotReload = true);
                    
                        PluginLoaders.Add(new JPluginData(loader, dirName));

                        loader.Reloaded += (s, e) =>
                        {
                            Console.WriteLine($"reload : {e.Loader.GetType().Name}");
                        };

                        return true;
                    }
                }
            }

            return false;
        }

        public object Execute<TRequest>(string dllName, TRequest param)
        {
            var exists = this.PluginLoaders.FirstOrDefault(m => m.DllName == (dllName));
            if (exists != null)
            {
                foreach (var pluginType in exists.Loader
                    .LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IJPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    // This assumes the implementation of IPlugin has a parameterless constructor
                    var plugin = (IJPlugin) Activator.CreateInstance(pluginType);
                    if (plugin != null)
                    {
                        //set request data
                        plugin.SetRequest<TRequest>(param);
                        
                        //validation request data
                        if (plugin.Validate())
                        {
                            // pre execute
                            if (plugin.PreExecute())
                            {
                                // main execute
                                var result = plugin.Execute();
                                
                                // after execute (need modify data)
                                plugin.AfterExecute();
                                
                                // return result;
                                return result;
                            }
                        }
                    }
                    //Console.WriteLine($"Created plugin instance '{plugin.Run("seokwon")}'.");
                }
            }

            return null;
        }
    }
}