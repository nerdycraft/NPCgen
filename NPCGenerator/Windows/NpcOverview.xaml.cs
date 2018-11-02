using System;

using NPCGenerator.Controls;
using NPCGenerator.Util;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using NPCGenerator.Model;

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
        private readonly NpcOverviewVM vm;
        public NpcOverview(NpcOverviewVM vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = this.vm;

            var root = new NpcTreeViewItem(new DirectoryInfo(vm.NpcPath), "*.json");
            tree.Items.Add(root);
            root.IsExpanded = true;
            tree.ItemDoubleClicked += Tree_ItemDoubleClicked;
        }

        private void Tree_ItemDoubleClicked(object sender, NpcTreeViewItemDoubleClickedEventArgs e)
        {
            if (!e.NpcTreeViewItem.IsDirectoryNode)
                vm.LoadNpcFromFile(e.NpcTreeViewItem.FullPath);
        }

        private void NewFolder_Click(object sender, RoutedEventArgs e)
        {
            if (tree.SelectedItem == null)
                return;

            var input = new InputDialog("Ordnernamen eingeben :)");
            if (input.ShowDialog() != true) return;

            var item = (NpcTreeViewItem)tree.SelectedItem;
            if (!item.IsDirectoryNode)
                item = (NpcTreeViewItem)item.Parent;

            CreateFolderNode(item, input.Input);
        }

        private void DeleteFile_Click(object sender, RoutedEventArgs e)
        {
            if (tree.SelectedItem == null)
                return;

            if (MessageBox.Show("Wirklich löschen?", ":o", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            var item = (NpcTreeViewItem)tree.SelectedItem;

            if (item.IsDirectoryNode)
            {
                if (((DirectoryInfo)item.Tag).Name != vm.NpcPath)
                    ((DirectoryInfo)item.Tag).Delete();
                else
                    MessageBox.Show("Root Verzeichnis kann nicht gelöscht werden");
            }
            else
                ((FileInfo)item.Tag).Delete();
            ((NpcTreeViewItem)item.Parent).Items.Remove(item);
        }

        private void CreateFolderNode(ItemsControl item, string folderName)
        {
            if (item.Tag is DirectoryInfo di)
            {
                if (!vm.IsValidFolderName(di, folderName))
                    MessageBox.Show("Pfad ungültig oder so.");
                else
                {
                    var newDi = Directory.CreateDirectory(Path.Combine(di.FullName, folderName));

                    item.Items.Add(new NpcTreeViewItem(newDi, "*.json"));
                }
            }
        }

        private DateTime lastClick = DateTime.Now;
        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle)
                vm.RemoveNpc(Tabs.SelectedItem);
            else if (e.ChangedButton == MouseButton.Left)
            {
                if (DateTime.Now.Subtract(lastClick).TotalMilliseconds <= 500)
                {
                    new NpcDetails(Tabs.SelectedItem as NPC).Show();
                    vm.RemoveNpc(Tabs.SelectedItem);
                }

                lastClick = DateTime.Now;
            }
        }
    }
}