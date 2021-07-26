using System;
using eXtensionSharp;
using JPlugin;

namespace MailSender
{
    public class MailSenderExecutor : IJPlugin
    {
        private string _request = string.Empty;
        public void SetRequest<TRequest>(TRequest request)
        {
            _request = request as string;
        }
        
        public bool Validate()
        {
            if (this._request.xIsNull())
            {
                return false;
            }
            return true;
        }
        
        public bool PreExecute()
        {
            if(this._request.xIsNotNull()) Console.WriteLine(this._request);
            Console.WriteLine("Mail Sender : PreExecute");
            return true;
        }

        public object Execute()
        {
            Console.WriteLine("Mail Sender : Execute");
            throw new Exception("Test Mail Sender Exception");
            return null;
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}