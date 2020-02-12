using System;

namespace Excepticon.Examples.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ExcepticonSdk.Init("{Your ApiKey Here"))
            {
                throw new ApplicationException("This error will be sent to Excepticon.");
            }
        }
    }
}
