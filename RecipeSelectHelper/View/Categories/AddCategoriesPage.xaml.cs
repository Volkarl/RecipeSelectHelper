using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddCategoriesPage.xaml
    /// </summary>
    public partial class AddCategoriesPage : Page, IAddElement, INotifyPropertyChanged
    {
        public enum CategoryMode
        {
            ProductCategory, RecipeCategory
        }
        private CategoryMode _mode;
        private ProgramData _data;
        private bool? _categoryNameValid;
        private ValidityChecker _valid;

        public AddCategoriesPage(ProgramData data, CategoryMode mode)
        {
            _data = data;
            _mode = mode;
            Loaded += AddCategoriesPage_Loaded;
            InitializeComponent();
        }

        private void AddCategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            _valid = new ValidityChecker(_data);
            TextBox_CategoryName.Focus();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool? CategoryNameValid
        {
            get { return _categoryNameValid; }
            set
            {
                _categoryNameValid = value;
                OnPropertyChanged(nameof(CategoryNameValid));
            }
        }

        #endregion

        public event EventHandler<bool> ItemSuccessfullyAdded;

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

            ItemSuccessfullyAdded?.Invoke(this, true);
            ClearUiElements();
            TextBox_CategoryName.Focus();
        }

        private void ClearUiElements()
        {
            TextBox_CategoryName.Text = string.Empty;
        }

        private void CategoryNameChanged(object sender, RoutedEventArgs e)
        {
            CategoryNameValid = _mode == CategoryMode.ProductCategory ? 
                _valid.ProductCategoryNameIsValid(TextBox_CategoryName.Text) : 
                _valid.RecipeCategoryNameIsValid(TextBox_CategoryName.Text);
        }
    }
}
