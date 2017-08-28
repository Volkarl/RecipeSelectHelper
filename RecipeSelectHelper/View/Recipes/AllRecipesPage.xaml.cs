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
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;
using AddElementBasePage = RecipeSelectHelper.View.Miscellaneous.AddElementBasePage;

namespace RecipeSelectHelper.View.Recipes
{
    /// <summary>
    /// Interaction logic for AllRecipesPage.xaml
    /// </summary>
    public partial class AllRecipesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

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

        private ObservableCollection<FilterRecipeCategory> _filterRc;
        public ObservableCollection<FilterRecipeCategory> FilterRc
        {
            get { return _filterRc; }
            set { _filterRc = value; OnPropertyChanged(nameof(FilterRc)); }
        }

        private ObservableCollection<FilterGroupedRecipeCategories> _filterGrc;
        public ObservableCollection<FilterGroupedRecipeCategories> FilterGrc
        {
            get { return _filterGrc; }
            set { _filterGrc = value; OnPropertyChanged(nameof(FilterGrc)); }
        }

        #endregion

        public AllRecipesPage(MainWindow parent)
        {
            this._parent = parent;
            this.Loaded += AllRecipesPageLoaded;
            InitializeComponent();
        }

        private void InitializeObservableObjects()
        {
            Recipes = new ObservableCollection<Recipe>(OrderBy.OrderByName(_parent.Data.AllRecipes));
            FilterRc = new ObservableCollection<FilterRecipeCategory>(_parent.Data.AllRecipeCategories.ConvertAll(x => new FilterRecipeCategory(x)));
            FilterGrc = new ObservableCollection<FilterGroupedRecipeCategories>(_parent.Data.AllGroupedRecipeCategories.ConvertAll(x => new FilterGroupedRecipeCategories(x)));
            SelectedRecipe = null;
        }

        private void AllRecipesPageLoaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            TextBox_SearchRecipes.Focus();
        }

        //private void SetUpWrapPanel()
        //{
        //    foreach (RecipeCategory rc in _parent.Data.AllRecipeCategories)
        //    {
        //        var check = new CheckBox { Content = rc.Name };
        //        check.Checked += (sender, e) => CategorySelected(rc);
        //        check.Unchecked += (sender, e) => CategoryDeselected(rc);
        //        WrapPanel_RecipeCategories.Children.Add(check);
        //    }
        //}

        //private void CategoryDeselected(RecipeCategory rc)
        //{
        //    if (_selectedCategories.Remove(rc))
        //    {
        //        IEnumerable<Recipe> filteredRecipes = FilterBySelectedCategories(_parent.Data.AllRecipes);
        //        filteredRecipes = SortByName(filteredRecipes);
        //        Recipes = new ObservableCollection<Recipe>(filteredRecipes);
        //    }
        //}

        //private void CategorySelected(RecipeCategory rc)
        //{
        //    _selectedCategories.Add(rc);
        //    Recipes = new ObservableCollection<Recipe>(FilterBySelectedCategories(Recipes.ToList()));
        //}

        //private List<Recipe> FilterBySelectedCategories(List<Recipe> recipesToFilter)
        //{
        //    var filteredRecipes = recipesToFilter.ToList();
        //    foreach (RecipeCategory cat in _selectedCategories)
        //    {
        //        foreach (Recipe rec in recipesToFilter)
        //        {
        //            if (rec.Categories.All(x => !x.Equals(cat)))
        //            {
        //                filteredRecipes.Remove(rec);
        //            }
        //        }
        //    }
        //    return filteredRecipes;
        //}

        private void Button_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddRecipePage(_parent), "Add New Recipe", _parent));
        }

        private void Button_EditRecipe_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(_parent));
        }

        private void Button_RemoveRecipe_Click(object sender, RoutedEventArgs e)
        {
            _parent.Data.RemoveElement(SelectedRecipe);

            Recipe selectedR = SelectedRecipe;
            ObservableCollection<Recipe> tempRecipeCollection = Recipes;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedR, ref tempRecipeCollection);
            SelectedRecipe = selectedR;
            Recipes = tempRecipeCollection;

            ListView_Recipes.Focus();
        }

        private void TextBox_SearchRecipes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SortListView();
                TextBox_SearchRecipes.Focus();
            }
        }

        //private void FilterRecipesByName(string searchParameter)
        //{
        //    Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes.Where(x => x.Name.Contains(searchParameter)));
        //}

        private void Button_SearchRecipes_Click(object sender, RoutedEventArgs e) => SortListView();

        private void SortListView()
        {
            List<RecipeCategory> containsRc = FilterRc.GetSelected();
            List<RecipeCategory> containsGrc = FilterGrc.GetSelected();
            ListView_Recipes.SetRecipeFilter(TextBox_SearchRecipes.Text, containsRc, containsGrc);
            ListView_Recipes.ApplyFilter();
        }

        private void listViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            Recipe rec = (Recipe)item.Content;

            DisplayRecipeInfo(rec);
        }

        private void DisplayRecipeInfo(Recipe r)
        {
            MessageBox.Show(FullItemDescriptor.GetDescription(r), "Recipe information");
        }
    }
}
