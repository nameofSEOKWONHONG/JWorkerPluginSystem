using System;
using System.Collections;
using System.Collections.Generic;
using JPlugin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JWorkerPluginSystem.Controller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WorkerStateController : ControllerBase
    {
        private ILogger _logger;
        public WorkerStateController(ILogger<WorkerStateController> logger)
        {
            this._logger = logger;
        }

        // [HttpGet]
        // public PluginError GetErrorLoader(string dllName)
        // {
        //     return JPluginLoaderInstance.PluginLoader.HasError(dllName);
        // }

        // [HttpGet]
        // public IEnumerable<PluginError> GetErrorLoaders()
        // {
        //     return JPluginLoaderInstance.PluginLoader.HasErrors();
        // }

        // [HttpPost]
        // public bool SetAddLoader(string dllName)
        // {
        //     JPluginLoaderInstance.PluginLoader.AddLoader(dllName);
        //     return true;
        // }
    }
}