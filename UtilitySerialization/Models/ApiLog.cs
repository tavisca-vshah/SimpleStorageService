using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStorageServiceBootcamp.Models
{
   public class ApiLog
    {
        [JsonProperty("binary_request")]
        public string BinaryRequest { get; set; }

        [JsonProperty("binary_response")]
        public string BinaryResponse { get; set; }
    }
}
