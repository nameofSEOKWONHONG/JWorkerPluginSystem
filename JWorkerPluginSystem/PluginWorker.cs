using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using eXtensionSharp;
using JPlugin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JWorkerPluginSystem
{
    public class PluginWorker : BackgroundService
    {
        private readonly ILogger<PluginWorker> _logger;
        private readonly IEnumerable<PluginSetting> _pluginSettings;

        public PluginWorker(ILogger<PluginWorker> logger)
        {
            _logger = logger;
            _pluginSettings = "pluginsettings.json".xFileReadAllText().xToEntities<PluginSetting>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _pluginSettings.xPararellForEach(setting =>
                {
                    if (JPluginLoaderInstance.PluginLoader.Exists(setting.DllName))
                    {
                        JPluginLoaderInstance.PluginLoader.Execute(setting.DllName, false, _logger);
                    }
                    else
                    {
                        JPluginLoaderInstance.PluginLoader.AddLoader(setting.DllName);
                        JPluginLoaderInstance.PluginLoader.Execute(setting.DllName, false, _logger);
                    }
                });
                _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    internal class PluginSetting
    {
        public string DllName { get; set; }
    }
}