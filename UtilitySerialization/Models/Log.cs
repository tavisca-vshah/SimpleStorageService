using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageServiceBootcamp.Models
{
    public class Log
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("log_time")]
        public DateTime LogTime { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("app_name")]
        public string AppName { get; set; }

        [JsonProperty("binary_request")]
        public string BinaryRequest { get; set; }

        [JsonProperty("binary_response")]
        public string BinaryResponse { get; set; }

        
    }
    
}
