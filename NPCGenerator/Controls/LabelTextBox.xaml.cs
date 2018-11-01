using System.Windows;
// ReSharper disable InconsistentNaming

namespace NPCGenerator.Controls
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// </summary>
    public partial class LabelTextBox
    {
        public LabelTextBox()
        {
            InitializeComponent();
        }

        private string localLabel = string.Empty;

        public string Label
        {
            get => localLabel;
            set
            {
                localLabel = value;
                BaseLabel.Content = value;
            }
        }

        public string TextBox
        {
            get => (string)GetValue(TextBoxProperty);
            set => SetValue(TextBoxProperty, value);
        }

        public static readonly DependencyProperty TextBoxProperty =
             DependencyProperty.Register("TextBox",
                         typeof(string),
                         typeof(LabelTextBox),
                         new FrameworkPropertyMetadata(string.Empty, OnTextBoxPropertyChanged));

        private static void OnTextBoxPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is LabelTextBox control) control.BaseTextBox.Text = (string)e.NewValue;
        }

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly",
                        typeof(bool),
                        typeof(LabelTextBox),
                        new PropertyMetadata(false, OnIsReadOnlyPropertyChanged));

        private static void OnIsReadOnlyPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is LabelTextBox control) control.BaseTextBox.IsReadOnly = (bool)e.NewValue;
        }

        public HorizontalAlignment TextHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(TextHorizontalAlignmentProperty);
            set => SetValue(TextHorizontalAlignmentProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextHorizontalAlignmentProperty =
            DependencyProperty.Register("TextHorizontalAlignment",
                        typeof(HorizontalAlignment),
                        typeof(LabelTextBox),
                        new PropertyMetadata(HorizontalAlignment.Left, OnTextHorizontalAlignmentPropertyChanged));

        private static void OnTextHorizontalAlignmentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is LabelTextBox control) control.BaseTextBox.HorizontalContentAlignment = (HorizontalAlignment)e.NewValue;
        }


        public VerticalAlignment TextVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(TextVerticalAlignmentProperty);
            set => SetValue(TextVerticalAlignmentProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextVerticalAlignmentProperty =
            DependencyProperty.Register("TextVerticalAlignment",
                        typeof(VerticalAlignment),
                        typeof(LabelTextBox),
                        new PropertyMetadata(VerticalAlignment.Top, OnTextVerticalAlignmentPropertyChanged));

        private static void OnTextVerticalAlignmentPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            if (source is LabelTextBox control) control.BaseTextBox.VerticalContentAlignment = (VerticalAlignment)e.NewValue;
        }
    }
}
