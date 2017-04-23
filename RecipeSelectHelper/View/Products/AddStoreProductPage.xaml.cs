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
using RecipeSelectHelper.View.Categories;

namespace RecipeSelectHelper.View.Products
{
    /// <summary>
    /// Interaction logic for AddStoreProductPage.xaml
    /// </summary>
    public partial class AddStoreProductPage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        private ValidityChecker _valid;

        public AddStoreProductPage(MainWindow parent)
        {
            _parent = parent;
            InitializeObservableObjects();
            Loaded += AddStoreProductPage_Loaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            _addedGpc = new List<GroupedSelection<ProductCategory>>(_parent.Data.AllGroupedProductCategories);
            GroupedProductCategories = new ObservableCollection<GroupedProductCategory>(_addedGpc.ConvertAll(x => new GroupedProductCategory(x)));
        }

        private static List<GroupedSelection<ProductCategory>> _addedGpc;
        private void UpdateObservableObjects()
        {
            foreach (GroupedSelection<ProductCategory> gpc in _parent.Data.AllGroupedProductCategories)
            {
                if (!_addedGpc.Contains(gpc)) GroupedProductCategories.Add(new GroupedProductCategory(gpc));
            }
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<GroupedProductCategory> _groupedProductCategories;
        public ObservableCollection<GroupedProductCategory> GroupedProductCategories
        {
            get { return _groupedProductCategories; }
            set { _groupedProductCategories = value; OnPropertyChanged(nameof(GroupedProductCategories)); }
        }

        private bool? _productNameValid;
        public bool? ProductNameValid
        {
            get { return _productNameValid; }
            set
            {
                _productNameValid = value;
                OnPropertyChanged(nameof(ProductNameValid));
            }
        }

        #endregion

        private void AddStoreProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            _valid = new ValidityChecker(_parent.Data);
            UpdateObservableObjects();

            SearchableListView_SubstituteProducts.InitializeSearchableListView(
                _parent.Data.AllProducts, 
                "Name", 
                (sp, searchParameter) => sp.Name.ToLower().Contains(searchParameter.ToLower()));

            SearchableListView_ProductCategories.InitializeSearchableListView(
                _parent.Data.AllProductCategories,
                "Name",
                (pc, searchParameter) => pc.Name.ToLower().Contains(searchParameter.ToLower()));

            TextBox_ProductName.Focus();
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            foreach (object item in SearchableListView_ProductCategories.InnerListView.SelectedItems)
            {
                categories.Add(item as ProductCategory);
            }

            List<Product> substituteProducts = new List<Product>();
            foreach (object item in SearchableListView_SubstituteProducts.InnerListView.SelectedItems)
            {
                substituteProducts.Add(item as Product);
            }

            List<GroupedProductCategory> groupedProductCategories = GroupedProductCategories.ToList();

            var product = new Product(TextBox_ProductName.Text, categories, groupedProductCategories);
            _parent.Data.AllProducts.Add(product);
            _parent.Data.ProductSubstitutes.AddSubstitutes(product, substituteProducts);
            
            ClearUI();
        }

        private void ClearUI()
        {
            TextBox_ProductName.Text = String.Empty;
            SearchableListView_ProductCategories.ClearSelection();
            SearchableListView_SubstituteProducts.ClearSelection();
            Expander_ProductCategories.IsExpanded = false;
            Expander_SubstituteProducts.IsExpanded = false;
            // Perhaps I should think about adding a clear button and leaving it all intact usually.
        }

        private void Button_AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new Resources.AddElementBasePage(new AddStoreProductPage(_parent), "Add New Store Product", _parent);
        }

        private void Button_AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new Resources.AddElementBasePage(new Categories.AddCategoriesPage(_parent.Data, Categories.AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category", _parent);
        }

        private void Button_ViewSelectedSubstituteProducts_OnClick(object sender, RoutedEventArgs e)
        {
            SearchableListView_SubstituteProducts.DisplaySelectedItems();
        }

        private void Button_ViewSelectedCategories_OnClick(object sender, RoutedEventArgs e)
        {
            SearchableListView_ProductCategories.DisplaySelectedItems();
        }


        private void Button_AddGroupedCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddGroupedProductCategoryPage(_parent), "Add New Product Types", _parent));
        }

        private void ProductNameChanged(object sender, RoutedEventArgs e)
        {
            ProductNameValid = _valid.StoreProductNameIsValid(TextBox_ProductName.Text);
        }
    }
}
