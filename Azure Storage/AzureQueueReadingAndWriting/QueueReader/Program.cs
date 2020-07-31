using System;
using System.Threading;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types
using Common;
namespace QueueReader
{
    class Program
    {
        static void Main(string[] args)
        {
            // Retrieve storage account from connection string
            string storageConnectionString = Common.ConnectionString.getConnString();
                
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);


            // Create the queue client
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Peek at the next message
            while(true){
                CloudQueueMessage retrievedMessage = queue.GetMessage();
                Console.WriteLine(retrievedMessage.AsString);
                // Process all messages in less than 5 minutes, deleting each message after processing.
                queue.DeleteMessage(retrievedMessage);
                Thread.Sleep(500);
                
            }
            // Display message.
            
        }
    }
}
