using RecipeSelectHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using RecipeSelectHelper.Model.Misc;
using RecipeSelectHelper.Model.SortingMethods;

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for RankingsViewPage.xaml
    /// </summary>
    public partial class RankingsViewPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public RankingsViewPage(MainWindow parent)
        {
            this._parent = parent;
            InitializeObservableObjects();

            InitializeComponent();
            this.Loaded += RankingsViewPageLoaded;
        }

        private void InitializeObservableObjects()
        {
            SortingMethods = new ObservableCollection<SortingMethod>(_parent.Data.AllSortingMethods.OrderBy(x => x.Name));
            SelectedSortingMethod = null;
            SelectedRecipe = null;

            var withPercentageScores = new List<RecipeWithPercentageScore>();
            foreach (Recipe rec in _parent.Data.AllRecipes)
            {
                withPercentageScores.Add(new RecipeWithPercentageScore(rec));
            }
            Recipes = new ObservableCollection<RecipeWithPercentageScore>(withPercentageScores.OrderBy(x => x.CorrespondinRecipe.Name)); // SHOULD BE EMPTY??
        }

        private void RankingsViewPageLoaded(object sender, RoutedEventArgs e)
        {
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<SortingMethod> _sortingMethods;
        public ObservableCollection<SortingMethod> SortingMethods
        {
            get { return _sortingMethods; }
            set { _sortingMethods = value; OnPropertyChanged(nameof(SortingMethods)); }
        }

        private SortingMethod _selectedSortingMethod;
        public SortingMethod SelectedSortingMethod
        {
            get { return _selectedSortingMethod; }
            set { _selectedSortingMethod = value; OnPropertyChanged(nameof(SelectedSortingMethod)); }
        }

        private ObservableCollection<RecipeWithPercentageScore> _recipes;
        public ObservableCollection<RecipeWithPercentageScore> Recipes
        {
            get { return _recipes; }
            set { _recipes = value; OnPropertyChanged(nameof(Recipes)); }
        }

        private RecipeWithPercentageScore _selectedRecipe;
        public RecipeWithPercentageScore SelectedRecipe
        {
            get { return _selectedRecipe; }
            set { _selectedRecipe = value; OnPropertyChanged(nameof(SelectedRecipe)); }
        }

        #endregion

        private void Button_SortRecipes_Click(object sender, RoutedEventArgs e)
        {
            _parent.Data.ResetAllValues();
            ProgressBar_Sorting.Value = 0;

            if (SelectedSortingMethod == null) return;
            SelectedSortingMethod.ProgressChanged += ChangeProgressBarValue;
            SelectedSortingMethod.Execute(_parent.Data);

            // Use recipe values to assign percentage scores to the recipes.
            List<Recipe> allRecipes = _parent.Data.AllRecipes;
            int maxValue = allRecipes.Max(x => x.Value);
            var withPercentageScores = new ObservableCollection<RecipeWithPercentageScore>();
            foreach (Recipe recipe in allRecipes)
            {
                withPercentageScores.Add(new RecipeWithPercentageScore(recipe, maxValue));
            }
            Recipes = new ObservableCollection<RecipeWithPercentageScore>(withPercentageScores.OrderBy(x => x.PercentageValue).Reverse());
            MessageBox.Show("Successfully Sorted");
        }

        private void ChangeProgressBarValue(object sender, double e)
        {
            if ((ProgressBar_Sorting.Value + e) > 100)
            {
                ProgressBar_Sorting.Value = 100;
            }
            else
            {
                ProgressBar_Sorting.Value += e;
            }
        }
    }
}
