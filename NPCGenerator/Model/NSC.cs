using System;

using Newtonsoft.Json;
using System.ComponentModel;

namespace NPCGenerator.Model
{
    public class NPC : INotifyPropertyChanged
    {
        [JsonProperty( "generatedonbuild" )]
        public int Build { get; set; }

        [JsonProperty( "level" )]
        public string Level { get; set; }

        [JsonProperty( "species" )]
        public string Species { get; set; }
        [JsonProperty( "culture" )]
        public string Culture { get; set; }

        [JsonProperty( "name" )]
        public string Name { get; set; }

        [JsonProperty( "age" )]
        public uint Age { get; set; }
        [JsonProperty( "size" )]
        public string Size { get; set; }
        [JsonProperty( "gender" )]
        public string Gender { get; set; }
        [JsonProperty( "stature" )]
        public string Stature { get; set; }
        [JsonProperty( "charakter" )]
        public string Charakter { get; set; }

        [JsonProperty( "job" )]
        public string Job { get; set; }

        [JsonProperty( "attributes" )]
        public Attributes Attributes { get; set; }
        [JsonProperty( "stats" )]
        public Stats Stats { get; set; }
        [JsonProperty( "talents" )]
        public Talent[] Talents { get; set; }

        private bool isSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( "IsSelected" ) );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return $"{Species}_{Culture}_{Job}-{Level}-{Name}";
        }
    }

    public class Attributes
    {
        public Attributes()
        {
            Mu = Kl = In = Ch = Ff = Ge = Ko = Kk = 8;
        }

        //MUT
        [JsonProperty( "mu" )]
        public uint Mu { get; set; }
        //Klugheit
        [JsonProperty( "kl" )]
        public uint Kl { get; set; }
        //Intuition
        [JsonProperty( "in" )]
        public uint In { get; set; }
        //Charisma
        [JsonProperty( "ch" )]
        public uint Ch { get; set; }
        //Fingerfertigkeit
        [JsonProperty( "ff" )]
        public uint Ff { get; set; }
        //Gewandheit
        [JsonProperty( "ge" )]
        public uint Ge { get; set; }
        //Konmstitution
        [JsonProperty( "ko" )]
        public uint Ko { get; set; }
        //Körperkraft
        [JsonProperty( "kk" )]
        public uint Kk { get; set; }

        public uint GetFromStr(string attr)
        {
            switch (attr)
            {
            case "MU":
                return Mu;
            case "KL":
                return Kl;
            case "IN":
                return In;
            case "CH":
                return Ch;
            case "FF":
                return Ff;
            case "GE":
                return Ge;
            case "KK":
                return Kk;
            case "KO":
                return Ko;
            default:
                throw new ArgumentException("yea... you shouldnt be here");
            }
        }
    }

    public class Stats
    {
        //Geschwindigkeit
        [JsonProperty( "gs" )]
        public uint Gs { get; set; }
        [JsonProperty( "lp" )]
        public uint Lp { get; set; }
        [JsonProperty( "aw" )]
        public uint Aw { get; set; }
        [JsonProperty( "ini" )]
        public uint Ini { get; set; }
        [JsonProperty( "sk" )]
        public int Sk { get; set; }
        [JsonProperty( "zk" )]
        public int Zk { get; set; }
    }
}
