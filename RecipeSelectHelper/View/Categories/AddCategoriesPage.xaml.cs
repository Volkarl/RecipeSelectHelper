using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddCategoriesPage.xaml
    /// </summary>
    public partial class AddCategoriesPage : Page, IAddElement
    {
        public enum CategoryMode
        {
            ProductCategory, RecipeCategory
        }
        private CategoryMode _mode;
        private MainWindow _parent;

        public AddCategoriesPage(MainWindow parent, CategoryMode mode)
        {
            _parent = parent;
            _mode = mode;
            InitializeComponent();
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            if (_mode == CategoryMode.ProductCategory)
            {
                _parent.Data.AllProductCategories.Add(new ProductCategory(TextBox_CategoryName.Text));
            }
            if (_mode == CategoryMode.RecipeCategory)
            {
                _parent.Data.AllRecipeCategories.Add(new RecipeCategory(TextBox_CategoryName.Text));
            }
            // Add error handling in the ctor of recipe and product category. For instance, null and string.empty is not allowed.

            ClearUIElements();
        }

        private void ClearUIElements()
        {
            TextBox_CategoryName.Text = string.Empty;
        }
    }
}
