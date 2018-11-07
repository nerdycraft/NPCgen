// ReSharper disable InconsistentNaming

using System.IO;

namespace NPCGenerator.Util
{
    public class References
    {
        public static string DATA_FILE { get; } = Path.Combine(Properties.Settings.Default.DataFolder, "general.json");
        public static string JOB_FOLDER { get; } = Path.Combine(Properties.Settings.Default.DataFolder, "jobs");
        public static string CULTURE_FOLDER { get; } = Path.Combine(Properties.Settings.Default.DataFolder, "cultures");
        public static string SPECIES_FOLDER { get; } = Path.Combine(Properties.Settings.Default.DataFolder, "species");

        public static string OUT_FOLDER { get; } = Path.Combine(Properties.Settings.Default.OutFolder, "{0}.json");
    }
}