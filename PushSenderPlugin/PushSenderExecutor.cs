using System;
using JPlugin;

namespace PushSenderPlugin
{
    public class PushSenderExecutor : IPlugin
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
            Console.WriteLine("push sender execute");
        }

        public bool AfterExecute()
        {
            return false;
        }
    }
}