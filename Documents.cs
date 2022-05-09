using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TextDocumentAPI
{
    public static class Documents
    {
        [FunctionName("Documents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "my-database", collectionName: "my-container",
                ConnectionStringSetting = "CosmosDbConnectionString"
                )]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //query param list
            string action = req.Query["action"];
            string documentID = req.Query["documentID"];
            string document = req.Query["document"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            documentID = documentID ?? data?.name;
            document = document ?? data?.name;
            action = action ?? data?.name;

            string responseMessage = string.Empty;
            if (string.IsNullOrEmpty(action))
            {
                responseMessage = "no action";
                return new OkObjectResult(responseMessage);
            }

            switch (action)
            {
                case ("create"):
                    if (!string.IsNullOrEmpty(document))
                    {
                        responseMessage = $"Success: {document}.";
                    }
                    break;

                case ("get"):
                    if (!string.IsNullOrEmpty(documentID))
                    {
                        responseMessage = $"Success: {documentID}.";
                    }
                    break;

                case ("update"):
                    if (!string.IsNullOrEmpty(documentID) && !string.IsNullOrEmpty(document))
                    {
                        responseMessage = $"Updated: {documentID} and {document}.";
                    }
                    break;

                case ("delete"):
                    if (!string.IsNullOrEmpty(documentID))
                    {
                        responseMessage = $"Deleted: {documentID}.";
                    }
                    break;

                default:
                    responseMessage = "please use query param : 'action' with 'document' and/or 'documentID'";
                    break;
            }


            //if (!string.IsNullOrEmpty(document))
            //{
            //    // Add a JSON document to the output container.
            //    await documentsOut.AddAsync(new
            //    {
            //        // create a random ID
            //        id = System.Guid.NewGuid().ToString(),
            //        documentBody = document
            //    });
            //}



            return new OkObjectResult(responseMessage);
        }
    }
}
  