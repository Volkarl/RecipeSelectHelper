using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Products
{
    /// <summary>
    /// Interaction logic for AddStoreProductPage.xaml
    /// </summary>
    public partial class AddStoreProductPage : Page, IAddElement
    {
        private MainWindow _parent;
        //private List<ProductCategory> _selectedPC;
        //private List<Product> _selectedSub;

        public AddStoreProductPage(MainWindow parent)
        {
            _parent = parent;
            //_selectedPC = new List<ProductCategory>();
            //_selectedSub = new List<Product>();
            Loaded += AddStoreProductPage_Loaded;
            InitializeComponent();
        }

        private void AddStoreProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            SearchableListView_SubstituteProducts.InitializeSearchableListView(
                _parent.Data.AllProducts, 
                "Name", 
                (sp, searchParameter) => sp.Name.ToLower().Contains(searchParameter.ToLower()));


            SearchableListView_ProductCategories.InitializeSearchableListView(
                _parent.Data.AllProductCategories,
                "Name",
                (pc, searchParameter) => pc.Name.ToLower().Contains(searchParameter.ToLower()));
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

            var product = new Product(TextBox_ProductName.Text, categories, substituteProducts);
            _parent.Data.AllProducts.Add(product);

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

        //private void SelectSubstitute(Product product)
        //{
        //    _selectedSub.Add(product);
        //    var content = new StackPanel { Orientation = Orientation.Horizontal };
        //    content.Children.Add(new Label { Content = product.Name });

        //    var btn = new Button();
        //    btn.Content = "X";
        //    btn.Click += RemoveElementFromStackPanel;
        //    btn.Click += (x, y) => _selectedSub.Remove(product);
        //    content.Children.Add(btn);

        //    StackPanel_ChosenSubstituteProducts.Children.Add(content);
        //}

        //private void SelectCategory(ProductCategory pc)
        //{
        //    _selectedPC.Add(pc);
        //    var content = new StackPanel { Orientation = Orientation.Horizontal };
        //    content.Children.Add(new Label { Content = pc.Name });

        //    var btn = new Button();
        //    btn.Content = "X";
        //    btn.Click += RemoveElementFromStackPanel;
        //    btn.Click += (x, y) => _selectedPC.Remove(pc);
        //    content.Children.Add(btn);

        //    StackPanel_ChosenCategories.Children.Add(content);
        //}

        //private void RemoveElementFromStackPanel(object sender, RoutedEventArgs e)
        //{
        //    var btn = sender as Button;
        //    var stack = btn.Parent as StackPanel;
        //    var parentStack = stack.Parent as StackPanel;
        //    parentStack.Children.Remove(stack);
        //}

        private void Button_ViewSelectedSubstituteProducts_OnClick(object sender, RoutedEventArgs e)
        {
            SearchableListView_SubstituteProducts.DisplaySelectedItems();
        }

        private void Button_ViewSelectedCategories_OnClick(object sender, RoutedEventArgs e)
        {
            SearchableListView_ProductCategories.DisplaySelectedItems();
        }


    }
}
