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
            InitiaalizeObservableObjects();

            InitializeComponent();
            this.Loaded += RankingsViewPageLoaded;
        }

        private void InitiaalizeObservableObjects()
        {
            //SortingMethods = new ObservableCollection<string>(_parent.Data.AllSortingMethods.ConvertAll(x => x.Name));
            //SelectedSortingMethod = SortingMethods.FirstOrDefault();
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

        private ObservableCollection<string> _sortingMethods;
        public ObservableCollection<string> SortingMethods
        {
            get { return _sortingMethods; }
            set { _sortingMethods = value; OnPropertyChanged(nameof(SortingMethods)); }
        }

        private static string _selectedSortingMethod = string.Empty;
        public string SelectedSortingMethod
        {
            get { return _selectedSortingMethod; }
            set { _selectedSortingMethod = value; OnPropertyChanged(nameof(SelectedSortingMethod)); }
        }

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

    }
}
