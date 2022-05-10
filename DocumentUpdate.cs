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
    public static class DocumentUpdate
    {
        [FunctionName("DocumentUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "documents/update/{id}/{text}")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.COSMOS_DB_DATABASE_NAME,
                collectionName: Constants.COSMOS_DB_CONTAINER_NAME,
                ConnectionStringSetting = Constants.COSMOS_DB_CONNECTION_STRING)] 
            ILogger log,
            string id,
            string text)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            //TODO
            //update CosmosDB

            var responseObj = new
            {
                status = 200,
                message = "Success, document updated",
                id = id,
                text = text
            };

            string responseMessage = JsonConvert.SerializeObject(responseObj);

            return new OkObjectResult(responseMessage);
        }
    }
}
