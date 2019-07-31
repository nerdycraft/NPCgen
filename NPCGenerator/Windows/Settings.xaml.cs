using System;
using System.Windows;

using NPCGenerator.WindowModels;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Nothing to see here, move along!
    /// </summary>
    public partial class Settings
    {
        public event EventHandler AddExperienceLevelClick;
        public event EventHandler DeleteExperienceLevelClick;
        public event EventHandler AddStatureClick;
        public event EventHandler DeleteStatureClick;
        public event EventHandler AddSizeClick;
        public event EventHandler DeleteSizeClick;
        public event EventHandler AddGenderClick;
        public event EventHandler DeleteGenderClick;

        public Settings(SettingsWM data)
        {
            InitializeComponent();
            DataContext = data;
        }

        private void OnAddExperienceLevelClicked(object sender, RoutedEventArgs e)
        {
            AddExperienceLevelClick?.Invoke(sender, e);
        }

        private void OnDeleteExpLevelClick(object sender, RoutedEventArgs e)
        {
            DeleteExperienceLevelClick?.Invoke(sender, e);
        }

        private void OnAddStatureClicked(object sender, RoutedEventArgs e)
        {
            AddStatureClick?.Invoke(sender, e);
        }

        private void OnDeleteStatureClick(object sender, RoutedEventArgs e)
        {
            DeleteStatureClick?.Invoke(sender, e);
        }

        private void OnAddSizeClicked(object sender, RoutedEventArgs e)
        {
            AddSizeClick?.Invoke(sender, e);
        }

        private void OnDeleteSizeClick(object sender, RoutedEventArgs e)
        {
            DeleteSizeClick?.Invoke(sender, e);
        }

        private void OnAddGenderClicked(object sender, RoutedEventArgs e)
        {
            AddGenderClick?.Invoke(sender, e);
        }

        private void OnDeleteGenderClick(object sender, RoutedEventArgs e)
        {
            DeleteGenderClick?.Invoke(sender, e);
        }
    }
}
