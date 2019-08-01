using System.Collections.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NPCGenerator.Dto
{
    public class Species
    {
        [JsonProperty("id"), JsonRequired]
        public string Id { get; set; }

        [JsonProperty( "name" ), JsonRequired]
        public string Name { get; set; }

        [JsonProperty( "baseHP" ), JsonRequired]
        public uint BaseHp { get; set; }

        [JsonProperty( "baseSK" ), JsonRequired]
        public int BaseSk { get; set; }

        [JsonProperty( "baseZK" ), JsonRequired]
        public int BaseZk { get; set; }

        [JsonProperty( "baseGS" ), JsonRequired]
        public uint BaseGs { get; set; }

        [JsonProperty( "mods" ), JsonRequired]
        public ObservableCollection<AttrMod> Mods { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AttrMod
    {
        [JsonProperty("value"), JsonRequired]
        public long Value { get; set; }

        [JsonProperty("stats"), JsonRequired]
        public string[] Stats { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ModType Type { get; set; }

        public override string ToString() { return $"{Type} | {Value} [{string.Join(",", Stats)}]"; }
    }

    public enum ModType
    {
        And,
        Or
    }
}
