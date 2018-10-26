using NPCGenerator.Controls;
using System;

namespace NPCGenerator.Util
{
    public class NpcTreeViewItemDoubleClickedEventArgs : EventArgs
    {
        public NpcTreeViewItemDoubleClickedEventArgs(NpcTreeViewItem npcTreeViewItem)
        {
            NpcTreeViewItem = npcTreeViewItem;
        }
        public NpcTreeViewItem NpcTreeViewItem { get; }
    }
}
