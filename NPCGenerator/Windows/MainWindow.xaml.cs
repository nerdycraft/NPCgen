using NPCGenerator.Util;
using System.Windows;
using System.Windows.Controls;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class MainWindow
    {
        private readonly Controller controller;
        public MainWindow(Controller trolly)
        {
            InitializeComponent();

            controller = trolly;

            DataContext = controller;
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
            if (!controller.HasJsonErrors)
                controller.Generate();
            else
                MessageBox.Show("Fix and reload json!");
        }

        private void ReloadJson_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                controller.Init();

                System.Media.SystemSounds.Beep.Play();
            }
            catch (JsonLoadException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckTalentWeight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(controller.CheckJobTalentWeight());
        }

        private void TalentIgnore_Click(object sender, RoutedEventArgs e)
        {
            new TalentSetting(controller.Data).ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.SaveSettings();
        }

        private void NPCOverview_Click(object sender, RoutedEventArgs e)
        {
            new NpcOverview().ShowDialog();
        }

        private void OnOpenJobDesigner(object sender, RoutedEventArgs e)
        {
            new JobDesigner(controller.Data.Jobs, controller.Data.Talents).ShowDialog();
        }
    }
}
