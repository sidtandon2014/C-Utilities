using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types

namespace AzureQueueReadingAndWriting
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnectionString = Common.ConnectionString.getConnString();
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            // Create a message and add it to the queue.
            int index = 0;
            while (index < 100) {
                CloudQueueMessage message = new CloudQueueMessage("Messsage ID " + index.ToString());
                queue.AddMessage(message);
                Console.WriteLine("Message Written " + index.ToString());
                index++;
                Thread.Sleep(100);
            }
            Console.Read();
        }
    }
}
