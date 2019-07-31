using System.Collections.Generic;
using System.Collections.ObjectModel;

using NPCGenerator.Dto;

namespace NPCGenerator.WindowModels
{
    public class SettingsWM
    {
        public IEnumerable<Talent> Talents { get; set; }

        public ObservableCollection<Level> Levels { get; set; }
        public ObservableCollection<string> Statures { get; set; }
        public ObservableCollection<string> Sizes { get; set; }
        public ObservableCollection<Gender> Gender { get; set; }
    }
}