﻿using System;
using JPlugin;

namespace SmsSenderPlugin
{
    public class SmsSenderExecutor : IPlugin
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
            Console.WriteLine("sms sender execute");
            return null;
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}