using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Path = System.Windows.Shapes.Path;

namespace RecipeSelectHelper.Resources
{
    public class ExtendedListView : ListView
    {
        public ExtendedListView()
        {
            Loaded += ListViewWithHeaderSort_Loaded;
            SizeChanged += ListViewWithHeaderSort_SizeChanged;
            this.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(GridViewColumnHeaderClick));
        }

        private void ListViewWithHeaderSort_Loaded(object sender, RoutedEventArgs e)
        {
            IncreaseAllColumnSizes(20); //To make room for the ascending/descending arrow
            FillingHeader = 0;
            HeaderFillRemainingSpace();
        }

        private void IncreaseAllColumnSizes(int pixels)
        {
            var gridView = this.View as GridView;
            if(gridView == null) return;

            foreach (GridViewColumn column in gridView.Columns)
            {
                column.Width = column.ActualWidth + pixels;
            }
        }

        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;
        //private bool _resourcesNotLoaded = true;

        private void GridViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            //if (_resourcesNotLoaded) LoadResources();

            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            if (column == null) return;

            if (_sortColumn == column)
            {
                // Toggle sorting direction 
                _sortDirection = _sortDirection == ListSortDirection.Ascending ?
                                                   ListSortDirection.Descending :
                                                   ListSortDirection.Ascending;
            }
            else
            {
                // Remove arrow from previously sorted header 
                if (_sortColumn != null)
                {
                    _sortColumn.Column.HeaderTemplate = null;
                    //_sortColumn.Column.Width = _sortColumn.ActualWidth - 20;
                }

                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
                //column.Column.Width = column.ActualWidth + 20;
            }

            if (_sortDirection == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate = Application.Current.Resources["ArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate = Application.Current.Resources["ArrowDown"] as DataTemplate;
            }

            string header = string.Empty;

            // if binding is used and property name doesn't match header content 
            Binding b = _sortColumn.Column.DisplayMemberBinding as Binding;
            if (b != null)
            {
                header = b.Path.Path;
            }

            // NULLREFERENCEEXCEPTION HERE? TRY FRIDGEPAGE!
            ICollectionView resultDataView = CollectionViewSource.GetDefaultView(ItemsSource);
            resultDataView.SortDescriptions.Clear();
            resultDataView.SortDescriptions.Add(new SortDescription(header, _sortDirection));
        }

        private void LoadResources()
        {
            //string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/XamlResources.xaml");
            //this.Resources.Source = new Uri(path);
            //_resourcesNotLoaded = false;
        }

        public void HeaderFillRemainingSpace()
        {
            if(!FillingHeader.HasValue) return;
            ListViewTools.HeaderFillRemainingSpace(this, FillingHeader.Value);
        }

        private void ListViewWithHeaderSort_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HeaderFillRemainingSpace();
        }

        public int? FillingHeader { get; set; }
    }
}
