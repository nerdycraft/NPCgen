using System.Collections.Generic;
using System.Collections.ObjectModel;

using NPCGenerator.Model;

namespace NPCGenerator.Util
{
    public class JobDesignerVM
    {
        public ObservableCollection<Job> Jobs { get; } = new ObservableCollection<Job>();
        public IEnumerable<Talent> Talents { get; set; }

        public bool EditMode { get; set; }
        public bool NewMode { get; set; }
    }
}