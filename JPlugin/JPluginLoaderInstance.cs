using System;
using System.Collections;
using System.Collections.Generic;

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

        public PluginError HasError(string dllName)
        {
            return this._pluginLoader.HasError(dllName);
        }

        public IEnumerable<PluginError> HasErrors()
        {
            return this._pluginLoader.HasErrors();
        }
    }
}