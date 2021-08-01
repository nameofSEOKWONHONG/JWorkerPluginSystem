using System;
using System.Data;
using JPlugin;
using JWLibrary.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using RepoDb;

namespace PushSenderPlugin
{
    public class PushSenderExecutor : IPlugin
    {
        private readonly ILogger _logger;
        public PushSenderExecutor(ILogger logger)
        {
            this._logger = logger;
        }

        public void SetRequest<TRequest>(TRequest request)
        {

        }

        public bool Validate()
        {
            //data validation
            
            return true;
        }

        public bool PreExecute()
        {
            //modify data
            
            return true;
        }

        public void Execute()
        {
            //main execute
            Console.WriteLine("push sender execute");
        }

        public bool AfterExecute()
        {
            //after main executed
            return false;
        }
    }
}