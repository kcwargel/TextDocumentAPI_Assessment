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
    public static class DocumentDelete
    {
        [FunctionName("DocumentDelete")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "documents/delete/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.COSMOS_DB_DATABASE_NAME,
                collectionName: Constants.COSMOS_DB_CONTAINER_NAME,
                ConnectionStringSetting = Constants.COSMOS_DB_CONNECTION_STRING)] 
            ILogger log,
            string id)
        {
            log.LogInformation("delete function triggered");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            //TODO
            //delete document from db at id

            string responseMessage = "deleted";

            return new OkObjectResult(responseMessage);
        }
    }
}
