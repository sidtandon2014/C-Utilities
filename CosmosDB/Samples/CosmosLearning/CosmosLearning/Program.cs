using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CosmosLearning
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Starting the demo");
            Learning cosmosLearning = new Learning();
            cosmosLearning.initializeCosmosAccount().Wait();
            //cosmosLearning.checkFirstRule().Wait();
            Console.ReadKey();
        }
    }

    class Learning
    {
        private string storedProcedure = "checkFirstTransaction";
        private string dbName = "tsDb";
        private string collection = "tsContainer";
        private DocumentClient client;
        private const string EndpointUrl = "https://timeseriesdata.documents.azure.com:443/";
        private const string PrimaryKey = "NqyJArbt3qqtNhjNSxgOnNktsoQD5gIeqX3ftUwzUFeeLWu0vO2hqwIHqehSXr96lh9aLqzpAhdXAI3xqhPFwQ==";
        public Learning()
        {
            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
        }

        public async Task initializeCosmosAccount()
        {
            await this.createDatabaseIfNotExists();
            await this.createCollectionIFNotExists();
            //await this.CreateTransactionDocumentIfNotExists(100);
            await this.CreateTimeSeriesDocumentIfNotExists(100);
        }
        private async Task createDatabaseIfNotExists()
        {
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = dbName });
            Console.WriteLine("Created database");
        }

        private async Task createCollectionIFNotExists()
        {
            await this.client.
                CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(dbName)
                , new DocumentCollection { Id = collection }
                );

            Console.WriteLine("Created collection");
        }

        private async Task CreateTransactionDocumentIfNotExists(int totalDocs)
        {
            int index = 0;
            while (index < totalDocs)
            {
                try
                {
                    Transactions transaction = getNewTransactionDocument(index);
                    await this.client
                        .CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(dbName, collection)
                            , transaction
                        );

                    Console.WriteLine("Created Document: " + index.ToString());
                    //await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, transaction.Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Index: " + index.ToString() + " Error Occured: " + ex.Message );
                }
                finally
                {
                    index++;
                }
                Thread.Sleep(10000);
            }

        }

        private async Task CreateTimeSeriesDocumentIfNotExists(int totalDocs)
        {
            int index = 0;
            while (index < totalDocs)
            {
                try
                {
                    TimeSeries ts = getNewTimeSeriesDocument(index);
                    await this.client
                        .CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(dbName, collection)
                            , ts
                        );

                    Console.WriteLine("Created Document: " + index.ToString());
                    //await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, transaction.Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Index: " + index.ToString() + " Error Occured: " + ex.Message);
                }
                finally
                {
                    index++;
                }
//                Thread.Sleep(10000);
            }

        }
        private Transactions getNewTransactionDocument(int id)
        {
            Transactions tran = new Transactions()
            {
                Id = id.ToString(),
                TransactionID = "TxN" + id.ToString(),
                Amount = new Random().NextDouble() * 100000,
                Channel = MasterData.Channels[new Random().Next(0, MasterData.Channels.Length)],
                CustomerStatus = MasterData.CustomerStatus[new Random().Next(0, MasterData.CustomerStatus.Length)],
                TransactionTimestamp = DateTime.Now,
                TransactionType = MasterData.TransactionType[new Random().Next(0, MasterData.TransactionType.Length)],
                FromAccountId = MasterData.FromAccountId[new Random().Next(0, MasterData.FromAccountId.Length)],
                ToAccountId = MasterData.ToAccountId[new Random().Next(0, MasterData.ToAccountId.Length)]
            };
            return tran;
        }

        private TimeSeries getNewTimeSeriesDocument(int id)
        {
            TimeSeries ts = new TimeSeries()
            {
                Id = id.ToString(),
                location = "loc" + id.ToString(),
                unit = "unit" + new Random().Next(0, 5),
                equipment = "equip" + new Random().Next(0, 10),
                tagid = "tag" + new Random().Next(0, 100).ToString() + "_" +DateTime.Now.Year.ToString() ,
                year = DateTime.Now.Year,
                value = new Random().NextDouble() * 1000,
                timestamp = DateTime.Now
            };

            return ts;
        }

        public async Task checkFirstRule()
        {
            StoredProcedureResponse<string> result = await client.ExecuteStoredProcedureAsync<string>(
                UriFactory.CreateStoredProcedureUri(dbName, collection, storedProcedure)
                , "111111"
                );
            Console.WriteLine(result);
        }
    }

    
}
