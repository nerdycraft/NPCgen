using Newtonsoft.Json;
using System.Collections.Generic;

namespace NPCGenerator.Model
{
    public class StringData
    {
        [JsonProperty( "data" ), JsonRequired]
        public IEnumerable<string> Data { get; set; }
    }
}
