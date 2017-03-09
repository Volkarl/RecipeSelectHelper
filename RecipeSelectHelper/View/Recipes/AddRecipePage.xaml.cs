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
using RecipeSelectHelper.View.Products;

namespace RecipeSelectHelper.View.Recipes
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        private ValidityChecker _valid;

        public AddRecipePage(MainWindow parent)
        {
            _parent = parent;
            Loaded += AddRecipePage_Loaded;
            InitializeComponent();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool? _recipeNameValid;
        public bool? RecipeNameValid
        {
            get { return _recipeNameValid; }
            set { _recipeNameValid = value;
                OnPropertyChanged(nameof(RecipeNameValid));
            }
        }

        private ObservableCollection<GroupedRecipeCategory> _groupedRecipeCategories;
        public ObservableCollection<GroupedRecipeCategory> GroupedRecipeCategories
        {
            get { return _groupedRecipeCategories; }
            set { _groupedRecipeCategories = value; OnPropertyChanged(nameof(GroupedRecipeCategories)); }
        }

        private ObservableCollection<Boolable<RecipeCategory>> _recipeCategories;
        public ObservableCollection<Boolable<RecipeCategory>> RecipeCategories
        {
            get { return _recipeCategories; }
            set { _recipeCategories = value; OnPropertyChanged(nameof(RecipeCategories)); }
        }

        private ObservableCollection<BoolableWithValue<Product, int>> _ingredients;
        public ObservableCollection<BoolableWithValue<Product, int>> Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; OnPropertyChanged(nameof(Ingredients)); }
        }

        #endregion

        private void AddRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            _valid = new ValidityChecker(_parent.Data);
        }

        private void InitializeObservableObjects()
        {
            GroupedRecipeCategories = new ObservableCollection<GroupedRecipeCategory>(_parent.Data.AllGroupedRecipeCategories.ConvertAll(x => new GroupedRecipeCategory(x)));
            RecipeCategories = new ObservableCollection<Boolable<RecipeCategory>>(_parent.Data.AllRecipeCategories.ConvertAll(x => new Boolable<RecipeCategory>(x)));
            Ingredients = new ObservableCollection<BoolableWithValue<Product, int>>(_parent.Data.AllProducts.ConvertAll(x => new BoolableWithValue<Product, int>(x)));
        }

        private void ClearUIElements()
        {
            TextBox_RecipeName.Text = string.Empty;
            TextBox_RecipeDescription.Text = string.Empty;
            TextBox_RecipeInstruction.Text = string.Empty;
            InitializeObservableObjects();
        }

        private void Button_AddCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddCategoriesPage(_parent.Data, AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category", _parent));
        }

        private void Button_AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddStoreProductPage(_parent), "Add New Store Product", _parent));
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            string name = TextBox_RecipeName.Text;
            string description = TextBox_RecipeDescription.Text;
            string instruction = TextBox_RecipeInstruction.Text;
            List<GroupedRecipeCategory> groupedRc = GroupedRecipeCategories.ToList();
            List<RecipeCategory> categories = RecipeCategories.Where(x => x.Bool).ToList().ConvertAll(y => y.Instance);
            List<Ingredient> ingredients = Ingredients.Where(x => x.Bool).ToList().ConvertAll(y => new Ingredient(y.Value, y.Instance));

            string error;
            if (_valid.RecipeNameIsValid(name, out error) &&
                _valid.DescriptionIsValid(name, out error) &&
                _valid.InstructionIsValid(name, out error) &&
                _valid.GroupedRcAreValid(groupedRc, out error) &&
                _valid.CategoriesAreValid(categories, out error) &&
                _valid.IngredientsAreValid(ingredients, out error))
            {
                bool innerErrorOccured = false;
                try
                {
                    var recipe = new Recipe(name, description, instruction, ingredients, categories, groupedRc);
                    _parent.Data.AllRecipes.Add(recipe);
                }
                catch (Exception ex)
                {
                    innerErrorOccured = true;
                    MessageBox.Show(ex.Message);
                }
                if(!innerErrorOccured) ClearUIElements();
            }
            else
            {
                MessageBox.Show(error);
            }
        }

        private void Button_AddGroupedCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddGroupedRecipeCategoryPage(_parent), "Add New Recipe Types", _parent));
        }

        private void Button_RecipeNameIsUnique_OnClick(object sender, RoutedEventArgs e)
        {
            string error;
            RecipeNameValid = _valid.RecipeNameIsValid(TextBox_RecipeName.Text, out error);
            if (!RecipeNameValid.Value)
            {
                TextBox_RecipeName.BorderBrush = System.Windows.Media.Brushes.Red;
                TextBox_RecipeName.ToolTip = error;
            }
            else
            {
                TextBox_RecipeName.BorderBrush = System.Windows.Media.Brushes.LawnGreen;
                TextBox_RecipeName.ToolTip = null;
            }
        }
    }
}
