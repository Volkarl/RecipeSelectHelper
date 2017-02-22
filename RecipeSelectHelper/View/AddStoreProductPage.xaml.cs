using RecipeSelectHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View
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
                (x,y) => ShowProductInformation(x), 
                () => _parent.Data.AllProducts, 
                (text, list) => list.Where(x => x.Name.ToLower().Contains(text.ToLower())).ToList(), 
                "Name");

            SearchableListView_ProductCategories.InitializeSearchableListView(
                (x, y) => ShowPCInformation(x),
                () => _parent.Data.AllProductCategories,
                (text, list) => list.Where(x => x.Name.ToLower().Contains(text.ToLower())).ToList(),
                "Name");
        }

        private void ShowPCInformation(ProductCategory productCategory)
        {
            MessageBox.Show(productCategory.Name);
        }

        private void ShowProductInformation(Product product)
        {
            MessageBox.Show(product.Name);
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            var categories = SearchableListView_ProductCategories.InnerListView.SelectedItems as List<ProductCategory> ??
                      new List<ProductCategory>();
            var substituteProducts = SearchableListView_SubstituteProducts.InnerListView.SelectedItems as List<Product> ??
                new List<Product>();

            var product = new Product(TextBox_ProductName.Text, categories, substituteProducts);
            _parent.Data.AllProducts.Add(product);
        }

        private void Button_AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddStoreProductPage(_parent), "Add New Store Product", _parent);
        }

        private void Button_AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_parent, AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category", _parent);
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
            throw new NotImplementedException();
        }

        private void Button_ViewSelectedCategories_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
