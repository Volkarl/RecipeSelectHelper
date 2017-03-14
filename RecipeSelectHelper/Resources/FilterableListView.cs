using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class FilterableListView : ExtendedListView
    {
        private CollectionView _view;

        public FilterableListView()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            Loaded += FilterableListView_Loaded;
        }

        private void FilterableListView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _view = (CollectionView)CollectionViewSource.GetDefaultView(this.ItemsSource);
            _view.Filter = x => true;
            //SetFilter<object>(x => true);
        }

        //public void SetFilter<T>(Predicate<T> filterFunc) where T : class
        //{
        //    _view.Filter = x => filterFunc(x as T);
        //}

        //public void AddAdditionalFilter<T>(Predicate<T> filterFunc) where T : class
        //{
        //    SetFilter<T>(x => _view.Filter(x) && filterFunc(x));
        //}

        public void SetProductFilter(string name, List<ProductCategory> containsPc, List<ProductCategory> containsGpc)
        {
            Predicate<Product> nameTrue = x => x.Name.ContainsCaseInsensitive(name);
            Predicate<Product> pcTrue = x => x.Categories.ContainsAll(containsPc);
            Predicate<Product> gpcTrue = x => x.GetCheckedGroupedCategories().ContainsAll(containsGpc);

            bool anyPcs = containsPc.Any();
            bool anyGpcs = containsGpc.Any();
            if(anyPcs && anyGpcs) _view.Filter = x => nameTrue(x as Product) && pcTrue(x as Product) && gpcTrue(x as Product);
            else if(anyPcs) _view.Filter = x => nameTrue(x as Product) && pcTrue(x as Product);
            else _view.Filter = x => nameTrue(x as Product) && gpcTrue(x as Product);
        }

        public void ApplyFilter()
        {
            CollectionViewSource.GetDefaultView(ItemsSource).Refresh();
        }
    }
}
