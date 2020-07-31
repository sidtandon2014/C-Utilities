using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CosmosLearning
{
    public class TimeSeries
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string location { get; set; }
        public string unit { get; set; }
        public string equipment { get; set; }
        public string tagid { get; set; }
        public int year { get; set; }
        public double value { get; set; }
        public DateTime timestamp { get; set; }
        
    }
}
