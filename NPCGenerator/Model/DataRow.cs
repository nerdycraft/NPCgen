using System.Collections.Generic;

namespace NPCGenerator.Model
{
    public class DataRow
    {
        public string DisplayName { get; set; } // Row1, Row2, ...
        public dynamic Value { get; set; }
        public string DisplayValueAs { get; set; } // TextBox, CheckBox, ComboBox, Numeric
        public IEnumerable<object> ItemSource { get; set; }
        public bool NoGeneration { get; set; }
    }
}
