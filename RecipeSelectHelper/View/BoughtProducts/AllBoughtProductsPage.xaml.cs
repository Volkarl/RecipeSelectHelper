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

        #endregion

        public AllBoughtProductsPage(MainWindow parent)
        {
            this._parent = parent;
            InitializeObservableObjects();

            Loaded += FridgePage_Loaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            BoughtProducts = new ObservableCollection<BoughtProduct>(_parent.Data.AllBoughtProducts);
            SelectedBoughtProduct = null;
            ExpiredProducts = new ObservableCollection<BoughtProduct>(GetExpiredProducts(BoughtProducts));
        }

        private void FridgePage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private List<BoughtProduct> GetExpiredProducts(IEnumerable<BoughtProduct> boughtProducts)
        {
            var expiredProducts = new List<BoughtProduct>();
            foreach (BoughtProduct bp in boughtProducts)
            {
                if (bp.ExpirationData.ProductExpirationTime.HasValue)
                {
                    if (DateTime.Now > bp.ExpirationData.ProductExpirationTime.Value)
                    {
                        expiredProducts.Add(bp);
                    }
                }
            }
            return expiredProducts;
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
            FilterProductsByName(TextBox_SearchBoughtProducts.Text);
        }

        private void FilterProductsByName(string searchParameter)
        {
            BoughtProducts = new ObservableCollection<BoughtProduct>(_parent.Data.AllBoughtProducts.Where(x => x.CorrespondingProduct.Name.Contains(searchParameter)));
        }

        private void Button_AddBoughtProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new Resources.AddElementBasePage(new AddBoughtProductPage(_parent), "Add New Product to Fridge", _parent));
        }

        private void Button_EditBoughtProduct_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_RemoveBoughtProduct_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.AllBoughtProducts.Remove(SelectedBoughtProduct);

            BoughtProduct selectedBp = SelectedBoughtProduct;
            ObservableCollection<BoughtProduct> tempBpCollection = BoughtProducts;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedBp, ref tempBpCollection);
            SelectedBoughtProduct = selectedBp;
            BoughtProducts = tempBpCollection;

            ListView_BoughtProducts.Focus();
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_ReviewExpiredItems_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
