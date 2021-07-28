using System;
using JPlugin;

namespace KakaoSenderPlugin
{
    public class KakaoSenderExecutor : IPlugin
    {
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