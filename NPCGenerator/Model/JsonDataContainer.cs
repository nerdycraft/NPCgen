using System.Collections.Generic;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

namespace NPCGenerator.Model
{
    public class JsonDataContainer
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
        public IEnumerable<Level> Levels { get; set; }

        [JsonProperty("statures"), JsonRequired]
        public IEnumerable<string> Statures { get; set; }

        [JsonProperty("sizes"), JsonRequired]
        public IEnumerable<string> Sizes { get; set; }

        [JsonProperty("gender"), JsonRequired]
        public IEnumerable<Gender> Gender { get; set; }
    }
}
