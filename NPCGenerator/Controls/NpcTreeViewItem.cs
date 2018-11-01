using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NPCGenerator.Controls
{
    public class NpcTreeViewItem : TreeViewItem
    {
        private readonly string filter;
        public NpcTreeViewItem(DirectoryInfo di, string fileFilter)
        {
            if (di == null)
                throw new ArgumentNullException(nameof(di));

            Header = di.Name;
            Tag = di;
            filter = fileFilter;
            AddDummy();

            Expanded += ItemExpanded;
        }

        public NpcTreeViewItem(FileSystemInfo fi)
        {
            Header = Path.GetFileNameWithoutExtension( fi.Name );
            Tag = fi;
        }

        public bool IsDirectoryNode => Tag is DirectoryInfo;

        public string FullPath => IsDirectoryNode ? ((DirectoryInfo)Tag).FullName : ((FileInfo)Tag).FullName;

        private void AddDummy()
        {
            Items.Add( new DummyTreeViewItem() );
        }

        public bool HasDummy => HasItems && Items.OfType<DummyTreeViewItem>().Any();

        private void RemoveDummy()
        {
            var dummies = Items.OfType<DummyTreeViewItem>().ToList();
            foreach ( var dummy in dummies )
                Items.Remove( dummy );
        }

        private void ExploreDirectory()
        {
            DirectoryInfo directoryInfo = null;
            switch (Tag)
            {
            case DirectoryInfo info:
                directoryInfo = info;
                break;
            case FileInfo fileInfo:
                directoryInfo = fileInfo.Directory;
                break;
            }

            if (directoryInfo == null) return;

            foreach ( var directory in directoryInfo.GetDirectories() )
            {
                var isHidden = (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                var isSystem = (directory.Attributes & FileAttributes.System) == FileAttributes.System;
                if ( !isHidden && !isSystem )
                    Items.Add( GetItem( directory ) );
            }

            foreach ( var file in directoryInfo.GetFiles(filter) )
            {
                var isHidden = (file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                var isSystem = (file.Attributes & FileAttributes.System) == FileAttributes.System;
                if ( !isHidden && !isSystem )
                    Items.Add( new NpcTreeViewItem( file ) );
            }
        }

        private NpcTreeViewItem GetItem(DirectoryInfo directory)
        {
            var item = new NpcTreeViewItem( directory, filter );
            item.AddDummy();
            item.Expanded += ItemExpanded;
            return item;
        }

        private void ItemExpanded(object sender, RoutedEventArgs e)
        {
            if (!HasDummy) return;

            Cursor = Cursors.Wait;
            RemoveDummy();
            ExploreDirectory();
            Cursor = Cursors.Arrow;
        }
    }

    public class DummyTreeViewItem : TreeViewItem
    {
        public DummyTreeViewItem()
        {
            Header = "Dummy";
            Tag = "Dummy";
        }
    }
}
