using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NPCGenerator.Dto
{
    public class Gender
    {
        [JsonProperty("id"), JsonRequired]
        public string Id { get; set; }

        [JsonProperty("namelist")]
        [JsonConverter(typeof(StringEnumConverter))]
        public NamingList NameList { get; set; }

        public enum NamingList
        {
            Male,
            Female
        }

        public override string ToString() { return Id; }
    }
}
