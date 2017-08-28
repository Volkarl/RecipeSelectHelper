using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;
using AddElementBasePage = RecipeSelectHelper.View.Miscellaneous.AddElementBasePage;

namespace RecipeSelectHelper.View.SortingMethods
{
    /// <summary>
    /// Interaction logic for AllSortingMethodsPage.xaml
    /// </summary>
    public partial class AllSortingMethodsPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public AllSortingMethodsPage(MainWindow parent)
        {
            _parent = parent;
            DataContext = this;

            Loaded += AllSortingMethodsPage_Loaded;
            InitializeComponent();
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

        #endregion

        private void InitializeObservableObjects()
        {
            SortingMethods = new ObservableCollection<SortingMethod>(_parent.Data.AllSortingMethods);
            SelectedSortingMethod = null;
        }

        private void AllSortingMethodsPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
        }

        private void Button_AddSortingMethod_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new SortingMethods.AddSortingMethodPage(_parent), "Add New Sorting Method", _parent));
        }

        private void Button_EditSortingMethod_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSortingMethod == null) return;
            string s = "";
            foreach (IPreference preference in SelectedSortingMethod.Preferences)
            {
                s += preference.Description + " | ";
            }
            MessageBox.Show(SelectedSortingMethod.Name + "Preferences: \n" + s);
        }

        private void Button_RemoveSortingMethod_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSortingMethod == null) return;
            _parent.Data.RemoveElement(SelectedSortingMethod); //AllSortingMethods.Remove(SelectedSortingMethod);

            SortingMethod sortingMethodToRemove = SelectedSortingMethod;
            ObservableCollection<SortingMethod> newSortingMethodCollection = SortingMethods;
            ListViewTools.RemoveElementAndSelectPrevious(ref sortingMethodToRemove, ref newSortingMethodCollection);
            SelectedSortingMethod = sortingMethodToRemove;
            SortingMethods = newSortingMethodCollection;

            ListView_SortingMethods.Focus();
        }
    }
}
