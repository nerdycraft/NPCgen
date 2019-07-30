using Newtonsoft.Json;

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

        [JsonProperty( "mod" ), JsonRequired]
        public AttrMod Mod { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class AttrMod
    {
        [JsonProperty( "rnd" )]
        public long? Rnd { get; set; }

        [JsonProperty( "and" )]
        public StatMod And { get; set; }

        [JsonProperty( "or" )]
        public StatMod Or { get; set; }
    }

    public class StatMod
    {
        [JsonProperty( "value" ), JsonRequired]
        public long Value { get; set; }

        [JsonProperty( "stats" ), JsonRequired]
        public string[] Stats { get; set; }
    }
}
