using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View
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

        public AllStoreProductsPage(MainWindow parent)
        {
            this._parent = parent;
            InitializeObservableObjects();

            Loaded += AllStoreProductsPage_Loaded;
            InitializeComponent();
        }

        private void AllStoreProductsPage_Loaded(object sender, RoutedEventArgs e)
        {
            StoreProducts = new ObservableCollection<Product>(OrderByName(_parent.Data.AllProducts));
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
            FilterProductsByName(TextBox_SearchStoreProducts.Text);
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
