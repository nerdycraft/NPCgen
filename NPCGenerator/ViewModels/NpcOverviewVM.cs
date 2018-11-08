
using NPCGenerator.Model;
using System.Collections.Generic;
// ReSharper disable InconsistentNaming

namespace NPCGenerator.ViewModels
{
    public class NpcOverviewVM
    {
        public NpcOverviewVM(IEnumerable<NPC> npcs) { NPCs = npcs; }

        public IEnumerable<NPC> NPCs { get; }
    }
}
