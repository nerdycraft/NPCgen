using System.Windows;

using NPCGenerator.WindowModels;

namespace NPCGenerator.Windows
{
    public partial class SpeciesDesigner
    {
        public SpeciesDesigner(SpeciesDesignerWM wm)
        {
            InitializeComponent();
            DataContext = wm;
        }

        private void OnAddSpeciesClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeleteSpeciesClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnAddAttrModClicked(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeleteAttrModClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
