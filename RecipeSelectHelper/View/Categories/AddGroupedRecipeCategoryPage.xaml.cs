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
            set { _maxSelectionAmount = value; OnPropertyChanged(nameof(MaxSelectionAmount)); }
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

            var groupedCategories = new GroupedSelection<RecipeCategory>(categories, MinSelectionAmount, MaxSelectionAmount);
            _parent.Data.AllGroupedRecipeCategories.Add(groupedCategories);
            ClearPage();
        }

        private void ClearPage()
        {
            _data = new ProgramData();
        }

        private void Button_AddNewRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_data, AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category To Group", _parent);
        }
    }
}
