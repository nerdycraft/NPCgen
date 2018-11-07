using System.Collections.Generic;
using System.ComponentModel;

using NPCGenerator.Model;

namespace NPCGenerator.Util
{
    public class MainVM : INotifyPropertyChanged
    {
        public MainVM(IEnumerable<DataRow> rows) { DataRows = rows; }

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