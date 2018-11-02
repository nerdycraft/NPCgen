using NPCGenerator.Model;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class NpcDetails
    {
        public NpcDetails(NPC npc)
        {
            InitializeComponent();
            Details.DataContext = npc;
            Title = npc.Name;
        }
    }
}
