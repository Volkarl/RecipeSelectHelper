using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RecipeSelectHelper.Resources
{
    public class FilterableListView : ExtendedListView
    {
        private CollectionView _view;

        public FilterableListView()
        {
            SetFilter<object>(x => true);
        }

        public void SetFilter<T>(Predicate<T> filterFunc) where T : class
        {
            _view = (CollectionView)CollectionViewSource.GetDefaultView(this.ItemsSource);
            _view.Filter = x => filterFunc(x as T);
        }

        public void AddAdditionalFilter<T>(Predicate<T> filterFunc) where T : class
        {
            SetFilter<T>(x => _view.Filter(x) && filterFunc(x));
        }

        public void ApplyFilter()
        {
            CollectionViewSource.GetDefaultView(ItemsSource).Refresh();
        }
    }
}
