using Newtonsoft.Json;

using NPCGenerator.Model;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

// ReSharper disable PossibleMultipleEnumeration

namespace NPCGenerator.Util
{
    public class Controller : INotifyPropertyChanged
    {
        private const int BUILD_VERSION = 1;

        private const string STATURES_FILE = @"data\statures.json";
        private const string SPECIES_FILE = @"data\species.json";
        private const string SIZES_FILE = @"data\sizes.json";
        private const string XP_FILE = @"data\xp.json";

        private const string SPECIES_FOLDER = @"data\species";
        private const string CULTURES_FOLDER = @"data\cultures";
        private const string JOB_FOLDER = @"data\jobs";

        public const string OUT_PATH = @"output\{0}.json";

        private const string SETTINGS_FILE = @"data\settings.json";

        private readonly Random rnd = new Random();

        public JsonDataContainer Data { get; } = new JsonDataContainer();

        private readonly ObservableCollection<DataRow> dataRows = new ObservableCollection<DataRow>();
        public IEnumerable<DataRow> DataRows => dataRows;
        public Settings Settings { get; private set; }

        public bool HasJsonErrors { get; private set; }
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

        private DataRow rowXp,
                    rowFirstName,
                    rowLastName,
                    rowAge,
                    rowGender,
                    rowSpecies,
                    rowCulture,
                    rowStature,
                    rowSize,
                    rowCharacter,
                    rowJob;

        public void Init()
        {
            dataRows.Clear();
            LoadJsonData();

            rowXp = CreateDataRow("Erfahrung", "ComboBox", true, Data.XpLevels);
            rowFirstName = CreateDataRow("Vorname", "TextBox");
            rowLastName = CreateDataRow("Nachname", "TextBox");
            rowAge = CreateDataRow("Alter", "Numeric");
            rowGender = CreateDataRow("Geschlecht", "ComboBox", false, Data.Gender);
            rowSpecies = CreateDataRow("Spezies", "ComboBox", false, Data.Species);
            rowCulture = CreateDataRow("Kultur", "ComboBox", false, Data.Cultures);
            rowStature = CreateDataRow("Statur", "ComboBox", false, Data.Statures);
            rowSize = CreateDataRow("Größe", "ComboBox", false, Data.Sizes);
            rowCharacter = CreateDataRow("Charakter", "TextBox", true);
            rowJob = CreateDataRow("Beruf", "ComboBox", true, Data.Jobs);

            StatusText = "Data successfully loaded.";
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
            Data.Species = DeserializeHandler<SpeciesFile>(SPECIES_FILE).Species;
            Data.Statures = DeserializeHandler<StringData>(STATURES_FILE).Data;
            Data.Sizes = DeserializeHandler<StringData>(SIZES_FILE).Data;
            Data.XpLevels = DeserializeHandler<XPFile>(XP_FILE).Levels;

            //Data.Species = Directory.GetFiles(SPECIES_FILE, "*.json").Select(DeserializeHandler<Species>);
            Data.Jobs = Directory.GetFiles(JOB_FOLDER, "*.json").Select(DeserializeHandler<Job>);
            Data.Cultures = Directory.GetFiles(CULTURES_FOLDER, "*.json").Select(DeserializeHandler<Culture>);

            Data.Gender = new[] {"männlich", "weiblich", "-"};

            Settings = DeserializeHandler<Settings>(SETTINGS_FILE);

            HasJsonErrors = false;
        }

        public string CheckJobTalentWeight()
        {
            var output = new System.Text.StringBuilder();
            foreach (var job in Data.Jobs)
            {
                var totalWeight = job.Talents.Sum(talent => (int) talent.Weight);

                output.AppendLine($"{job.Name} - Hat {totalWeight} von 80 Gewichtungspunkte verwendet");
            }

            return output.ToString();
        }

        public void Generate()
        {
            StatusText = "Erstelle NPC!";
            var npc = new NPC
                      {
                          Build = BUILD_VERSION,

                          Age = rowAge.NoGeneration ? rowAge.Value : (uint) rnd.Next(12, 100),
                          Gender = rowGender.NoGeneration ? rowGender.Value : RandomFromList(Data.Gender),
                          Size = rowSize.NoGeneration ? rowSize.Value : RandomFromList(Data.Sizes),
                          Stature = rowStature.NoGeneration ? rowStature.Value : RandomFromList(Data.Statures),
                          Charakter = rowCharacter.Value
                      };

            var isFemale = npc.Gender == "weiblich";

            Level level = rowXp.Value;
            npc.Level = level.Name;

            Species species = rowSpecies.NoGeneration ? rowSpecies.Value : RandomFromList(Data.Species);
            npc.Species = species.Name;

            var culture = GetCulture(species);
            npc.Culture = culture.Name;

            var job = GetJob(culture);
            npc.Job = isFemale ? job.FemName : job.Name;

            npc.Name = GetName(isFemale, culture);

            npc.Talents = CalculateTalents(level, MergeBaseTalents(culture.Talents, job.Talents)).ToArray();

            npc.Attributes = CalculateAttr(level, job.Statweight, species.Mod);
            npc.Stats = CalculateStats(species, npc.Attributes);

            File.WriteAllText(string.Format(OUT_PATH, npc), JsonConvert.SerializeObject(npc));
            StatusText = $"NPC Erstellt: {npc}";
        }

