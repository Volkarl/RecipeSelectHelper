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
            InitializeObservableObjects();
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            ProductExpiration = new Boolable<ExpirationInfo>(new ExpirationInfo());
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

        // observablecollection of StoreProducts here

        #endregion

        private void AddBoughtProductPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Success");
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

            var now = DateTime.Now;
            ProductExpiration.Instance.ProductCreatedTime = now.Subtract(new TimeSpan(daysAgo, 0, 0, 0));

            HighlightButtonBackground(btn);
        }

        private void HighlightButtonBackground(Button button)
        {
            ClearOtherButtons();
            button.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void ClearOtherButtons()
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
