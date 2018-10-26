using System.Collections.Generic;

namespace NPCGenerator.Model
{
    public class JsonDataContainer
    {

        public IEnumerable<Culture> Cultures { get; set; }
        public IEnumerable<Species> Species { get; set; }
        public IEnumerable<Level> XpLevels { get; set; }
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<string> Statures { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<string> Gender { get; set; }
    }
}
