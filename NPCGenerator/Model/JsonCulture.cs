using Newtonsoft.Json;

namespace NPCGenerator.Model
{

    public class Culture
    {
        [JsonProperty( "name"), JsonRequired]
        public string Name { get; set; }

        [JsonProperty( "parentspecies" ), JsonRequired]
        public int[] DefaultSpecies { get; set; }

        [JsonProperty( "language"), JsonRequired]
        public Language Language { get; set; }

        [JsonProperty( "defaultjobs"), JsonRequired]
        public string[] DefaultJobs { get; set; }

        [JsonProperty( "talents" ), JsonRequired]
        public Talent[] Talents { get; set; }

        [JsonProperty( "names" ), JsonRequired]
        public Names Names { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Language
    {
        [JsonProperty( "name"), JsonRequired]
        public string Name { get; set; }
        [JsonProperty( "dialect")]
        public string Dialect { get; set; }
        [JsonProperty( "font")]
        public string Font { get; set; }
    }

    public class Names
    {
        [JsonProperty( "female" ), JsonRequired]
        public string[] Female { get; set; }

        [JsonProperty( "male" ), JsonRequired]
        public string[] Male { get; set; }

        [JsonProperty( "last" ), JsonRequired]
        public string[] Last { get; set; }

        [JsonProperty( "suffix" )]
        public string[] Suffix { get; set; }

        [JsonProperty( "formatmale" )]
        public string FormatMale { get; set; }

        [JsonProperty( "formatfemale" )]
        public string FormatFemale { get; set; }

        [JsonProperty( "formatmalelast" )]
        public string FormatMaleLast { get; set; }

        [JsonProperty( "formatfemalelast" )]
        public string FormatFemaleLast { get; set; }
    }
}
