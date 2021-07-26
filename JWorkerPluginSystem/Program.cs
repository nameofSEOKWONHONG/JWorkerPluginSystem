using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eXtensionSharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JWorkerPluginSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (XOS.xIsLinux())
            {
                CreateHostBuilderLinux(args).Build().Run();    
            }
            
            if (XOS.xIsWindows())
            {
                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureWebHost(config =>
                {
                    config.UseUrls("http://*:5050");
                })
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHostedService<MailSenderWorker>();
                });
        
        public static IHostBuilder CreateHostBuilderLinux(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureWebHost(config =>
                {
                    config.UseUrls("http://*:5050");
                })
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddHostedService<MailSenderWorker>();
                });        
    }
}