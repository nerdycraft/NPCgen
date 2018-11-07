using System;
using System.Windows;

using NPCGenerator.Util;

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
                new Controller().Run();
            }
            catch ( Exception ex)
            {
                MessageBox.Show( ex.ToString() );
            }
        }
    }
}
