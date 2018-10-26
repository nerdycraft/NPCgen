using NPCGenerator.Util;
using NPCGenerator.Windows;
using System;
using System.Windows;

namespace NPCGenerator
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public partial class App
    {
        private readonly Controller controller = new Controller();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                System.IO.Directory.CreateDirectory( "output" );
                try
                {
                    controller.Init();
                }
                catch ( JsonLoadException ex )
                {
                    MessageBox.Show( ex.Message );
                }

                // Create the startup window
                var wnd = new MainWindow( controller );
                // Show the window
                wnd.Show();
            }
            catch ( Exception ex)
            {
                MessageBox.Show( ex.ToString() );
            }
        }
    }
}
