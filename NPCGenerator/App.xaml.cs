using System;
using System.Windows;

using NPCGenerator.ViewModels;

namespace NPCGenerator
{
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // Create the startup window
                new MainVM().Run();
            }
            catch ( Exception ex)
            {
                MessageBox.Show( ex.ToString() );
            }
        }
    }
}
