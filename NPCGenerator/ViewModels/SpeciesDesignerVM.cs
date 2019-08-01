using NPCGenerator.Dto;
using NPCGenerator.WindowModels;
using NPCGenerator.Windows;

namespace NPCGenerator.ViewModels
{
    public class SpeciesDesignerVM
    {
        public DataContainer Data { get; }

        private SpeciesDesigner sd;
        public SpeciesDesignerVM(DataContainer data) { Data = data; }

        public void Run()
        {
            sd = new SpeciesDesigner(new SpeciesDesignerWM { Species = Data.Species });
            sd.ShowDialog();
        }
    }
}