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

        public object Execute()
        {
            Console.WriteLine("kakao sender execute");
            return null;
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}