using System.Collections.Generic;
using System.ComponentModel;

using NPCGenerator.Dto;

namespace NPCGenerator.WindowModels
{
    public class MainWM : INotifyPropertyChanged
    {
        public MainWM(IEnumerable<DataRow> rows) { DataRows = rows; }

        public IEnumerable<DataRow> DataRows { get; }

        private string statusText;
        public string StatusText
        {
            get => statusText;
            set
            {
                statusText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusText"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}