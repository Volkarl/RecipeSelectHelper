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

namespace RecipeSelectHelper.View.BoughtProducts
{
    /// <summary>
    /// Interaction logic for AllBoughtProductsPage.xaml
    /// </summary>
    public partial class AllBoughtProductsPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<BoughtProduct> _boughtProducts;
        public ObservableCollection<BoughtProduct> BoughtProducts
        {
            get { return _boughtProducts; }
            set { _boughtProducts = value; OnPropertyChanged(nameof(BoughtProducts)); }
        }

        private BoughtProduct _selectedBoughtProduct;
        public BoughtProduct SelectedBoughtProduct
        {
            get { return _selectedBoughtProduct; }
            set { _selectedBoughtProduct = value; OnPropertyChanged(nameof(SelectedBoughtProduct)); }
        }

        private ObservableCollection<BoughtProduct> _expiredProducts;
        public ObservableCollection<BoughtProduct> ExpiredProducts
        {
            get { return _expiredProducts; }
            set { _expiredProducts = value; OnPropertyChanged(nameof(ExpiredProducts)); }
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

        public AllBoughtProductsPage(MainWindow parent)
        {
            this._parent = parent;
            Loaded += FridgePage_Loaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            BoughtProducts = new ObservableCollection<BoughtProduct>(OrderBy.OrderByName(_parent.Data.AllBoughtProducts));
            FilterPc = new ObservableCollection<FilterProductCategory>(_parent.Data.AllProductCategories.ConvertAll(x => new FilterProductCategory(x)));
            FilterGpc = new ObservableCollection<FilterGroupedProductCategories>(_parent.Data.AllGroupedProductCategories.ConvertAll(x => new FilterGroupedProductCategories(x)));
            SelectedBoughtProduct = null;
            ExpiredProducts = new ObservableCollection<BoughtProduct>(BoughtProducts.GetExpiredProducts());
        }

        private void FridgePage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            TextBox_SearchBoughtProducts.Focus();
        }

        private void Button_SearchBoughtProducts_OnClick(object sender, RoutedEventArgs e) => SortListView();

        private void TextBox_SearchBoughtProducts_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SortListView();
                TextBox_SearchBoughtProducts.Focus();
            }
        }

        private void SortListView()
        {
            List<ProductCategory> containsPc = FilterPc.GetSelected();
            List<ProductCategory> containsGpc = FilterGpc.GetSelected();
            ListView_BoughtProducts.SetBoughtProductFilter(TextBox_SearchBoughtProducts.Text, containsPc, containsGpc);
            ListView_BoughtProducts.ApplyFilter();
        }

        private void Button_AddBoughtProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddBoughtProductPage(_parent), "Add New Product to Fridge", _parent));
        }

        private void Button_EditBoughtProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(_parent));
        }

        private void Button_RemoveBoughtProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.RemoveElement(SelectedBoughtProduct); //AllBoughtProducts.Remove(SelectedBoughtProduct);

            BoughtProduct selectedBp = SelectedBoughtProduct;
            ObservableCollection<BoughtProduct> tempBpCollection = BoughtProducts;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedBp, ref tempBpCollection);
            SelectedBoughtProduct = selectedBp;
            BoughtProducts = tempBpCollection;

            ListView_BoughtProducts.Focus();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var bp = item?.Content as BoughtProduct;
            MessageBox.Show(bp?.ToString());
        }

        private void Button_ReviewExpiredItems_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
