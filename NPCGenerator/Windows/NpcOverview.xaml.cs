using System;

using System.IO;
using System.Windows;
using System.Windows.Input;

using NPCGenerator.Controls;
using NPCGenerator.Model;
using NPCGenerator.Util;
using NPCGenerator.ViewModels;

// ReSharper disable StringLiteralTypo

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Interaction logic for the npc overview
    /// </summary>
    public partial class NpcOverview
    {
        public event EventHandler<NpcTreeViewItem> TreeItemDoubleClicked;
        public event EventHandler<NpcTreeViewItem> NewFolderClicked;
        public event EventHandler<NpcTreeViewItem> DeleteItemClicked;
        public event EventHandler<NPC> TabMiddleClick;
        public event EventHandler<NPC> TabDoubleClick;

        public NpcOverview(NpcOverviewVM vm)
        {
            InitializeComponent();
            DataContext = vm;

            var root = new NpcTreeViewItem(new DirectoryInfo(References.OUT_FOLDER), "*.json");
            Tree.Items.Add(root);
            root.IsExpanded = true;
            Tree.ItemDoubleClicked += (sender, item) => TreeItemDoubleClicked?.Invoke(sender, item);
        }

        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {
            NewFolderClicked?.Invoke(sender, Tree.SelectedItem as NpcTreeViewItem);
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            DeleteItemClicked?.Invoke(sender, Tree.SelectedItem as NpcTreeViewItem);
        }

        private DateTime lastClick = DateTime.Now;
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
                TabMiddleClick?.Invoke(sender, Tabs.SelectedItem as NPC);
            else if (e.ChangedButton == MouseButton.Left)
            {
                if (DateTime.Now.Subtract(lastClick).TotalMilliseconds <= 500)
                    TabDoubleClick?.Invoke(sender, Tabs.SelectedItem as NPC);

                lastClick = DateTime.Now;
            }
        }
    }
}