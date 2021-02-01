using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HttpTriggerStudy20201003
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static void Run(
            [BlobTrigger("blob-trigger/{name}")] Stream image,
            string name,
            [Queue("outqueue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg,
            ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request. name is {name}.");

            // Add a message to the output collection.
            msg.Add($"Name passed to the function: {name}");
        }
    }
}
