using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NPCGenerator.Controls
{
    public class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            CommandManager.AddPreviewCanExecuteHandler( this, CanExecuteRoutedEventHandler );
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric( e.Text );
        }

        protected void CanExecuteRoutedEventHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if ( e.Command == ApplicationCommands.Paste )
            {
                e.Handled = !IsNumeric( Clipboard.GetText() );
            }
        }

        private static bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }
    }
}
