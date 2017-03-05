using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddGroupedProductCategories.xaml
    /// </summary>
    public partial class AddGroupedProductCategories : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public AddGroupedProductCategories(MainWindow parent)
        {
            _parent = parent;
            LoadObservableObjects();
            InitializeComponent();
        }

        private void LoadObservableObjects()
        {
            MinSelectionAmount = 0;
            MaxSelectionAmount = 3;
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _minSelectionAmount;
        public int MinSelectionAmount
        {
            get { return _minSelectionAmount; }
            set { _minSelectionAmount = value; OnPropertyChanged(nameof(MinSelectionAmount)); }
        }

        private int _maxSelectionAmount;

        public int MaxSelectionAmount
        {
            get { return _maxSelectionAmount; }
            set { _maxSelectionAmount = value; OnPropertyChanged(nameof(MaxSelectionAmount)); }
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            SearchableListView something;
            new GroupedSelection<ProductCategory>(null, MinSelectionAmount, MaxSelectionAmount);
        }

        #endregion
    }
}
