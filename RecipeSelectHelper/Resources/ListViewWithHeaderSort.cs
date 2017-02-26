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
    public class ListViewWithHeaderSort : ListView
    {
        public ListViewWithHeaderSort()
        {
            Loaded += ListViewWithHeaderSort_Loaded;
            this.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(SecondResultDataViewClick));
        }

        private void ListViewWithHeaderSort_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private ListSortDirection _sortDirection;
        private GridViewColumnHeader _sortColumn;
        private bool _resourcesNotLoaded = true;

        private void SecondResultDataViewClick(object sender, RoutedEventArgs e)
        {
            if (_resourcesNotLoaded) LoadResources();

            GridViewColumnHeader column = e.OriginalSource as GridViewColumnHeader;
            if (column == null)
            {
                return;
            }

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
                    _sortColumn.Column.Width = _sortColumn.ActualWidth - 20;
                }

                _sortColumn = column;
                _sortDirection = ListSortDirection.Ascending;
                column.Column.Width = column.ActualWidth + 20;
            }

            if (_sortDirection == ListSortDirection.Ascending)
            {
                column.Column.HeaderTemplate = Resources["ArrowUp"] as DataTemplate;
            }
            else
            {
                column.Column.HeaderTemplate = Resources["ArrowDown"] as DataTemplate;
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
            string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources/XamlResources.xaml");
            this.Resources.Source = new Uri(path);
            _resourcesNotLoaded = false;
        }
    }
}
