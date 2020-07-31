using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace SendEvents
{
    class Program
    {
        private static EventHubClient eventHubClient;
        static string eventHubName = "ehsampledata ";
        //static string connectionString = "Endpoint=sb://.servicebus.windows.net/;SharedAccessKeyName=RooT9XeRsomz9XEMwGlA/xEZFFCGKr+yIgHLati95c=";
        static string connectionString = "Endpoint=sb://.servicebus.windows.net/;SharedAccessKeyName=RootManagOEsz92V6+=";
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(connectionString)
            {
                EntityPath = eventHubName

            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
            Task[] allTask = new Task[10000];
            for (int i = 0; i < allTask.Length; i++) //Run 10 methods simultaneously 
            {
                var myTask = Task.Run(async () =>
                {
                    await SendMessagesToEventHub(100, 1);
                }
                );
                allTask[i] = myTask;
            }

            await Task.WhenAll(allTask);

            await eventHubClient.CloseAsync();
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static async Task SendMessagesToEventHub(int numMessagesToSend, int startingIndex)
        {
            int index = startingIndex;
            int count = startingIndex + numMessagesToSend;
            String[] users = new String[] { "user1", "user2", "user3" };
            while (index < count)
            {
                try
                {
                    BladeData data = new BladeData()
                    {
                        user = users[new Random().Next(0, users.Length)],
                        activity = "Hello_" + eventHubName + "_" + index.ToString(),
                        timestamp = DateTime.Now
                        /*id = 1,
                        name = "Test",
                        value = new Random().Next(100) * .10,
                        currentTime = DateTime.Now*/
                    };
                    var message = JsonConvert.SerializeObject(data);
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                    Console.WriteLine("Sent message Index: {0}, User: {1}, timestamp: {2}", index,
                        data.user, data.timestamp);
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                    Console.ResetColor();
                }

                //Console.ReadLine();
                index++;
            }
        }
    }

    class BladeData
    {
        public string user { get; set; }
        public string activity { get; set; }

        public DateTime timestamp { get; set; }
        /*public int id { get; set; }
        public string name { get; set; }
        public double value { get; set; }

        public DateTime currentTime { get; set; }
        */
    }
}
