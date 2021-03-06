﻿using System;

using System.Windows;
using System.Windows.Controls;

using NPCGenerator.Util;
using NPCGenerator.WindowModels;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class MainWindow
    {
        public event EventHandler<RoutedEventArgs> OpenJobDesignerClicked;
        public event EventHandler<RoutedEventArgs> OpenSpeciesDesignerClicked;
        public event EventHandler<RoutedEventArgs> OpenNpcOverviewClicked;
        public event EventHandler<RoutedEventArgs> OpenTalentSettingsClicked;
        public event EventHandler<RoutedEventArgs> GenerateClicked;
        public event EventHandler<RoutedEventArgs> ReloadJsonClicked; 

        public MainWindow(MainWM wm)
        {
            InitializeComponent();
            DataContext = wm;
        }

        private void NoGenCheckChange(object sender, RoutedEventArgs e)
        {
            if (e.Source is CheckBox cb)
            {
                var row = dgInput.Items.IndexOf(cb.DataContext);
                dgInput.GetCell(row, 1).IsEnabled = cb.IsChecked.HasValue && cb.IsChecked.Value;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MinWidth = ActualWidth;
            MinHeight = ActualHeight;
            MaxHeight = ActualHeight;
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            GenerateClicked?.Invoke(this, e);
        }

        private void ReloadJson_Click(object sender, RoutedEventArgs e)
        {
            ReloadJsonClicked?.Invoke(sender, e);
        }

        private void TalentIgnore_Click(object sender, RoutedEventArgs e)
        {
            OpenTalentSettingsClicked?.Invoke(sender, e);
        }

        private void NPCOverview_Click(object sender, RoutedEventArgs e)
        {
            OpenNpcOverviewClicked?.Invoke(this, e);
        }

        private void OnOpenJobDesigner(object sender, RoutedEventArgs e)
        {
            OpenJobDesignerClicked?.Invoke(this, e);
        }

        private void OnOpenSpeciesDesigner(object sender, RoutedEventArgs e)
        {
            OpenSpeciesDesignerClicked?.Invoke(this, e);
        }
    }
}
