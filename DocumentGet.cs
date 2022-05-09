using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TextDocumentAPI
{
    public static class DocumentGet
    {
        [FunctionName("DocumentGet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "documents/get/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: Constants.COSMOS_DB_DATABASE_NAME,
                collectionName: Constants.COSMOS_DB_CONTAINER_NAME,
                ConnectionStringSetting = Constants.COSMOS_DB_CONNECTION_STRING,
                SqlQuery ="SELECT * FROM c WHERE c.id={id}")] Document documentItem,
            ILogger log,
            string id)
        {
            log.LogInformation("text document get triggered.");

            string document = id;
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            document = document ?? data?.name;

            //return not found if no items exists
            if (documentItem == null)
            {
                return new NotFoundResult();
            }

            string responseMessage = documentItem.documentBody;

            return new OkObjectResult(responseMessage);
        }
    }
}
