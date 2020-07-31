using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ReadChangeFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
        public async static void Run(IReadOnlyList<Document> input, ILogger log)
        {
            string EndpointUrl = "https://sidcosmosaccount.documents.azure.com:443/";
            string PrimaryKey = "VgAp0Spm3hWGSNMq4HE5RNU0L8FltboD0xwvhiqPyDUFpbWJ0j0mMBeSQFtViQgv5A3R5NfKnIjR2UPeXyfI9Q==";
            string dbName = "airlinedb";
            string collection = "lastNameContainer";
            DocumentClient client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            if (input != null && input.Count > 0)
            {
                //log.LogInformation("Documents modified " + input.Count);
                //log.LogInformation("First document Id " + input[0].lastName);
                foreach (var doc in input)
                {
                    Document newDoc = new Document();
                    newDoc.SetPropertyValue("id", doc.GetPropertyValue<string>("id"));
                    newDoc.SetPropertyValue("lastName", doc.GetPropertyValue<string>("lastName"));
                    log.LogInformation(doc.GetPropertyValue<string>("id"));
                    
                    await client
                        .CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(dbName, collection)
                            , newDoc
                        );
                }
            }
        }

    }
}


