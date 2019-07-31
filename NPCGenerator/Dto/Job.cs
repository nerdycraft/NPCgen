using System.Collections.ObjectModel;

using Newtonsoft.Json;

namespace NPCGenerator.Dto
{
    public class Job
    {
        [JsonProperty("basename"), JsonRequired]
        public string ReferenceName { get; set; } = string.Empty;

        [JsonProperty("name"), JsonRequired]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("femname"), JsonRequired]
        public string FemName { get; set; } = string.Empty;

        [JsonProperty("statweight"), JsonRequired]
        public Statweight Statweight { get; set; } = new Statweight();

        [JsonProperty( "talents" ), JsonRequired]
        public ObservableCollection<Talent> Talents { get; set; } = new ObservableCollection<Talent>();

        [JsonProperty( "weapon" )]
        public Weapon Weapon { get; set; } = new Weapon();

        public override string ToString()
        {
            return $"{Name}/{FemName}";
        }

        [JsonIgnore] public bool IsNew { get; set; }
    }

    public class Statweight
    {
        [JsonProperty( "MU" ), JsonRequired]
        public int Mu { get; set; }

        [JsonProperty( "KL" ), JsonRequired]
        public int Kl { get; set; }

        [JsonProperty( "IN" ), JsonRequired]
        public int In { get; set; }

        [JsonProperty( "CH" ), JsonRequired]
        public int Ch { get; set; }

        [JsonProperty( "FF" ), JsonRequired]
        public int Ff { get; set; }

        [JsonProperty( "GE" ), JsonRequired]
        public int Ge { get; set; }

        [JsonProperty( "KO" ), JsonRequired]
        public int Ko { get; set; }

        [JsonProperty( "KK" ), JsonRequired]
        public int Kk { get; set; }

        [JsonIgnore]
        public int CumMu => Mu;
        [JsonIgnore]
        public int CumKl => Kl + CumMu;
        [JsonIgnore]
        public int CumIn => In + CumKl;
        [JsonIgnore]
        public int CumCh => Ch + CumIn;
        [JsonIgnore]
        public int CumFf => Ff + CumCh;
        [JsonIgnore]
        public int CumGe => Ge + CumFf;
        [JsonIgnore]
        public int CumKo => Ko + CumGe;
        [JsonIgnore]
        public int CumKk => Kk + CumKo;
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
