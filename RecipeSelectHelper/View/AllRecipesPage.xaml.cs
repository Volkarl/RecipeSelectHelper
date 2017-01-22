using RecipeSelectHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AllRecipesPage.xaml
    /// </summary>
    public partial class AllRecipesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;
        public List<IRecipe> AllRecipes { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AllRecipesPage(MainWindow parent)
        {
            this._parent = parent;
            InitializeObservableObjects();

            InitializeComponent();
            this.Loaded += RankingsViewPageLoaded;
        }

        private void InitializeObservableObjects()
        {
            AllRecipes = _parent.Data.AllRecipes;
            Recipes = new ObservableCollection<IRecipe>(AllRecipes);
            SelectedRecipe = null;
        }

        private void RankingsViewPageLoaded(object sender, RoutedEventArgs e)
        {
            ListView_SizeChanged(ListView_Recipes, null);
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
            gridView.Columns[0].Width = remainingWidth;
        }

        private void Button_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            var newRecipe = new Recipe("Pasta");
            Recipes.Add(newRecipe);
            AllRecipes.Add(newRecipe);
        }

        private void Button_EditRecipe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_RemoveRecipe_Click(object sender, RoutedEventArgs e)
        {
            IRecipe recipeToBeRemoved = SelectedRecipe;
            int indexOfSelection = Recipes.IndexOf(recipeToBeRemoved);
            if (indexOfSelection != 0)
            {
                SelectedRecipe = Recipes[indexOfSelection - 1];
            }
            else
            {
                SelectedRecipe = null;
            }
            ListView_Recipes.Focus();

            Recipes.Remove(recipeToBeRemoved);
            AllRecipes.Remove(recipeToBeRemoved);
        }

        #region ObservableObjects

        private ObservableCollection<IRecipe> _recipes;
        public ObservableCollection<IRecipe> Recipes
        {
            get { return _recipes; }
            set { _recipes = value; OnPropertyChanged(nameof(Recipes)); }
        }

        private IRecipe _selectedRecipe;
        public IRecipe SelectedRecipe
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
            }
        }

        private void FilterRecipesByName(string searchParameter)
        {
            Recipes = new ObservableCollection<IRecipe>(AllRecipes.Where(x => x.Name.Contains(searchParameter)));
        }

        private void Button_SearchRecipes_Click(object sender, RoutedEventArgs e)
        {
            FilterRecipesByName(TextBox_SearchRecipes.Text);
        }
    }
}
