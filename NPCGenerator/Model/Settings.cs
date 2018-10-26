using Newtonsoft.Json;

namespace NPCGenerator.Model
{
    public class Settings
    {
        [JsonProperty( "defaultgen" ), JsonRequired]
        public bool UseDefaultGeneration { get; set; }

        [JsonProperty( "talents" ), JsonRequired]
        public IgnoreTalent[] Talents { get; set; }
    }

    public class IgnoreTalent
    {
        [JsonProperty( "name" ), JsonRequired]
        public string Name { get; set; }

        [JsonProperty( "ignore" ), JsonRequired]
        public bool Ignore { get; set; }

        [JsonProperty( "attr1" ), JsonRequired]
        public string Attr1 { get; set; }

        [JsonProperty( "attr2" ), JsonRequired]
        public string Attr2 { get; set; }

        [JsonProperty( "attr3" ), JsonRequired]
        public string Attr3 { get; set; }
    }
}
