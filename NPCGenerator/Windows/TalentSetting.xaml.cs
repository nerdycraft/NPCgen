using NPCGenerator.WindowModels;

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
        public TalentSetting(SettingsWM data)
        {
            InitializeComponent();
            DataContext = data;
        }
    }
}
