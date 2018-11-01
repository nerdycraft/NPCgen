using NPCGenerator.Model;

namespace NPCGenerator.Windows
{
    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    /// <summary>
    /// Nothing to see here, move along!
    /// </summary>
    public partial class TalentSetting
    {
        public TalentSetting(JsonDataContainer data)
        {
            InitializeComponent();
            DataContext = data;
        }
    }
}
