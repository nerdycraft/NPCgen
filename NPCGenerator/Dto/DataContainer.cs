using System.Collections.Generic;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

namespace NPCGenerator.Dto
{
    public class DataContainer
    {
        [JsonIgnore]
        public IEnumerable<Culture> Cultures { get; set; }
        [JsonIgnore]
        public IEnumerable<Species> Species { get; set; }
        [JsonIgnore]
        public ObservableCollection<Job> Jobs { get; set; }

        [JsonProperty("talents"), JsonRequired]
        public IEnumerable<Talent> Talents { get; set; }

        [JsonProperty("levels"), JsonRequired]
        public ObservableCollection<Level> Levels { get; set; }

        [JsonProperty("statures"), JsonRequired]
        public ObservableCollection<string> Statures { get; set; }

        [JsonProperty("sizes"), JsonRequired]
        public ObservableCollection<string> Sizes { get; set; }

        [JsonProperty("gender"), JsonRequired]
        public ObservableCollection<Gender> Gender { get; set; }
    }
}
