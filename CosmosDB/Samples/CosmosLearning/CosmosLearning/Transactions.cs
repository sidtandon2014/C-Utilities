using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosLearning
{
    public class Transactions
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string TransactionID { get; set; }
        public double Amount { get; set; }
        public string Channel { get; set; }
        public string CustomerStatus { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string TransactionType { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class MasterData
    {
        public static string[] Channels = new string[] { "ATM", "Mobile", "USSD", "IBS", "WEB", "ATM" };
        public static string[] TransactionType = new string[] { "Debit", "Credit" };
        public static string[] CustomerStatus = new string[] { "Active", "InActive" };
        public static string[] FromAccountId = new string[] { "11111", "222222", "33333", "444444", "555555" };
        public static string[] ToAccountId = new string[] { "66666", "777777", "88888", "999999" };
    }
}