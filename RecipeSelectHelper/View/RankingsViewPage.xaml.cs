using RecipeSelectHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RecipeSelectHelper.Model.SortingMethods;

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for RankingsViewPage.xaml
    /// </summary>
    public partial class RankingsViewPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public RankingsViewPage(MainWindow parent)
        {
            this._parent = parent;
            DataContext = this;
            InitializeObservableObjects();

            InitializeComponent();
            this.Loaded += RankingsViewPageLoaded;
        }

        private void InitializeObservableObjects()
        {
            SortingMethods = new ObservableCollection<SortingMethod>(_parent.Data.AllSortingMethods.OrderBy(x => x.Name));
            SelectedSortingMethod = null;
            Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes.OrderBy(x => x.Name)); // SHOULD BE EMPTY??
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

        #region ObservableObjects

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

        private void Button_SortRecipes_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar_Sorting.Value = 0;

            if (SelectedSortingMethod == null) return;
            SelectedSortingMethod.ProgressChanged += ChangeProgressBarValue;
            SelectedSortingMethod.Execute(_parent.Data);

            Recipes = new ObservableCollection<Recipe>(_parent.Data.AllRecipes.OrderBy(x => x.Value));
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
