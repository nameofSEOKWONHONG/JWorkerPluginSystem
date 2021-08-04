using System;
using System.IO;
using System.Linq;
using CSScriptLib;
using JPlugin;
using Microsoft.CodeAnalysis.Scripting;

namespace CSScriptPlugin
{
    public class CSScriptExecutor : IPlugin
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
            string code = File.ReadAllText("./HelloWorld.cs");
            dynamic exec = CSScript.Evaluator.ReferenceAssembliesFromCode(code).LoadCode(code);
            exec.Execute();            
        }

        public bool AfterExecute()
        {
            return true;
        }
    }
}