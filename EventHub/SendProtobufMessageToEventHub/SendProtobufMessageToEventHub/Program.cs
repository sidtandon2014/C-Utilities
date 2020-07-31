using BlockPlus;
using Google.Protobuf;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Protobuf
{
    class Program
    {
        private static List<Data> lstData = new List<Data>();
        private static int dataBatches = 100;
        private static int msgIdBatches = 10000;
        private static EventHubClient eventHubClient;
        static string eventHubName = "Test-1-11";

        static string connectionString = "Endpoint=sb://ssKey=uTfBK7No9nr7R1zRGwECcbMuwxTIHomF58JDwDqA+fY=";

        static void generateData(int batches)
        {
            int index = 0;
            while (index <= batches)
            {
                Data d = new Data()
                {
                    EnterpriseId = index,
                    AccountId = index,
                    AggregatorId = index,
                    TelcoId = index,
                    mcc = index,
                    mnc = index,
                    source_address = string.Format("Test@TanlaLtd{0}", index)
                };
                lstData.Add(d);
                index += 1;
            }
        }

        private static async Task Parallel(string[] args)
        {
            for (int threadid = 0; threadid < 32; threadid++)
            {
                string[] args1 = new string[2];
                args1[0] = (threadid * 5001).ToString();
                args1[1] = ((threadid * 5001) + 9999).ToString();
                MainAsync(args1);
                Console.WriteLine("ThreadId {0} started ----------------------------------", threadid);
            }


        }

        static void Main(string[] args)
        {
            //Parallel(args).GetAwaiter().GetResult();
            MainAsync(args).GetAwaiter().GetResult();
            Console.ReadLine();

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

            generateData(dataBatches);
            await SendMessagesToEventHub(2);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static ulong getGUID()
        {
            return (ulong)BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
        }

        static ulong LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return Convert.ToUInt64(Math.Abs(longRand % (max - min)) + min);
        }

        private static async Task SendMessagesToEventHub(int numMessagesToSend)
        {
            int index = 0, partIndex = 0; //0-8,
            int parts = 1; // new Random().Next(0, 5);
            ulong msgId = 123; // getGUID();
            while (index < numMessagesToSend)
            {
//                ulong msgId = getGUID();
                int randVal = new Random().Next(0, dataBatches - 1);
                partIndex = 1;
                while (partIndex <= parts)
                {
                    //randVal = 15;
                    try
                    {
                        SMSBuffer data = new SMSBuffer();
                        data.ServerInterface = new ServerInterface();
                        try
                        {
                            // 19 Mar to 20 Mar
                            data.ServerInterface.AcceptedTime = LongRandom(1584583289000, 1584683289000, new Random());
                            //Convert.ToUInt64(DateTimeOffset.Now.ToUnixTimeMilliseconds());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        data.ServerInterface.MessageId = msgId; // getGUID(); //msgIds[randValMsgId]; //
                        data.ServerInterface.EnterpriseId = lstData[randVal].EnterpriseId;
                        data.ServerInterface.AccountId = 52; // lstData[randVal].AccountId;
                        data.ServerInterface.Header = ServerInterface.Types.HEADER.Error;
                        data.ServerInterface.AccountType = ServerInterface.Types.ACCOUNT_TYPE.Postpaid;
                        data.ServerInterface.AccountMsgType = ServerInterface.Types.ACCOUNT_MSG_TYPE.Promo;


                        data.ClientInterface = new ClientInterface();
                        data.ClientInterface.SourceAddress = lstData[randVal].source_address;
                        data.ClientInterface.DestinationAddress = Convert.ToUInt64(new Random().Next(1, 10));
                        data.ClientInterface.Mcc = lstData[randVal].mcc;
                        data.ClientInterface.Mnc = lstData[randVal].mnc;

                        data.ClientInterface.ScheduleTime = Convert.ToUInt32(new Random().Next(1572582720, 1577766720));

                        if (!eventHubName.Contains("ingress"))
                        {
                            //Thread.Sleep(1000 * 60 * 1);
                            data.RouteInfo = new RouteInfo();
                            data.RouteInfo.TelcoId = lstData[randVal].TelcoId;
                            data.RouteInfo.AggregatorId = lstData[randVal].AggregatorId;
                            data.RouteInfo.RouterProcessedTime = LongRandom(1584583289000, 1584683289000, new Random()); //Convert.ToUInt64(DateTimeOffset.Now.ToUnixTimeMilliseconds());
                            data.RouteInfo.NumberOfParts = parts;
                            data.RouteInfo.PartNumber = partIndex;
                            data.RouteInfo.LmrefId = parts > 0 ? msgId : 0;
                            //data.RouteInfo.

                        }

                        if (eventHubName.Contains("cdrin"))
                        {
                            //Thread.Sleep(1000 * 60 * 1);
                            data.DrInfo = new DRInfo();
                            data.DrInfo.Status = (DRInfo.Types.STATUS)new Random().Next(1, 5);
                            data.DrInfo.ErrorCode = new Random().Next(1, 10);
                            data.DrInfo.StatusTime = LongRandom(1584583289000, 1584683289000, new Random());  //Convert.ToUInt64(DateTimeOffset.Now.ToUnixTimeMilliseconds());

                            data.PlatformErrorInfo = new PlatformErrorInfo();
                            data.PlatformErrorInfo.ErrorCode = new Random().Next(1, 10);
                        }

                        var message = data.ToByteArray();

                        await eventHubClient.SendAsync(new EventData(message));
                        Console.WriteLine("Sent message Index: " + index + "-" + partIndex);
                    }
                    catch (DivideByZeroException exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                        Console.ResetColor();
                    }
                    partIndex++;
                }
                index++;
            }
        }
    }

    class Data
    {
        public int EnterpriseId { get; set; }
        public int AccountId { get; set; }
        public int TelcoId { get; set; }
        public int AggregatorId { get; set; }

        public string source_address { get; set; }

        public int mcc { get; set; }
        public int mnc { get; set; }
    }
}
