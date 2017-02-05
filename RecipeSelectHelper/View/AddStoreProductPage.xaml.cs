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

        public AddStoreProductPage(MainWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
            var content = new StackPanel { Orientation = Orientation.Horizontal };
            content.Children.Add(new Label { Content = product.Name });

            content.Children.Add(new Button { Content = "X" });

            StackPanel_ChosenSubstituteProducts.Children.Add(content);
        }

        private void Button_SelectSubsituteProduct_Click(object sender, RoutedEventArgs e)
        {
            SelectSubstitute(_parent.Data.AllProducts.First());
            //SelectItemAsSubstitute(SelectedSubstituteProduct);
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
            var content = new StackPanel { Orientation = Orientation.Horizontal };
            content.Children.Add(new Label { Content = pc.Name });
            content.Children.Add(new Button { Content = "X" });


            // I AM MISSING A BUTTONCLICKEVENT THAT REMOVES THE ENTIRE STACKPANEL FROM THE CHOSENCATEGORIES STACKPANEL



            StackPanel_ChosenCategories.Children.Add(content);
        }
    }
}
