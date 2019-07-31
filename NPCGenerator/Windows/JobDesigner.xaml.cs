using System;
using System.Windows;

using NPCGenerator.Dto;
using NPCGenerator.WindowModels;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class JobDesigner
    {
        public delegate Job AddJobClicked();
        public event AddJobClicked AddClicked;
        public event EventHandler SaveClicked;
        public event EventHandler DeleteClicked;
        public event EventHandler AddTalentWeight;
        public event EventHandler DeleteTalentWeight;

        public JobDesigner(JobDesignerWM vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void OnAddJobClick(object sender, RoutedEventArgs e)
        {
            List.SelectedItem = AddClicked?.Invoke();
        }

        private void OnSaveJobClicked(object sender, RoutedEventArgs e)
        {
            SaveClicked?.Invoke(this, e);
        }

        private void OnDeleteJobClicked(object sender, RoutedEventArgs e)
        {
            DeleteClicked?.Invoke(this, e);
        }

        private void OnAddTalentWeight(object sender, RoutedEventArgs e)
        {
            AddTalentWeight?.Invoke(sender, e);
        }

        private void OnDeleteTalentWeightClick(object sender, RoutedEventArgs e)
        {
            DeleteTalentWeight?.Invoke(sender, e);
        }
    }
}
