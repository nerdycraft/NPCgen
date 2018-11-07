using System.Collections.Generic;
using System.Collections.ObjectModel;

using NPCGenerator.Model;

namespace NPCGenerator.Util
{
    public class JobDesignerVM
    {
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<Talent> Talents { get; set; }
    }
}