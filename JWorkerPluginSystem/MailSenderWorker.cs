using System;
using System.Threading;
using System.Threading.Tasks;
using JPlugin;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JWorkerPluginSystem
{
    public class MailSenderWorker : BackgroundService
    {
        private readonly ILogger<MailSenderWorker> _logger;
        public MailSenderWorker(ILogger<MailSenderWorker> logger)
        {
            this._logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                JPluginLoaderInstance.PluginLoader.Execute<string>("MailSender", "seokwon");
                _logger.LogInformation("Mail Sender running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}