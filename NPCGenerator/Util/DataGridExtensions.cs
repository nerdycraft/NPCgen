using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace NPCGenerator.Util
{
    public static class DataGridExtensions
    {
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            var child = default( T );
            var numVisuals = VisualTreeHelper.GetChildrenCount( parent );
            for ( var i = 0; i < numVisuals; i++ )
            {
                var v = (Visual)VisualTreeHelper.GetChild( parent, i );
                child = v as T ?? GetVisualChild<T>( v );
                if ( child != null )
                    break;
            }
            return child;
        }

        public static DataGridRow GetRow(this DataGrid grid, int index)
        {
            var row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex( index );
            if (row != null) return row;

            // May be virtualized, bring into view and try again.
            grid.UpdateLayout();
            grid.ScrollIntoView( grid.Items[index] );
            return (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
        }

        public static DataGridCell GetCell(this DataGrid grid, DataGridRow row, int column)
        {
            if (row == null) return null;

            var presenter = GetVisualChild<DataGridCellsPresenter>( row );

            if ( presenter == null )
            {
                grid.ScrollIntoView( row, grid.Columns[column] );
                presenter = GetVisualChild<DataGridCellsPresenter>( row );
            }

            return (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
        }

        public static DataGridCell GetCell(this DataGrid grid, int row, int column)
        {
            var gridRow = GetRow( grid, row );
            return GetCell( grid, gridRow, column );
        }
    }
}
