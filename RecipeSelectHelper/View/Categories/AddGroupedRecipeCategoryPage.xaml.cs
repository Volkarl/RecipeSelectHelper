using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddGroupedRecipeCategoryPage.xaml
    /// </summary>
    public partial class AddGroupedRecipeCategoryPage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        private static ProgramData _data = new ProgramData();

        public AddGroupedRecipeCategoryPage(MainWindow parent)
        {
            _parent = parent;

            Loaded += AddGroupedRecipeCategoryPage_Loaded;
            InitializeComponent();
        }

        private void AddGroupedRecipeCategoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadObservableObjects();
        }

        private void LoadObservableObjects()
        {
            RecipeCategories = new ObservableCollection<RecipeCategory>(_data.AllRecipeCategories.OrderBy(x => x.Name));
            // This is done to not lose all created recipe categories when we switch pages (to add new recipe categories for instance)
            SelectedRC = null;
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static int _minSelectionAmount = 0;
        public int MinSelectionAmount
        {
            get { return _minSelectionAmount; }
            set { _minSelectionAmount = value; OnPropertyChanged(nameof(MinSelectionAmount)); }
        }

        private static int _maxSelectionAmount = 3;
        public int MaxSelectionAmount
        {
            get { return _maxSelectionAmount; }
            set { _maxSelectionAmount = value; 
                //value > RecipeCategories.Count ? RecipeCategories.Count : value;
                OnPropertyChanged(nameof(MaxSelectionAmount)); }
        }

        private ObservableCollection<RecipeCategory> _recipeCategories;
        public ObservableCollection<RecipeCategory> RecipeCategories
        {
            get { return _recipeCategories; }
            set { _recipeCategories = value; OnPropertyChanged(nameof(RecipeCategories)); }
        }

        private RecipeCategory _selectedRc;
        public RecipeCategory SelectedRC
        {
            get { return _selectedRc; }
            set { _selectedRc = value; OnPropertyChanged(nameof(SelectedRC)); }
        }
        #endregion

        public void AddItem(object sender, RoutedEventArgs e)
        {
            List<RecipeCategory> categories = new List<RecipeCategory>();

            foreach (RecipeCategory rc in RecipeCategories)
            {
                categories.Add(rc);
            }

            var selection = new GroupedSelection<RecipeCategory>(categories, MinSelectionAmount, MaxSelectionAmount);
            _parent.Data.AllGroupedRecipeCategories.Add(selection);
            ClearPage();
        }

        private void ClearPage()
        {
            _data = new ProgramData();
            RecipeCategories = new ObservableCollection<RecipeCategory>();
            MinSelectionAmount = 0;
            MaxSelectionAmount = 3;
        }

        private void Button_AddNewRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_data, AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category To Group", _parent);
        }

        private void Button_EditRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonRemoveRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _data.AllRecipeCategories.Remove(SelectedRC);

            RecipeCategory selectedRc = SelectedRC;
            ObservableCollection<RecipeCategory> RC = RecipeCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedRc, ref RC);
            SelectedRC = selectedRc;
            RecipeCategories = RC;

            ListView_RecipeCategories.Focus();
        }
    }
}
