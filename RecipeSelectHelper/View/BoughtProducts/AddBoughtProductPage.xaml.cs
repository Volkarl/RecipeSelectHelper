using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.BoughtProducts
{
    /// <summary>
    /// Interaction logic for AddBoughtProductPage.xaml
    /// </summary>
    public partial class AddBoughtProductPage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public AddBoughtProductPage(MainWindow parent)
        {
            _parent = parent;
            this.Loaded += AddBoughtProductPage_Loaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            ProductExpiration = new Boolable<ExpirationInfo>(new ExpirationInfo());
            StoreProducts = new ObservableCollection<Product>(OrderBy.OrderByName(_parent.Data.AllProducts));
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Boolable<ExpirationInfo> _productExpiration;
        public Boolable<ExpirationInfo> ProductExpiration
        {
            get { return _productExpiration; }
            set
            {
                _productExpiration = value;
                OnPropertyChanged(nameof(ProductExpiration));
            }
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

        private void AddBoughtProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            SetProductCreationTime(0);
            HighlightButtonBackground(ButtonDefaultExpiration);
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            try
            {
                BoughtProduct bp = new BoughtProduct(SelectedStoreProduct, uint.Parse(IntegerTextBoxAmountBought.Text), ProductExpiration.Bool ? ProductExpiration.Instance : new ExpirationInfo());
                _parent.Data.AllBoughtProducts.Add(bp);
                ClearUiElements();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Success");
        }

        private void ClearUiElements()
        {
            ClearButtonColor();
            ProductExpiration = new Boolable<ExpirationInfo>(new ExpirationInfo());
        }

        private void TextBox_SearchParameter_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter) CollectionViewSource.GetDefaultView(ListView_Items.ItemsSource).Refresh();
        }

        private void ListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var s = sender as ListViewItem;
            if (s != null) MessageBox.Show(s.Content.ToString());
        }

        private void Button_OnClick_ChangeProductCreatedTime(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if(btn == null) return;
            var daysAgo = Convert.ToInt32(btn.Tag);

            SetProductCreationTime(daysAgo);
            HighlightButtonBackground(btn);
        }

        private void SetProductCreationTime(int daysAgo)
        {
            var now = DateTime.Now;
            ProductExpiration.Instance.ProductCreatedTime = now.Subtract(new TimeSpan(daysAgo, 0, 0, 0));
        }

        private void HighlightButtonBackground(Button button)
        {
            ClearButtonColor();
            button.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void ClearButtonColor()
        {
            foreach (UIElement child in UniformGridProducedDateButtons.Children)
            {
                var btn = child as Button;
                if (btn != null) btn.Background = _defaultButtonColor;
            }
        }

        private readonly SolidColorBrush _defaultButtonColor = new SolidColorBrush(Colors.LightGray);
    }
}
