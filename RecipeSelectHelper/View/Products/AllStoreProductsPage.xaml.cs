using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;
using AddElementBasePage = RecipeSelectHelper.View.Miscellaneous.AddElementBasePage;

namespace RecipeSelectHelper.View.Products
{
    /// <summary>
    /// Interaction logic for AllStoreProductsPage.xaml
    /// </summary>
    public partial class AllStoreProductsPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Product> _storeProducts;
        public ObservableCollection<Product> StoreProducts
        {
            get { return _storeProducts; }
            set { _storeProducts = value; OnPropertyChanged(nameof(StoreProducts)); }
        }

        private Product _selectedStoreProduct;
        public Product SelectedStoreProduct
        {
            get { return _selectedStoreProduct; }
            set { _selectedStoreProduct = value; OnPropertyChanged(nameof(SelectedStoreProduct)); }
        }

        private ObservableCollection<FilterProductCategory> _filterPc;
        public ObservableCollection<FilterProductCategory> FilterPc
        {
            get { return _filterPc; }
            set { _filterPc = value; OnPropertyChanged(nameof(FilterPc)); }
        }

        private ObservableCollection<FilterGroupedProductCategories> _filterGpc;
        public ObservableCollection<FilterGroupedProductCategories> FilterGpc
        {
            get { return _filterGpc; }
            set { _filterGpc = value; OnPropertyChanged(nameof(FilterGpc)); }
        }

        #endregion

        public AllStoreProductsPage(MainWindow parent)
        {
            _parent = parent;
            Loaded += AllStoreProductsPage_Loaded;
            InitializeComponent();
        }

        private void AllStoreProductsPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            TextBox_SearchStoreProducts.Focus();
        }

        private void InitializeObservableObjects()
        {
            StoreProducts = new ObservableCollection<Product>(OrderBy.OrderByName(_parent.Data.AllProducts));
            FilterPc = new ObservableCollection<FilterProductCategory>(_parent.Data.AllProductCategories.ConvertAll(x => new FilterProductCategory(x)));
            FilterGpc = new ObservableCollection<FilterGroupedProductCategories>(_parent.Data.AllGroupedProductCategories.ConvertAll(x => new FilterGroupedProductCategories(x)));
            SelectedStoreProduct = null;
        }

        private void Button_RemoveStoreProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.RemoveElement(SelectedStoreProduct); 

            Product selected = SelectedStoreProduct;
            ObservableCollection<Product> newProductCollection = StoreProducts;
            ListViewTools.RemoveElementAndSelectPrevious(ref selected, ref newProductCollection);
            SelectedStoreProduct = selected;
            StoreProducts = newProductCollection;

            ListView_StoreProducts.Focus();
        }

        private void Button_EditStoreProduct_OnClick(object sender, RoutedEventArgs e)
        {
            DisplayProductInfo(SelectedStoreProduct);
        }

        private void Button_AddStoreProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddStoreProductPage(_parent), "Add New Store Product", _parent));
        }

        private void Button_SearchStoreProducts_OnClick(object sender, RoutedEventArgs e) => SortListView();

        private void TextBox_SearchStoreProducts_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SortListView();
                TextBox_SearchStoreProducts.Focus();
            }
        }

        private void SortListView()
        {
            List<ProductCategory> containsPc = FilterPc.GetSelected();
            List<ProductCategory> containsGpc = FilterGpc.GetSelected();
            ListView_StoreProducts.SetProductFilter(TextBox_SearchStoreProducts.Text, containsPc, containsGpc);
            ListView_StoreProducts.ApplyFilter();
        }

        #region deprecated

        // This method can be rewritten to be quite a bit more readable and efficient!
        // At the moment it reapplies ALL filters every time it is run, and doesn't "just" apply the changes.
        //private void AddCategoryFilters()
        //{
        //    foreach (FilterProductCategory pc in FilterRc)
        //    {
        //        if (pc.Bool)
        //        {
        //            ListView_StoreProducts.AddAdditionalFilter<Product>(x => x.Categories.Any(y => y.Equals(pc.Instance)));
        //        }
        //    }

        //    var pcsToCheckFor = new List<ProductCategory>();
        //    foreach (FilterGroupedProductCategories gpc in FilterGrc)
        //    {
        //        pcsToCheckFor.AddRange(gpc.GetCheckedCategories());
        //    }

        //    ListView_StoreProducts.AddAdditionalFilter<Product>(x => x.GetCheckedGroupedCategories().ContainsAll(pcsToCheckFor));
        //}

        //private void AddSearchTextFilter()
        //{
        //    ListView_StoreProducts.SetFilter<Product>(x => x.Name.ContainsCaseInsensitive(TextBox_SearchStoreProducts.Text));
        //}

        //private void FilterProductsByName(string searchParameter)
        //{
        //    StoreProducts = new ObservableCollection<Product>(_parent.Data.AllProducts.Where(x => x.Name.Contains(searchParameter)));
        //}

        #endregion

        private void ListViewItem_OnDoubleClick(object sender, MouseButtonEventArgs e)
            => DisplayProductInfo(SelectedStoreProduct);

        public void DisplayProductInfo(Product product)
        {
            if (product != null)
            {
                MessageBox.Show(product.ToString(_parent.Data.ProductSubstitutes));
            }
        }
    }
}
