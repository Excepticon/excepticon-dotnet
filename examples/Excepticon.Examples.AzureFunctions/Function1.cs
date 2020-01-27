using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Excepticon.Examples.AzureFunctions
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            using (ExcepticonSdk.Init("CuDC2IVMs+IgTa4+lM6abtQF14IbYj8gy6LEEkQAsyY="))
            {
                try
                {
                    throw new ApplicationException("This error will be sent to Excepticon.");
                }
                catch (Exception ex)
                {
                    ExcepticonSdk.CaptureException(ex);
                }
            }
        }
    }
}