        public void SaveSettings() { File.WriteAllText(SETTINGS_FILE, JsonConvert.SerializeObject(Settings)); }

        private Attributes CalculateAttr(Level level, Statweight weight, AttrMod mod)
        {
            if (weight.Kk != 100)
                throw new GenerationException("Ich habe gesagt es muss 100 ergeben!!!!");

            var attr = new Attributes();

            var rndAttr = RandomAttrMod(mod);

            var pointsSpent = 0;
            while (pointsSpent < level.Attr)
            {
                var w = rnd.Next(1, 100);
                if (w <= weight.Mu && attr.Mu < GetMaxAttr("MU", level.MaxAttr, mod, rndAttr))
                    attr.Mu++;
                else if (w <= weight.Kl && attr.Kl < GetMaxAttr("KL", level.MaxAttr, mod, rndAttr))
                    attr.Kl++;
                else if (w <= weight.In && attr.In < GetMaxAttr("IN", level.MaxAttr, mod, rndAttr))
                    attr.In++;
                else if (w <= weight.Ch && attr.Ch < GetMaxAttr("CH", level.MaxAttr, mod, rndAttr))
                    attr.Ch++;
                else if (w <= weight.Ff && attr.Ff < GetMaxAttr("FF", level.MaxAttr, mod, rndAttr))
                    attr.Ff++;
                else if (w <= weight.Ge && attr.Ge < GetMaxAttr("GE", level.MaxAttr, mod, rndAttr))
                    attr.Ge++;
                else if (w <= weight.Ko && attr.Ko < GetMaxAttr("KO", level.MaxAttr, mod, rndAttr))
                    attr.Ko++;
                else if (w <= weight.Kk && attr.Kk < GetMaxAttr("KK", level.MaxAttr, mod, rndAttr))
                    attr.Kk++;
                else
                    continue;
                pointsSpent++;
            }

            return attr;
        }

        private static Stats CalculateStats(Species species, Attributes attr)
        {
            var stats = new Stats
                        {
                            Lp = species.BaseHp + 2 * attr.Ko,
                            Sk = species.BaseSk + (int) Math.Floor((attr.Mu + attr.Kl + attr.In) / 6d),
                            Zk = species.BaseZk + (int) Math.Floor((attr.Ko + attr.Ko + attr.Kk) / 6d),
                            Aw = attr.Ge / 2,
                            Ini = (attr.Mu + attr.Ge) / 2,
                            Gs = species.BaseGs
                        };

            return stats;
        }

        private IEnumerable<AttrTalent> CalculateTalents(Level level, IEnumerable<WeightedTalent> talents)
        {
            var talentWeight = new Dictionary<uint, WeightedTalent>();
            var noWeightTalents = 0;
            uint totalWeight = 0;
            foreach (var talent in talents)
            {
                if (talent.Weight > 0)
                {
                    totalWeight += talent.Weight;
                    talentWeight.Add(totalWeight, talent);
                }
                else
                    noWeightTalents++;
            }

            if (totalWeight > 80)
                throw new GenerationException("Job Talent weight should be max 80!");

            var newTalentWeight = (uint) Math.Ceiling((100f - totalWeight) / noWeightTalents);
            foreach (var talent in talents.Where(t => t.Weight == 0))
            {
                totalWeight += newTalentWeight;
                talentWeight.Add(totalWeight, talent);
            }

            var sorted = from pair in talentWeight orderby pair.Key descending select pair;

            //punkte verteilen
            var pointsSpent = 0;
            while (pointsSpent < level.Fw)
            {
                var rndWeight = rnd.Next(1, (int) totalWeight);
                foreach (var kvp in sorted)
                    if (kvp.Key <= rndWeight)
                    {
                        if (kvp.Value.Value >= level.MaxFw)
                            break;

                        kvp.Value.Value++;
                        pointsSpent++;
                    }
            }

            var list = new List<AttrTalent>();
            foreach (var talent in talentWeight.Values)
            {
                var attrT = Settings.Talents.FirstOrDefault(t => t.Name == talent.Name);
                if (attrT != null)
                    list.Add(new AttrTalent
                             {
                                 Name = talent.Name,
                                 Value = talent.Value,
                                 Attr1 = attrT.Attr1,
                                 Attr2 = attrT.Attr2,
                                 Attr3 = attrT.Attr3
                             });
            }

            return list;
        }

