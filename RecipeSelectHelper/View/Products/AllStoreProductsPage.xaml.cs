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

        #endregion

        public List<FilterProductCategory> FilterPc { get; set; }
        public List<FilterGroupedProductCategories> FilterGpc { get; set; }

        public AllStoreProductsPage(MainWindow parent)
        {
            _parent = parent;
            InitializeObservableObjects();

            Loaded += AllStoreProductsPage_Loaded;
            InitializeComponent();
        }

        private void AllStoreProductsPage_Loaded(object sender, RoutedEventArgs e)
        {
            StoreProducts = new ObservableCollection<Product>(OrderByName(_parent.Data.AllProducts));
            FilterPc = _parent.Data.AllProductCategories.ConvertAll(x => new FilterProductCategory(x));
            FilterGpc = _parent.Data.AllGroupedProductCategories.ConvertAll(x => new FilterGroupedProductCategories(x));
            TextBox_SearchStoreProducts.Focus();
        }

        private IEnumerable<Product> OrderByName(IEnumerable<Product> products)
        {
            return products.OrderBy(x => x.Name);
        }

        private void InitializeObservableObjects()
        {
            StoreProducts = new ObservableCollection<Product>();
            SelectedStoreProduct = null;
        }

        private void Button_RemoveStoreProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.AllProducts.Remove(SelectedStoreProduct);

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
            AddSearchTextFilter();
            AddCategoryFilters();
            ListView_StoreProducts.ApplyFilter();
            //FilterProductsByName(TextBox_SearchStoreProducts.Text);
        }

        private void AddCategoryFilters()
        {
            foreach (FilterProductCategory pc in FilterPc)
            {
                if(pc.Bool) ListView_StoreProducts.AddAdditionalFilter<Product>(x => x.Categories.Any(y => y.Equals(pc.Instance)));
            }

            var pcsToCheckFor = new List<ProductCategory>();
            foreach (FilterGroupedProductCategories gpc in FilterGpc)
            {
                pcsToCheckFor.AddRange(gpc.GetCheckedCategories());
            }




            ListView_StoreProducts.AddAdditionalFilter<Product>(x => pcsToCheckFor.Except<ProductCategory>(x.GroupedCategories.ConvertAll(y => y.GetCurrentSelectedItems())));



            foreach (Boolable<ProductCategory> gpc in FilterGpc.)
            {
                foreach (Boolable<ProductCategory> pc in gpc)
                {
                    if (pc.Bool)
                    {
                        
                        Predicate<Product> s = x => x.GroupedCategories.Any(y => y.GroupedPc.Any(z => z.Instance.Equals()))
                    }
                }
            }
            ListView_StoreProducts.AddAdditionalFilter<Product>();
        }

        private bool PcContainsAllBpcs(Product p, Boolable<ProductCategory> bpc)
        {
            var s = p.GroupedCategories.ConvertAll(x => x.GroupedPc);
            var f = s.ConvertAll(x => x.get)
        }

        private void AddSearchTextFilter()
        {
            ListView_StoreProducts.SetFilter<Product>(x => x.Name.Contains(TextBox_SearchStoreProducts.Text));
        }

        private void FilterProductsByName(string searchParameter)
        {
            StoreProducts = new ObservableCollection<Product>(_parent.Data.AllProducts.Where(x => x.Name.Contains(searchParameter)));
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            DisplayProductInfo(SelectedStoreProduct);
        }

        public void DisplayProductInfo(Product product)
        {
            if (product != null)
            {
                MessageBox.Show(product.ToString());
            }
        }
    }
}
