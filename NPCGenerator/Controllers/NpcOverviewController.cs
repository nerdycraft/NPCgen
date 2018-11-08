using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Newtonsoft.Json;

using NPCGenerator.Controls;
using NPCGenerator.Model;
using NPCGenerator.Util;
using NPCGenerator.ViewModels;
using NPCGenerator.Windows;

using NpcDetails = NPCGenerator.Windows.NpcDetails;

namespace NPCGenerator.Controllers
{
    public class NpcOverviewController
    {
        private readonly ObservableCollection<NPC> npcs = new ObservableCollection<NPC>();

        public void Run()
        {
            var no = new NpcOverview(new NpcOverviewVM(npcs));
            no.TreeItemDoubleClicked += (sender, item) => OnTreeItemDoubleClicked(item);
            no.NewFolderClicked += (sender, item) => NewFolderClicked(item);
            no.DeleteItemClicked += (sender, item) => DeleteItemClicked(item);
            no.TabMiddleClick += (sender, npc) => TabMiddleClicked(npc);
            no.TabDoubleClick += (sender, npc) => TabDoubleClicked(npc);
            no.ShowDialog();
        }

        private void OnTreeItemDoubleClicked(NpcTreeViewItem item)
        {
            if (!item.IsDirectoryNode)
                LoadNpcFromFile(item.FullPath);
        }

        private void NewFolderClicked(NpcTreeViewItem selectedItem)
        {
            if (selectedItem == null)
                return;

            var input = new InputDialog("Ordnernamen eingeben :)");
            if (input.ShowDialog() != true) return;

            if (!selectedItem.IsDirectoryNode)
                selectedItem = (NpcTreeViewItem)selectedItem.Parent;

            CreateFolderNode(selectedItem, input.Input);
        }

        private void DeleteItemClicked(NpcTreeViewItem selectedItem)
        {
            if (selectedItem == null)
                return;

            if (MessageBox.Show("Wirklich löschen?", ":o", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;

            if (selectedItem.IsDirectoryNode)
            {
                if (((DirectoryInfo)selectedItem.Tag).Name != References.OUT_FOLDER)
                    ((DirectoryInfo)selectedItem.Tag).Delete();
                else
                    MessageBox.Show("Root Verzeichnis kann nicht gelöscht werden");
            }
            else
                ((FileInfo)selectedItem.Tag).Delete();
            ((NpcTreeViewItem)selectedItem.Parent).Items.Remove(selectedItem);
        }

        private void TabMiddleClicked(NPC clickedItem)
        {
            if (clickedItem != null)
                npcs.Remove(clickedItem);
        }
        private void TabDoubleClicked(NPC clickedItem)
        {
            if (clickedItem != null)
            {
                new NpcDetails(clickedItem).Show();
                npcs.Remove(clickedItem);
            }
        }
        public void LoadNpcFromFile(string path)
        {
            var npc = JsonConvert.DeserializeObject<NPC>(File.ReadAllText(path));
            if (npcs.Any(n => n.ToString() == npc.ToString()))
            {
                npcs.First(n => n.ToString() == npc.ToString()).IsSelected = true;
                return;
            }

            npcs.Add(npc);
            npc.IsSelected = true;
        }

        private void CreateFolderNode(ItemsControl item, string folderName)
        {
            if (item.Tag is DirectoryInfo di)
            {
                if (!IsValidFolderName(di, folderName))
                    MessageBox.Show("Pfad ungültig oder so.");
                else
                {
                    var newDi = Directory.CreateDirectory(Path.Combine(di.FullName, folderName));

                    item.Items.Add(new NpcTreeViewItem(newDi, "*.json"));
                }
            }
        }

        public bool IsValidFolderName(DirectoryInfo parentDi, string folderName)
        {
            return folderName != string.Empty && !Directory.Exists(Path.Combine(parentDi.FullName, folderName)) && !Path.GetInvalidFileNameChars().Any(folderName.Contains) && forbidden.All(x => !string.Equals(folderName, x, StringComparison.CurrentCultureIgnoreCase));
        }
        private readonly string[] forbidden = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

    }
}