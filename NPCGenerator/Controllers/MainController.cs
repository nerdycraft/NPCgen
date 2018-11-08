using Newtonsoft.Json;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using NPCGenerator.Model;
using NPCGenerator.Util;
using NPCGenerator.ViewModels;
using NPCGenerator.Windows;

namespace NPCGenerator.Controllers
{
    public class MainController
    {
        private MainWindow wnd;
        private MainVM vm;

        public DataContainer Data { get; private set; }

        private readonly ObservableCollection<DataRow> dataRows = new ObservableCollection<DataRow>();

        private Generator generator;

        public void Run()
        {
            if (wnd == null)
            {
                Directory.CreateDirectory(References.OUT_FOLDER);
                vm = new MainVM(dataRows);
                wnd = new MainWindow(vm);
                wnd.OpenJobDesignerClicked += delegate { new JobDesignerController(Data).Run(); };
                wnd.OpenNpcOverviewClicked += delegate { new NpcOverviewController().Run(); };
                wnd.Closing += delegate { SaveSettings(); };
                wnd.GenerateClicked += delegate { Generate(); };
                wnd.ReloadJsonClicked += delegate { Init(); };
            }

            wnd.Show();
            Init();
        }
        private void Init()
        {
            try
            {
                dataRows.Clear();
                LoadJsonData();

                generator.RowXp = CreateDataRow("Erfahrung", "ComboBox", true, Data.Levels);
                generator.RowFirstName = CreateDataRow("Vorname", "TextBox");
                generator.RowLastName = CreateDataRow("Nachname", "TextBox");
                generator.RowAge = CreateDataRow("Alter", "Numeric");
                generator.RowGender = CreateDataRow("Geschlecht", "ComboBox", false, Data.Gender);
                generator.RowSpecies = CreateDataRow("Spezies", "ComboBox", false, Data.Species);
                generator.RowCulture = CreateDataRow("Kultur", "ComboBox", false, Data.Cultures);
                generator.RowStature = CreateDataRow("Statur", "ComboBox", false, Data.Statures);
                generator.RowSize = CreateDataRow("Größe", "ComboBox", false, Data.Sizes);
                generator.RowCharacter = CreateDataRow("Charakter", "TextBox", true);
                generator.RowJob = CreateDataRow("Beruf", "ComboBox", true, Data.Jobs);

                vm.StatusText = "Data successfully loaded.";

                System.Media.SystemSounds.Beep.Play();
            }
            catch (JsonLoadException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DataRow CreateDataRow(string displayName, string displayValueAs, bool noGen = false,
                    IEnumerable<object> itemSource = null)
        {
            var dr = new DataRow
                     {
                         DisplayName = displayName, DisplayValueAs = displayValueAs,
                         ItemSource = itemSource, NoGeneration = noGen
                     };
            dataRows.Add(dr);
            return dr;
        }

        private void LoadJsonData()
        {
            generator = null;
            Data = DeserializeHandler<DataContainer>(References.DATA_FILE);

            Data.Species = Directory.GetFiles(References.SPECIES_FOLDER, "*.json").Select(DeserializeHandler<Species>).ToList();
            Data.Jobs = new ObservableCollection<Job>(Directory.GetFiles(References.JOB_FOLDER, "*.json").Select(DeserializeHandler<Job>));
            Data.Cultures = Directory.GetFiles(References.CULTURE_FOLDER, "*.json").Select(DeserializeHandler<Culture>).ToList();

            foreach (var job in Data.Jobs)
                foreach (var talent in job.Talents)
                {
                    var refTalent = Data.Talents.First(t => talent.Id == t.Id);
                    talent.Attr = refTalent.Attr;
                    talent.Category = refTalent.Category;
                }

            CheckJobTalentWeight();

            generator = new Generator(Data);
            generator.UpdateStatus += (sender, s) => vm.StatusText = s;
        }

        public void CheckJobTalentWeight()
        {
            var output = new System.Text.StringBuilder();
            foreach (var job in Data.Jobs)
            {
                var totalWeight = job.Talents.Sum(talent => talent.Weight);
                if (totalWeight > 800)
                    output.AppendLine($"{job.ReferenceName} - Hat {totalWeight} von 800 Gewichtungspunkte verwendet");
            }

            if (output.Length > 0)
                throw new JsonLoadException(output.ToString());
        }

        public void Generate()
        {
            if (generator == null)
            {
                MessageBox.Show("Fix and reload json!");
                return;
            }

            var npc = generator.Generate();
            var path = Path.Combine(References.OUT_FOLDER, $"{npc}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(npc));
        }

        public void SaveSettings() { File.WriteAllText(References.DATA_FILE, JsonConvert.SerializeObject(Data)); }

        private static T DeserializeHandler<T>(string filePath)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch (JsonSerializationException ex)
            {
                throw new JsonLoadException(ex.Message.Replace("\'\'", filePath), ex);
            }
        }
    }
}