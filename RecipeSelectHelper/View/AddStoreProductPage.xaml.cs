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

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for AddStoreProductPage.xaml
    /// </summary>
    public partial class AddStoreProductPage : Page, IAddElement
    {
        private MainWindow _parent;
        private List<ProductCategory> _selectedPC;
        private List<Product> _selectedSub;

        public AddStoreProductPage(MainWindow parent)
        {
            _parent = parent;
            _selectedPC = new List<ProductCategory>();
            _selectedSub = new List<Product>();
            InitializeComponent();
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            var categories = new List<ProductCategory>();
            var substituteProducts = new List<Product>();

//            foreach (UIElement item in StackPanel_ChosenCategories.Children)
//            {
//                var stack = item as StackPanel;
//                var label = stack.Children[0] as Label;
//            }

            throw new NotImplementedException(); // HERE!!

            var product = new Product(TextBox_ProductName.Text, categories, substituteProducts);
            _parent.Data.AllProducts.Add(product);
        }

        private void Button_AddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddStoreProductPage(_parent), "Add New Store Product", _parent);
        }

        private void listViewItem_Substitutes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var product = item.Content as Product;

            SelectSubstitute(product);
        }

        private void SelectSubstitute(Product product)
        {
            _selectedSub.Add(product);
            var content = new StackPanel { Orientation = Orientation.Horizontal };
            content.Children.Add(new Label { Content = product.Name });

            var btn = new Button();
            btn.Content = "X";
            btn.Click += RemoveElementFromStackPanel;
            btn.Click += (x, y) => _selectedSub.Remove(product);
            content.Children.Add(btn);

            StackPanel_ChosenSubstituteProducts.Children.Add(content);
        }

        private void Button_SelectSubsituteProduct_Click(object sender, RoutedEventArgs e)
        {
            //SelectSubstitute(SelectedSubstituteProducts);
            throw new NotImplementedException();
        }

        private void Button_ViewSubstituteProduct_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("");
        }

        private void Button_ViewCategory_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_SelectCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_parent, AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category", _parent);
        }

        private void listViewItem_Categories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var pc = item.Content as ProductCategory;

            SelectCategory(pc);
        }

        private void SelectCategory(ProductCategory pc)
        {
            _selectedPC.Add(pc);
            var content = new StackPanel { Orientation = Orientation.Horizontal };
            content.Children.Add(new Label { Content = pc.Name });

            var btn = new Button();
            btn.Content = "X";
            btn.Click += RemoveElementFromStackPanel;
            btn.Click += (x, y) => _selectedPC.Remove(pc);
            content.Children.Add(btn);

            StackPanel_ChosenCategories.Children.Add(content);
        }

        private void RemoveElementFromStackPanel(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var stack = btn.Parent as StackPanel;
            var parentStack = stack.Parent as StackPanel;
            parentStack.Children.Remove(stack);
        }
    }
}
