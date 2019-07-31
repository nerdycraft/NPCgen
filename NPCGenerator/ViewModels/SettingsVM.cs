using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using NPCGenerator.Dto;
using NPCGenerator.WindowModels;
using NPCGenerator.Windows;

namespace NPCGenerator.ViewModels
{
    public class SettingsVM
    {
        public DataContainer Data { get; }

        public SettingsVM(DataContainer data) { Data = data; }

        private Settings settings;


        public void Run()
        {
            settings = new Settings( new SettingsWM { Talents = Data.Talents, Levels = Data.Levels, Statures = Data.Statures, Sizes = Data.Sizes, Gender = Data.Gender });
            settings.AddExperienceLevelClick += SettingsOnAddExperienceLevelClick;
            settings.DeleteExperienceLevelClick += SettingsOnDeleteExperienceLevelClick;
            settings.AddStatureClick += SettingsOnAddStatureClick;
            settings.DeleteStatureClick += SettingsOnDeleteStatureClick;
            settings.AddSizeClick += SettingsOnAddSizeClick;
            settings.DeleteSizeClick += SettingsOnDeleteSizeClick;
            settings.ShowDialog();
        }

        private void SettingsOnAddExperienceLevelClick(object sender, EventArgs e)
        {
            string newLevelName = settings.LevelName.Text.Trim();
            if (string.IsNullOrEmpty(newLevelName))
            {
                MessageBox.Show("Bitte gib einen Namen ein.");
                settings.LevelName.Focus();
                settings.LevelName.SelectAll();
                return;
            }

            if (Data.Levels.Any(l => string.Equals(l.Name, newLevelName, StringComparison.CurrentCultureIgnoreCase)))
            {
                MessageBox.Show("Dieser Name wird bereits verwendet.");
                settings.LevelName.Focus();
                settings.LevelName.SelectAll();
                return;
            }

            settings.LevelName.Text = string.Empty;
            Data.Levels.Add(new Level {Name = newLevelName});
            settings.LevelGrid.ScrollIntoView(Data.Levels.Last());
        }

        private void SettingsOnDeleteExperienceLevelClick(object sender, EventArgs e)
        {
            if (!(((Button)sender).Tag is Level delLevel))
                return;

            Data.Levels.Remove(delLevel);
        }

        private void SettingsOnAddStatureClick(object sender, EventArgs e)
        {
            string newStatureName = settings.StaturName.Text.Trim();
            if (string.IsNullOrEmpty(newStatureName))
            {
                MessageBox.Show("Bitte gib einen Namen ein.");
                settings.StaturName.Focus();
                settings.StaturName.SelectAll();
                return;
            }

            if (Data.Statures.Any(s => string.Equals(s, newStatureName, StringComparison.CurrentCultureIgnoreCase)))
            {
                MessageBox.Show("Dieser Name wird bereits verwendet.");
                settings.StaturName.Focus();
                settings.StaturName.SelectAll();
                return;
            }

            settings.StaturName.Text = string.Empty;
            Data.Statures.Add(newStatureName);
            settings.StaturGrid.ScrollIntoView(newStatureName);
        }

        private void SettingsOnDeleteStatureClick(object sender, EventArgs e)
        {
            if (!(((Button)sender).Tag is string delStature))
                return;

            Data.Statures.Remove(delStature);
        }

        private void SettingsOnAddSizeClick(object sender, EventArgs e)
        {
            string newSizeName = settings.SizeName.Text.Trim();
            if (string.IsNullOrEmpty(newSizeName))
            {
                MessageBox.Show("Bitte gib einen Namen ein.");
                settings.SizeName.Focus();
                settings.SizeName.SelectAll();
                return;
            }

            if (Data.Statures.Any(s => string.Equals(s, newSizeName, StringComparison.CurrentCultureIgnoreCase)))
            {
                MessageBox.Show("Dieser Name wird bereits verwendet.");
                settings.SizeName.Focus();
                settings.SizeName.SelectAll();
                return;
            }

            settings.SizeName.Text = string.Empty;
            Data.Sizes.Add(newSizeName);
            settings.SizeGrid.ScrollIntoView(newSizeName);
        }

        private void SettingsOnDeleteSizeClick(object sender, EventArgs e)
        {
            if (!(((Button)sender).Tag is string delSize))
                return;

            Data.Sizes.Remove(delSize);
        }
    }
}