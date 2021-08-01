using System;
using JPlugin;
using Microsoft.Extensions.Logging;

namespace KakaoSenderPlugin
{
    public class KakaoSenderExecutor : IPlugin
    {
        private readonly ILogger _logger;
        public KakaoSenderExecutor(ILogger logger)
        {
            this._logger = logger;
        }
        public void SetRequest<TRequest>(TRequest request)
        {
        }

        public bool Validate()
        {
            return true;
        }

        public bool PreExecute()
        {
            return true;
        }

        public void Execute()
        {
            Console.WriteLine("kakao sender execute");
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}