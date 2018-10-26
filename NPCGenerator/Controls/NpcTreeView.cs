using NPCGenerator.Util;
using System;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NPCGenerator.Controls
{
    public class NpcTreeView : TreeView
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            foreach (var item in e.NewItems.OfType<NpcTreeViewItem>())
            {
                item.MouseDoubleClick += Item_MouseDoubleClick;
                item.PreviewMouseMove += Item_PreviewMouseMove;
                item.PreviewMouseDown += Item_PreviewMouseDown;
                item.DragOver += Item_DragOver;
                item.Drop += Item_Drop;
            }
        }

        public event EventHandler<NpcTreeViewItemDoubleClickedEventArgs> ItemDoubleClicked;
        private void Item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            if (SelectedItem is NpcTreeViewItem item)
                OnItemDoubleClicked(new NpcTreeViewItemDoubleClickedEventArgs(item));
        }

        protected void OnItemDoubleClicked(NpcTreeViewItemDoubleClickedEventArgs e)
        {
            ItemDoubleClicked?.Invoke(this, e);
        }

        private Point startPos;
        private NpcTreeViewItem draggedItem, target;

        private void Item_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            startPos = e.GetPosition(this);
        }

        private void Item_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var pos = e.GetPosition(this);

            if (Math.Abs(pos.X - startPos.X) > 10 || Math.Abs(pos.Y - startPos.Y) > 10)
                StartDrag(e);
        }

        private void Item_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                var currentPosition = e.GetPosition(this);

                if (Math.Abs(currentPosition.X - startPos.X) > 10 ||
                    Math.Abs(currentPosition.Y - startPos.Y) > 10)
                {
                    // Verify that this is a valid drop and then store the drop target
                    var item = GetNearestContainer(e.OriginalSource as UIElement);
                    e.Effects = CheckDropTarget(draggedItem, item) ? DragDropEffects.Move : DragDropEffects.None;
                }
                e.Handled = true;
            }
            catch (Exception)
            {
                //TODO error handling
            }
        }

        private void Item_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                var targetItem = GetNearestContainer(e.OriginalSource as UIElement);
                if (targetItem != null && draggedItem != null)
                {
                    target = targetItem;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
                //TODO error handling
            }
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void StartDrag(MouseEventArgs e)
        {
            draggedItem = (NpcTreeViewItem)SelectedItem;
            if (draggedItem != null && !draggedItem.IsDirectoryNode)
            {
                var finalDropEffect = DragDrop.DoDragDrop(this, SelectedValue, DragDropEffects.Move);
                //Checking target is not null and item is
                if (finalDropEffect == DragDropEffects.Move && target != null)
                {
                    // A Move drop was accepted
                    if (!draggedItem.Header.ToString().Equals(target.Header.ToString()))
                    {
                        CopyItem(draggedItem, target);
                        target = null;
                        draggedItem = null;
                    }
                }

            }
        }

        private void CopyItem(NpcTreeViewItem sourceItem, NpcTreeViewItem targetItem)
        {
            //Asking user wether he want to drop the dragged TreeViewItem here or not
            if (MessageBox.Show("Would you like to drop " + sourceItem.Header + " into " + targetItem.Header + "", "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            try
            {
                //adding dragged TreeViewItem in target TreeViewItem
                AddChild(sourceItem, targetItem);

                //finding Parent TreeViewItem of dragged TreeViewItem 
                var parentItem = FindVisualParent<NpcTreeViewItem>(sourceItem);
                // if parent is null then remove from TreeView else remove from Parent TreeViewItem
                if (parentItem == null)
                    Items.Remove(sourceItem);
                else
                    parentItem.Items.Remove(sourceItem);
            }
            catch
            {
                // ignored
            }
        }

        private static bool CheckDropTarget(NpcTreeViewItem sourceItem, NpcTreeViewItem targetItem)
        {
            //Check whether the target item is meeting your condition
            return targetItem.IsDirectoryNode && !sourceItem.IsDirectoryNode;
        }

        private static NpcTreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            var container = element as NpcTreeViewItem;
            while (container == null && element != null)
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as NpcTreeViewItem;
            }
            return container;
        }

        private static void AddChild(NpcTreeViewItem sourceItem, NpcTreeViewItem targetItem)
        {
            if (sourceItem == null) throw new ArgumentNullException(nameof(sourceItem));

            // add item in target TreeViewItem 
            if (sourceItem.Tag is FileInfo sourceFile)
            {
                sourceFile.MoveTo(Path.Combine(((DirectoryInfo)targetItem.Tag).FullName, sourceFile.Name));

                if (!targetItem.HasDummy)
                    targetItem.Items.Add(new NpcTreeViewItem(sourceFile));
            }
        }

        private static TObject FindVisualParent<TObject>(DependencyObject child) where TObject : UIElement
        {
            if (child == null)
                return null;

            var parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                if (parent is TObject found)
                    return found;

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }
    }
}
