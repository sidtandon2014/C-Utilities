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

namespace AirlineData
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Starting the demo");
            Ticket ticket = new Ticket();
            ticket.initializeCosmosAccount().Wait();
            //cosmosLearning.checkFirstRule().Wait();
            Console.ReadKey();
        }
    }

    class Ticket
    {
        private string dbName = "airlinedb";
        private string collection = "userContainer";
        private DocumentClient client;
        private const string EndpointUrl = "https://sidcosmosaccount.documents.azure.com:443/";
        private const string PrimaryKey = "VgAp0Spm3hWGSNMq4HE5RNU0L8FltboD0xwvhiqPyDUFpbWJ0j0mMBeSQFtViQgv5A3R5NfKnIjR2UPeXyfI9Q==";
        private string[] cities = new string[5] { "Delhi", "Hyderabad", "Bangalore", "Mumbai", "Lucknow" };
        public Ticket()
        {
            client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
        }

        public async Task initializeCosmosAccount()
        {
            await this.createDatabaseIfNotExists();
            await this.createCollectionIFNotExists();
            //await this.CreateTransactionDocumentIfNotExists(100);
            await this.CreateTicketDocumentIfNotExists(10000000);
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

        private async Task CreateTicketDocumentIfNotExists(int totalDocs)
        {
            int index = 100000;
            while (index < totalDocs)
            //async Parallel.For(0, totalDocs, index =>
            {
                try
                {
                    TicketInfo ts = issueNewTicket(index);
                    await this.client
                        .CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(dbName, collection)
                            , ts
                        );

                    if (index % 100 == 0)
                    {
                        Console.WriteLine("Created Document: " + index.ToString());
                    }
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

        private TicketInfo issueNewTicket(int id)
        {
            string fname = GenerateName(5);
            string lname = GenerateName(5);
            string city = cities[id % cities.Length];
            TicketInfo ts = new TicketInfo()
            {
                Id = id.ToString(),
                UserId = fname + "@test.com",
                firstName = fname,
                lastName = lname,
                pnr = "tag" + id.ToString(),
                cityOfOrigin = city
            };

            return ts;
        }

        public static string GenerateName(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "v" };
            string[] vowels = { "a", "e", "i", "o", "u" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;


        }
    }


    public class TicketInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string pnr { get; set; }
        public string cityOfOrigin { get; set; }

    }

}
