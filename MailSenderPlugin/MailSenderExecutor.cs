﻿using System;
using eXtensionSharp;
using JPlugin;

namespace MailSenderPlugin
{
    public class MailSenderExecutor : IPlugin
    {
        private string _request = string.Empty;

        public void SetRequest<TRequest>(TRequest request)
        {
            _request = request as string;
        }

        public bool Validate()
        {
            if (_request.xIsNull()) return false;
            return true;
        }

        public bool PreExecute()
        {
            if (_request.xIsNotNull()) Console.WriteLine(_request);
            Console.WriteLine("Mail Sender : PreExecute");
            return true;
        }

        public void Execute()
        {
            Console.WriteLine("Mail Sender : Execute");
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}