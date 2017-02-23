using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
using System.Xml;

namespace RecipeSelectHelper.View
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

            this.Loaded += RankingsViewPageLoaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes);
            SelectedRecipe = null;
        }

        private void RankingsViewPageLoaded(object sender, RoutedEventArgs e)
        {
            SetUpWrapPanel();
            ListView_SizeChanged(ListView_Recipes, null);
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

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;
            var remainingWidth = listView.ActualWidth - 5;

            for (Int32 i = 1; i < gridView.Columns.Count; i++)
            {
                remainingWidth -= gridView.Columns[i].ActualWidth;
            }


            // ADD LOGIC HERE TO CONTROL THE MIN WIDTH OF THE COLUMNS
            // ADD IT TO THE LISTVIEW TOOLS CLASS


            gridView.Columns[0].Width = remainingWidth;
        }

        //private static int _addRecipeCounter = 1;
        private void Button_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            //string name = "Leftovers" + _addRecipeCounter++;
            //string description = "A mix of leftovers";
            //string instruction = "Mix whatever you've got leftover and eat with lots of ketchup";
            //var ingredients = new List<Ingredient>();
            //var recipeCategories = new List<RecipeCategory>();

            //var productCategories = new List<ProductCategory>();
            //var productcat = new ProductCategory("Green food");
            //_parent.Data.AllProductCategories.Add(productcat);
            //productCategories.Add(productcat);
            //var product1 = new Product("Celery", productCategories);
            //_parent.Data.AllProducts.Add(product1);

            //var productCategories2 = new List<ProductCategory>();
            //productCategories2.Add(productcat);
            //var productcat2 = new ProductCategory("Disgusting");
            //_parent.Data.AllProductCategories.Add(productcat2);
            //productCategories2.Add(productcat2);
            //var product2 = new Product("Milk", productCategories, new List<Product>() { product1 });
            //_parent.Data.AllProducts.Add(product2);

            //var newing = new Ingredient(20, product2);
            //ingredients.Add(newing);

            //var reccat = new RecipeCategory("Non-appetizing food");
            //_parent.Data.AllRecipeCategories.Add(reccat);
            //recipeCategories.Add(reccat);

            //var newRecipe = new Recipe(name, description, instruction, ingredients, recipeCategories);
            
            //Recipes.Add(newRecipe);
            //_parent.Data.AllRecipes.Add(newRecipe);

            _parent.SetPage(new AddElementBasePage(new AddRecipePage(_parent), "Add New Recipe", _parent));
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
            _parent.Data.AllRecipes.Remove(SelectedRecipe);

            Recipe selectedR = SelectedRecipe;
            ObservableCollection<Recipe> newRecipeCollection = Recipes;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedR, ref newRecipeCollection);
            SelectedRecipe = selectedR;
            Recipes = newRecipeCollection;

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
                ListView_Recipes.Focus();
            }
        }

        private void FilterRecipesByName(string searchParameter)
        {
            Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes.Where(x => x.Name.Contains(searchParameter)));
        }

        private void Button_SearchRecipes_Click(object sender, RoutedEventArgs e)
        {
            FilterRecipesByName(TextBox_SearchRecipes.Text);
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
