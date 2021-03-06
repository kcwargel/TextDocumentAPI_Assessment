using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;

namespace TextDocumentAPI
{
    public static class DocumentNew
    {
        [FunctionName("DocumentNew")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "documents/new/{documentBody}")] HttpRequest req,
            [CosmosDB(
            databaseName: Constants.COSMOS_DB_DATABASE_NAME,
            collectionName: Constants.COSMOS_DB_CONTAINER_NAME,
            ConnectionStringSetting = Constants.COSMOS_DB_CONNECTION_STRING
                )]IAsyncCollector<dynamic> documentsOut,
            ILogger log,
            string documentBody)
        {
            log.LogInformation("document new function triggered");

            string document = documentBody;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            document = document ?? data?.name;

            //create new document obj to insert
            var newDocument = new Document
            {
                id = System.Guid.NewGuid().ToString(),
                documentBody = documentBody
            };

            //insert doc
            await documentsOut.AddAsync(newDocument);

            var responseObj = new
            {
                status = 200,
                message = "Success, document created",
                id = newDocument.id,
            };

            string responseMessage = JsonConvert.SerializeObject(responseObj);

            return new OkObjectResult(responseMessage);
        }
    }
}
