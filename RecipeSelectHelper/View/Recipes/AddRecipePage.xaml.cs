using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;
using RecipeSelectHelper.View.Categories;
using RecipeSelectHelper.View.Miscellaneous;
using RecipeSelectHelper.View.Products;
using AddElementBasePage = RecipeSelectHelper.View.Miscellaneous.AddElementBasePage;

namespace RecipeSelectHelper.View.Recipes
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page, IAddElement, INotifyPropertyChanged
    {
        private IParentPage _parent;
        private ValidityChecker _valid;

        public AddRecipePage(IParentPage parent)
        {
            _parent = parent;
            InitializeObservableObjects();
            Loaded += AddRecipePage_Loaded;
            InitializeComponent();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string RecipeName { get; set; }

        public string RecipeDescription { get; set; }

        public StringList RecipeInstructions { get; set; } = new StringList();

        public int RecipeServings { get; set; } = 1;

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

        public event EventHandler<bool> ItemSuccessfullyAdded;

        private void AddRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            _valid = new ValidityChecker(_parent.Data);
            UpdateObservableObjects();
        }

        private static List<GroupedSelection<RecipeCategory>> _addedGrc;
        private static List<RecipeCategory> _addedRc;
        private static List<Product> _addedIng;
        private void UpdateObservableObjects() 
        {
            foreach (GroupedSelection<RecipeCategory> grc in _parent.Data.AllGroupedRecipeCategories)
            {
                if (!_addedGrc.Contains(grc))
                {
                    GroupedRecipeCategories.Add(new GroupedRecipeCategory(grc));
                    _addedGrc.Add(grc);
                }
            }
            foreach (RecipeCategory rc in _parent.Data.AllRecipeCategories)
            {
                if (!_addedRc.Contains(rc))
                {
                    RecipeCategories.Add(new Boolable<RecipeCategory>(rc));
                    _addedRc.Add(rc);
                }
            }
            foreach (Product i in _parent.Data.AllProducts)
            {
                if (!_addedIng.Contains(i))
                {
                    Ingredients.Add(new BoolableWithValue<Product, int>(i));
                    _addedIng.Add(i);
                }
            }
        }

        private void InitializeObservableObjects()
        // This method only is invoked on proper instantiation of the page (ie. new()), and not on navigation-events
        {
            // This is done to save the items that are added to the page's observable objects. When we then reload the page, we do
            // not to reload already loaded objects. As such, checkmarks and input is preserved upon switching pages.
            _addedGrc = new List<GroupedSelection<RecipeCategory>>(_parent.Data.AllGroupedRecipeCategories);
            _addedRc = new List<RecipeCategory>(_parent.Data.AllRecipeCategories);
            _addedIng = new List<Product>(_parent.Data.AllProducts);

            GroupedRecipeCategories = new ObservableCollection<GroupedRecipeCategory>(_addedGrc.ConvertAll(x => new GroupedRecipeCategory(x)));
            RecipeCategories = new ObservableCollection<Boolable<RecipeCategory>>(_addedRc.ConvertAll(x => new Boolable<RecipeCategory>(x)));
            Ingredients = new ObservableCollection<BoolableWithValue<Product, int>>(_addedIng.ConvertAll(x => new BoolableWithValue<Product, int>(x)));
        }

        private void ClearUiElements()
        {
            RecipeName = String.Empty;
            RecipeDescription = String.Empty;
            RecipeInstructions = new StringList();
            InitializeObservableObjects();
            TextBox_RecipeName.Focus(); //ClearValue(Border.BorderBrushProperty);
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
            List<GroupedRecipeCategory> groupedRc = GroupedRecipeCategories.ToList();
            List<RecipeCategory> categories = RecipeCategories.Where(x => x.Bool).ToList().ConvertAll(y => y.Instance);
            List<Ingredient> ingredients = Ingredients.Where(x => x.Bool).ToList().ConvertAll(y => new Ingredient(Convert.ToUInt32(y.Value), y.Instance));

            var recipe = new Recipe(RecipeName, RecipeServings, RecipeDescription, RecipeInstructions, ingredients, categories, groupedRc);
            List<string> errors;
            bool success = _valid.RecipeIsValid(recipe, out errors);
            ItemSuccessfullyAdded?.Invoke(this, success);
            if (success)
            {
                _parent.Data.AllRecipes.Add(recipe);
                ClearUiElements();
            }
            else
            {
                MessageBox.Show(string.Join("\n", errors));
            }
        }

        private void Button_AddGroupedCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddGroupedRecipeCategoryPage(_parent), "Add New Recipe Types", _parent));
        }

        private void RecipeNameChanged(object sender, RoutedEventArgs e)
        {
            RecipeNameValid = _valid.RecipeNameIsValid(TextBox_RecipeName.Text);
        }
    }
}
