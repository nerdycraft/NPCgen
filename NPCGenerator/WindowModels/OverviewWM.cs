
using NPCGenerator.Dto;

using System.Collections.ObjectModel;

// ReSharper disable InconsistentNaming

namespace NPCGenerator.WindowModels
{
    public class OverviewWM
    {
        public ObservableCollection<NPC> NPCs { get; } = new ObservableCollection<NPC>();
    }
}
