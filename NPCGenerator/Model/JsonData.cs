using System.Collections.Generic;

using Newtonsoft.Json;

namespace NPCGenerator.Model
{
    public class DataFile
    {
        [JsonProperty("statures"), JsonRequired]
        public IEnumerable<string> Statures { get; set; }

        [JsonProperty("sizes"), JsonRequired]
        public IEnumerable<string> Sizes { get; set; }

        [JsonProperty("gender"), JsonRequired]
        public IEnumerable<string> Gender { get; set; }

        [JsonProperty( "levels" ), JsonRequired]
        public IEnumerable<Level> Levels { get; set; }
    }

    public class Level
    {
        [JsonProperty( "name" ), JsonRequired]
        public string Name { get; set; }

        [JsonProperty( "maxattr" ), JsonRequired]
        public long MaxAttr { get; set; }

        [JsonProperty( "attr" ), JsonRequired]
        public long Attr { get; set; }

        [JsonProperty( "maxfw" ), JsonRequired]
        public long MaxFw { get; set; }

        [JsonProperty( "fw" ), JsonRequired]
        public long Fw { get; set; }

        public override string ToString()
        {
            return $"{Name} \t ({Attr} - Attr | {Fw} - FW )";
        }
    }
}
