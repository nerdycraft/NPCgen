using System.ComponentModel;

using Newtonsoft.Json;

namespace NPCGenerator.Model
{
    public class Talent
    {
        [JsonProperty("id"), JsonRequired]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [DefaultValue(0)]
        [JsonProperty("value", DefaultValueHandling = DefaultValueHandling.Populate)]
        public uint Value { get; set; }

        [DefaultValue(-1)]
        [JsonProperty("weight", DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Weight { get; set; }

        [DefaultValue(false)]
        [JsonProperty("ignore", DefaultValueHandling = DefaultValueHandling.Populate)]
        public bool Ignore { get; set; }

        [JsonProperty("attr")]
        public string[] Attr { get; set; }

        [JsonIgnore]
        public string Format => string.Join("/", Attr);
    }
}