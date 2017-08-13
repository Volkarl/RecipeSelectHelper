using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Miscellaneous
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
            if (_parent.Data.AllRecipes != null && _parent.Data.AllRecipes.Any())
            {
                int maxVal = _parent.Data.AllRecipes.Max(x => x.Value);
                foreach (Recipe rec in _parent.Data.AllRecipes)
                    withPercentageScores.Add(new RecipeWithPercentageScore(rec, maxVal));
            }
            Recipes = new ObservableCollection<RecipeWithPercentageScore>(withPercentageScores.OrderBy(x => x.CorrespondingRecipe.Name)); 
        }

        private void RankingsViewPageLoaded(object sender, RoutedEventArgs e) { }

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
            ProgressBar_Sorting.ProgressBar.Value = 0;

            if (SelectedSortingMethod == null) return;
            SelectedSortingMethod.ProgressChanged += ChangeProgressBarValue;
            SelectedSortingMethod.Execute(_parent.Data, CheckBox_SubstitutesEnabled.IsChecked ?? false);

            // Use recipe values to assign percentage scores to the recipes.
            List<Recipe> allRecipes = _parent.Data.AllRecipes;
            int maxValue = allRecipes.Max(x => x.Value);
            var withPercentageScores = new ObservableCollection<RecipeWithPercentageScore>(allRecipes.ConvertAll(r => new RecipeWithPercentageScore(r, maxValue))); 
            Recipes = new ObservableCollection<RecipeWithPercentageScore>(withPercentageScores.OrderBy(x => x.PercentageValue).Reverse());
            MessageBox.Show("Successfully Sorted");
        }

        private void ChangeProgressBarValue(object sender, double e)
        {
            if ((ProgressBar_Sorting.ProgressBar.Value + e) > 100)
            {
                ProgressBar_Sorting.ProgressBar.Value = 100;
                ProgressBar_Sorting.OuterText = "100 %";
            }
            else
            {
                ProgressBar_Sorting.ProgressBar.Value += e;
                ProgressBar_Sorting.OuterText = e + " %";
            }
        }

        private void ScrollViewer_MouseWheelScrolling(object sender, MouseWheelEventArgs e) => 
            CsharpResources.ScrollViewer_MouseWheelScrolling(sender, e);
    }
}
