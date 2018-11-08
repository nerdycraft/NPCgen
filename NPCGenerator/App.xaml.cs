using System;
using System.Windows;

using NPCGenerator.Controllers;

namespace NPCGenerator
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                // Create the startup window
                new MainController().Run();
            }
            catch ( Exception ex)
            {
                MessageBox.Show( ex.ToString() );
            }
        }
    }
}
