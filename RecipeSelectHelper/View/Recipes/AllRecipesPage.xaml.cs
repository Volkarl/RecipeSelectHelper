using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Recipes
{
    /// <summary>
    /// Interaction logic for AllRecipesPage.xaml
    /// </summary>
    public partial class AllRecipesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;
        private List<RecipeCategory> _selectedCategories;

        public AllRecipesPage(MainWindow parent)
        {
            this._parent = parent;
            InitializeObservableObjects();

            this.Loaded += AllRecipesPageLoaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes);
            SelectedRecipe = null;
        }

        private void AllRecipesPageLoaded(object sender, RoutedEventArgs e)
        {
            SetUpWrapPanel();
            FilterRecipesByName(TextBox_SearchRecipes.Text);
            Recipes = new ObservableCollection<Recipe>(SortByName(Recipes));
            _selectedCategories = new List<RecipeCategory>();
            TextBox_SearchRecipes.Focus();
        }

        private IEnumerable<Recipe> SortByName(IEnumerable<Recipe> recipes)
        {
            return recipes.OrderBy(x => x.Name);
        }

        private void SetUpWrapPanel()
        {
            foreach (RecipeCategory rc in _parent.Data.AllRecipeCategories)
            {
                var check = new CheckBox { Content = rc.Name };
                check.Checked += (sender, e) => CategorySelected(rc);
                check.Unchecked += (sender, e) => CategoryDeselected(rc);
                WrapPanel_RecipeCategories.Children.Add(check);
            }
        }

        private void CategoryDeselected(RecipeCategory rc)
        {
            if (_selectedCategories.Remove(rc))
            {
                IEnumerable<Recipe> filteredRecipes = FilterBySelectedCategories(_parent.Data.AllRecipes);
                filteredRecipes = SortByName(filteredRecipes);
                Recipes = new ObservableCollection<Recipe>(filteredRecipes);
            }
        }

        private void CategorySelected(RecipeCategory rc)
        {
            _selectedCategories.Add(rc);
            Recipes = new ObservableCollection<Recipe>(FilterBySelectedCategories(Recipes.ToList()));
        }

        private List<Recipe> FilterBySelectedCategories(List<Recipe> recipesToFilter)
        {
            var filteredRecipes = recipesToFilter.ToList();
            foreach (RecipeCategory cat in _selectedCategories)
            {
                foreach (Recipe rec in recipesToFilter)
                {
                    if (rec.Categories.All(x => !x.Equals(cat)))
                    {
                        filteredRecipes.Remove(rec);
                    }
                }
            }
            return filteredRecipes;
        }

        private void Button_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new Resources.AddElementBasePage(new Recipes.AddRecipePage(_parent), "Add New Recipe", _parent));
        }

        private void Button_EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRecipe != null)
            {
                DisplayRecipeInfo(SelectedRecipe);
            }
        }

        private void Button_RemoveRecipe_Click(object sender, RoutedEventArgs e)
        {
            _parent.Data.RemoveElement(SelectedRecipe); //.AllRecipes.Remove(SelectedRecipe);

            Recipe selectedR = SelectedRecipe;
            ObservableCollection<Recipe> tempRecipeCollection = Recipes;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedR, ref tempRecipeCollection);
            SelectedRecipe = selectedR;
            Recipes = tempRecipeCollection;

            ListView_Recipes.Focus();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Recipe> _recipes;
        public ObservableCollection<Recipe> Recipes
        {
            get { return _recipes; }
            set { _recipes = value; OnPropertyChanged(nameof(Recipes)); }
        }

        private Recipe _selectedRecipe;
        public Recipe SelectedRecipe
        {
            get { return _selectedRecipe; }
            set { _selectedRecipe = value; OnPropertyChanged(nameof(SelectedRecipe)); }
        }

        #endregion

        private void TextBox_SearchRecipes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FilterRecipesByName(TextBox_SearchRecipes.Text);
                TextBox_SearchRecipes.Focus();
            }
        }

        private void FilterRecipesByName(string searchParameter)
        {
            Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes.Where(x => x.Name.Contains(searchParameter)));
        }

        private void Button_SearchRecipes_Click(object sender, RoutedEventArgs e)
        {
            FilterRecipesByName(TextBox_SearchRecipes.Text);
            TextBox_SearchRecipes.Focus();
        }

        private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            Recipe rec = (Recipe)item.Content;

            DisplayRecipeInfo(rec);
        }

        private void DisplayRecipeInfo(Recipe rec)
        {
            string s = "Name: " + rec.Name + "\n";
            //s += "ID: " + rec.ID + "\n";
            s += "Categories: " + rec.CategoriesAsString + "\n";
            s += "Grouped Categories: ";
            foreach (GroupedRecipeCategory groupedRc in rec.GroupedCategories)
            {
                s += groupedRc + ", ";
            }
            s = s.TrimEnd(' ', ',');
            s += "\n";
            s += "Description: " + rec.Description + "\n";
            s += "Instruction: " + rec.Instruction + "\n";
            s += "Value: " + rec.Value + "\n";
            s += "Ingredients: ";
            foreach (Ingredient ing in rec.Ingredients)
            {
                s += ing.CorrespondingProduct.Name + " (";
                s += "value: " + ing.Value + ", ";
                s += ing.AmountNeeded + " needed), ";
            }
            MessageBox.Show(s, "Recipe information");
        }
    }
}