        private T RandomFromList<T>(IEnumerable<T> lst) { return lst.ElementAt(rnd.Next(0, lst.Count())); }

        private static long GetMaxAttr(string attr, long defaultMax, AttrMod mod, string rndAttr)
        {
            if (mod.Rnd.HasValue && rndAttr == attr)
                return defaultMax + 1;
            if (mod.And != null && mod.And.Stats.Any(s => s == attr))
                return defaultMax + mod.And.Value;
            if (mod.Or != null && rndAttr == attr)
                return defaultMax + mod.Or.Value;
            return defaultMax;
        }

        private static string RandomAttrMod(AttrMod mod)
        {
            if (!mod.Rnd.HasValue)
                return mod.Or.Stats[new Random().Next(0, 1)];
            var attrLst = new[] { "MU", "KL", "IN", "CH", "FF", "GE", "KO", "KK" };
            return attrLst[new Random().Next(0, 7)];
        }

        private IEnumerable<WeightedTalent> MergeBaseTalents(IEnumerable<Talent> cultTalents, IEnumerable<WeightedTalent> jobTalents)
        {
            var talents = cultTalents.Select(t => new WeightedTalent
                                    { Name = t.Name, Value = t.Value, Weight = 0 }).ToList();

            foreach (var talent in jobTalents)
            {
                var tal = talents.FirstOrDefault(t => t.Name == talent.Name);
                if (tal != null)
                {
                    tal.Value += talent.Value;
                    tal.Weight = talent.Weight;
                }
                else
                    talents.Add(talent);
            }

            return talents.Where(t => !Settings.Talents.Any(it => it.Name == t.Name && it.Ignore));
        }


        private Culture GetCulture(Species species)
        {
            Culture culture;
            if (rowCulture.NoGeneration)
                culture = rowCulture.Value;
            else if (!Settings.UseDefaultGeneration)
                culture = RandomFromList(Data.Cultures);
            else
            {
                var defaultCults = Data.Cultures.Where(c => c.DefaultSpecies.Contains(species.Name));
                if (!defaultCults.Any())
                    throw new GenerationException("there is no culture with the species " + species.Name);
                culture = RandomFromList(defaultCults);
            }

            return culture;
        }

        private Job GetJob(Culture culture)
        {
            Job job;
            if (rowJob.NoGeneration)
                job = rowJob.Value;
            else if (!Settings.UseDefaultGeneration)
                job = RandomFromList(Data.Jobs);
            else
                job = RandomFromList(Data.Jobs.Where(j => culture.DefaultJobs.Contains(j.ReferenceName)));
            return job;
        }

        private string GetName(bool isFemale, Culture culture)
        {
            string firstName,
                        lastName;

            if (rowFirstName.NoGeneration)
                firstName = rowFirstName.Value;
            else
            {
                var format = isFemale ? culture.Names.FormatFemale : culture.Names.FormatMale;
                if (string.IsNullOrEmpty(format))
                    firstName = isFemale ? culture.Names.Female[rnd.Next(0, culture.Names.Female.Length - 1)]
                                            : culture.Names.Male[rnd.Next(0, culture.Names.Male.Length - 1)];
                else
                    firstName = ResolveNameFormat(format, culture.Names);
            }

            if (rowLastName.NoGeneration)
                lastName = rowLastName.Value;
            else
            {
                var format = isFemale ? culture.Names.FormatFemaleLast : culture.Names.FormatMaleLast;
                if (string.IsNullOrEmpty(format))
                    lastName = culture.Names.Last.Any() ? 
                                           culture.Names.Last[rnd.Next(0, culture.Names.Last.Length - 1)]
                                           : string.Empty;
                else
                    lastName = ResolveNameFormat(format, culture.Names);
            }

            return $"{firstName} {lastName}".Trim();
        }

        private string ResolveNameFormat(string format, Names names)
        {
            var ret = format;
            if (names.Male != null && names.Male.Any())
                ret = ret.Replace("{male}", names.Male[rnd.Next(0, names.Male.Count() - 1)]);
            if (names.Female != null && names.Female.Any())
                ret = ret.Replace("{female}", names.Female[rnd.Next(0, names.Female.Count() - 1)]);
            if (names.Last != null && names.Last.Any())
                ret = ret.Replace("{last}", names.Last[rnd.Next(0, names.Last.Count() - 1)]);
            if (names.Suffix != null && names.Suffix.Any())
                ret = ret.Replace("{suffix}", names.Suffix[rnd.Next(0, names.Suffix.Count() - 1)]);
            return ret;
        }

        private T DeserializeHandler<T>(string filePath)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch (JsonSerializationException ex)
            {
                HasJsonErrors = true;
                throw new JsonLoadException(ex.Message.Replace("\'\'", filePath), ex);
            }
        }
    }
}