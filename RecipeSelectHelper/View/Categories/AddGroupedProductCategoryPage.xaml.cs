using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddGroupedProductCategoryPage.xaml
    /// </summary>
    public partial class AddGroupedProductCategoryPage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        private static ProgramData _data = new ProgramData();

        public AddGroupedProductCategoryPage(MainWindow parent)
        {
            _parent = parent;
            ResetInternalData();
            Loaded += AddGroupedProductCategoryPage_Loaded;
            InitializeComponent();
        }

        private void ResetInternalData()
        {
            // This is done to make sure that new instances of the AddRcPage are empty, 
            // while navigating back and forth still retains data
            _data = new ProgramData();
        }

        private void AddGroupedProductCategoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadObservableObjects();
        }

        private void LoadObservableObjects()
        {
            ProductCategories = new ObservableCollection<ProductCategory>(_data.AllProductCategories.OrderBy(x => x.Name));
            // This is done to not lose all created recipe categories when we switch pages (to add new recipe categories for instance)
            SelectedPc = null;
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static int _minSelectionAmount;
        public int MinSelectionAmount
        {
            get { return _minSelectionAmount; }
            set { _minSelectionAmount = value; OnPropertyChanged(nameof(MinSelectionAmount)); }
        }

        private static int _maxSelectionAmount;
        public int MaxSelectionAmount
        {
            get { return _maxSelectionAmount; }
            set { _maxSelectionAmount = value; OnPropertyChanged(nameof(MaxSelectionAmount)); }
        }

        private ObservableCollection<ProductCategory> _productCategories;
        public ObservableCollection<ProductCategory> ProductCategories
        {
            get { return _productCategories; }
            set { _productCategories = value; OnPropertyChanged(nameof(ProductCategories)); }
        }

        private ProductCategory _selectedPc;
        public ProductCategory SelectedPc
        {
            get { return _selectedPc; }
            set { _selectedPc = value; OnPropertyChanged(nameof(SelectedPc)); }
        }
        #endregion

        public void AddItem(object sender, RoutedEventArgs e)
        {
            var categories = new List<ProductCategory>();

            foreach (ProductCategory pc in ProductCategories)
            {
                categories.Add(pc);
            }

            try
            {
                var selection = new GroupedSelection<ProductCategory>(categories, MinSelectionAmount, MaxSelectionAmount);
                _parent.Data.AllGroupedProductCategories.Add(selection);
                ClearPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearPage()
        {
            _data = new ProgramData();
            ProductCategories = new ObservableCollection<ProductCategory>();
            MinSelectionAmount = 0;
            MaxSelectionAmount = 0;
        }

        private void Button_AddNewProductCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_data, AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category To Group", _parent);
        }

        private void Button_EditProductCategory_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonRemoveProductCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _data.RemoveElement(SelectedPc);

            ProductCategory selectedPc = SelectedPc;
            ObservableCollection<ProductCategory> rc = ProductCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedPc, ref rc);
            SelectedPc = selectedPc;
            ProductCategories = rc;

            ListView_ProductCategories.Focus();
        }
    }
}
