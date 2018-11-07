using System;
using System.Collections.Generic;
using System.Windows;

using NPCGenerator.Model;
using NPCGenerator.Util;

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
        public event EventHandler<Job> SaveClicked;
        public event EventHandler<Job> DeleteClicked;

        public JobDesigner(JobDesignerVM vm)
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
            SaveClicked?.Invoke(this, List.SelectedItem as Job);
        }

        private void OnDeleteJobClicked(object sender, RoutedEventArgs e)
        {
            DeleteClicked?.Invoke(this, List.SelectedItem as Job);
        }
    }
}
