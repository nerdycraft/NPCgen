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
            LayoutRoot.DataContext = this;
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly DependencyProperty LabelProperty =
                    DependencyProperty.Register("Label",
                                                typeof(string),
                                                typeof(LabelTextBox),
                                                new FrameworkPropertyMetadata("Label"));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
             DependencyProperty.Register("Text",
                         typeof(string),
                         typeof(LabelTextBox),
                         new FrameworkPropertyMetadata(string.Empty));

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
                        new PropertyMetadata(false));

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
                        new PropertyMetadata(HorizontalAlignment.Left));


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
                        new PropertyMetadata(VerticalAlignment.Top));
    }
}
