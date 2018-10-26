using Newtonsoft.Json;
using NPCGenerator.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NPCGenerator.Util
{
    // ReSharper disable once InconsistentNaming
    public class NpcOverviewVM
    {        
        private readonly ObservableCollection<NPC> npc = new ObservableCollection<NPC>();
        public string NpcPath => "output";
        public IEnumerable<NPC> NPCs => npc;

        public JsonDataContainer Data { get; set; }        

        public void LoadNpcFromFile(string path)
        {
            var npc = JsonConvert.DeserializeObject<NPC>( File.ReadAllText( path ) );
            if ( this.npc.Any( n => n.ToString() == npc.ToString() ) )
            {
                this.npc.First( n => n.ToString() == npc.ToString() ).IsSelected = true;
                return;
            }

            this.npc.Add( npc );
            npc.IsSelected = true;
        }

        public bool IsValidFolderName(DirectoryInfo parentDi, string folderName)
        {
            return folderName != string.Empty && !Directory.Exists( Path.Combine( parentDi.FullName, folderName ) ) && !Path.GetInvalidFileNameChars().Where( x => folderName.Contains( x ) ).Any() && !forbidden.Where( x => folderName.ToLower() == x.ToLower() ).Any();
        }
        private readonly string[] forbidden = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };

        public void RemoveNpc(object npc)
        {
            if ( npc is NPC n )
                this.npc.Remove( n );
        }
    }
}
