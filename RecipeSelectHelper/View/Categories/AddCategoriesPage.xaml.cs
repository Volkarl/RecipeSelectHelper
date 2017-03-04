using System;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

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
        private ProgramData _data;

        public AddCategoriesPage(ProgramData data, CategoryMode mode)
        {
            _data = data;
            _mode = mode;
            InitializeComponent();
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            if (_mode == CategoryMode.ProductCategory)
            {
                _data.AllProductCategories.Add(new ProductCategory(TextBox_CategoryName.Text));
            }
            if (_mode == CategoryMode.RecipeCategory)
            {
                _data.AllRecipeCategories.Add(new RecipeCategory(TextBox_CategoryName.Text));
            }
            // Add error handling in the ctor of recipe and product category. For instance, null and string.empty is not allowed.

            ClearUIElements();
            TextBox_CategoryName.Focus();
        }

        private void ClearUIElements()
        {
            TextBox_CategoryName.Text = string.Empty;
        }
    }
}
