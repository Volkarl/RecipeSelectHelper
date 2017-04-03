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
        }

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

        public void SetBoughtProductFilter(string name, List<ProductCategory> containsPc, List<ProductCategory> containsGpc)
        {
            Predicate<BoughtProduct> nameTrue = x => x.CorrespondingProduct.Name.ContainsCaseInsensitive(name);
            Predicate<BoughtProduct> pcTrue = x => x.CorrespondingProduct.Categories.ContainsAll(containsPc);
            Predicate<BoughtProduct> gpcTrue = x => x.CorrespondingProduct.GetCheckedGroupedCategories().ContainsAll(containsGpc);

            bool anyPcs = containsPc.Any();
            bool anyGpcs = containsGpc.Any();
            if (anyPcs && anyGpcs) _view.Filter = x => nameTrue(x as BoughtProduct) && pcTrue(x as BoughtProduct) && gpcTrue(x as BoughtProduct);
            else if (anyPcs) _view.Filter = x => nameTrue(x as BoughtProduct) && pcTrue(x as BoughtProduct);
            else _view.Filter = x => nameTrue(x as BoughtProduct) && gpcTrue(x as BoughtProduct);
        }

        public void SetRecipeFilter(string name, List<RecipeCategory> containsRc, List<RecipeCategory> containsGrc)
        {
            Predicate<Recipe> nameTrue = x => x.Name.ContainsCaseInsensitive(name);
            Predicate<Recipe> pcTrue = x => x.Categories.ContainsAll(containsRc);
            Predicate<Recipe> gpcTrue = x => x.GetCheckedGroupedCategories().ContainsAll(containsGrc);

            bool anyPcs = containsRc.Any();
            bool anyGpcs = containsGrc.Any();
            if (anyPcs && anyGpcs) _view.Filter = x => nameTrue(x as Recipe) && pcTrue(x as Recipe) && gpcTrue(x as Recipe);
            else if (anyPcs) _view.Filter = x => nameTrue(x as Recipe) && pcTrue(x as Recipe);
            else _view.Filter = x => nameTrue(x as Recipe) && gpcTrue(x as Recipe);
        }

        public void ApplyFilter()
        {
            CollectionViewSource.GetDefaultView(ItemsSource).Refresh();
        }
    }
}
