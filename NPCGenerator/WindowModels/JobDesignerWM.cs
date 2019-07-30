using System.Collections.Generic;

using NPCGenerator.Dto;

namespace NPCGenerator.WindowModels
{
    public class JobDesignerWM
    {
        public IEnumerable<Job> Jobs { get; set; }
        public IEnumerable<Talent> Talents { get; set; }
    }
}