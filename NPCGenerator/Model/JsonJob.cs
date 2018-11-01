using Newtonsoft.Json;

namespace NPCGenerator.Model
{
    public class Job
    {
        [JsonProperty( "name" ), JsonRequired]
        public string Name { get; set; }

        [JsonProperty( "femname" ), JsonRequired]
        public string FemName { get; set; }

        [JsonProperty( "basename" ), JsonRequired]
        public string ReferenceName { get; set; }

        [JsonProperty( "statweight" ), JsonRequired]
        public Statweight Statweight { get; set; }

        [JsonProperty( "talents" ), JsonRequired]
        public Talent[] Talents { get; set; }

        [JsonProperty( "weapon" )]
        public Weapon Weapon { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Statweight
    {
        private long kl, i, ch, ff, ge, ko, kk;

        [JsonProperty( "MU" ), JsonRequired]
        public long Mu { get; set; }

        [JsonProperty( "KL" ), JsonRequired]
        public long Kl { get => kl + Mu;
            set => kl = value;
        }

        [JsonProperty( "IN" ), JsonRequired]
        public long In { get => i + Kl;
            set => i = value;
        }

        [JsonProperty( "CH" ), JsonRequired]
        public long Ch { get => ch + In;
            set => ch = value;
        }

        [JsonProperty( "FF" ), JsonRequired]
        public long Ff { get => ff + Ch;
            set => ff = value;
        }

        [JsonProperty( "GE" ), JsonRequired]
        public long Ge { get => ge + Ff;
            set => ge = value;
        }

        [JsonProperty( "KO" ), JsonRequired]
        public long Ko { get => ko + Ge;
            set => ko = value;
        }

        [JsonProperty( "KK" ), JsonRequired()]
        public long Kk { get => kk + Ko;
            set => kk = value;
        }
    }

    public class Weapon
    {
        [JsonProperty( "name" ), JsonRequired()]
        public string Name { get; set; }

        [JsonProperty( "at" )]
        public long At { get; set; }

        [JsonProperty( "pa" )]
        public long Pa { get; set; }

        [JsonProperty( "fk" )]
        public long Fk { get; set; }

        [JsonProperty( "tp" ), JsonRequired()]
        public string Tp { get; set; }

        [JsonProperty( "rw" )]
        public string Rw { get; set; }
    }
}
