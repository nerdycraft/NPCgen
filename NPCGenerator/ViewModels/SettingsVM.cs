using NPCGenerator.Dto;
using NPCGenerator.WindowModels;
using NPCGenerator.Windows;

namespace NPCGenerator.ViewModels
{
    public class SettingsVM
    {
        public DataContainer Data { get; }

        public SettingsVM(DataContainer data) { Data = data; }


        public void Run()
        {
            var settings = new TalentSetting( new SettingsWM { Talents = Data.Talents });
            settings.ShowDialog();
        }
    }
}