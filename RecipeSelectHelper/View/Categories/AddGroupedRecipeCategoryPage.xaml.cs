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
using RecipeSelectHelper.View.Miscellaneous;
using AddElementBasePage = RecipeSelectHelper.View.Miscellaneous.AddElementBasePage;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddGroupedRecipeCategoryPage.xaml
    /// </summary>
    public partial class AddGroupedRecipeCategoryPage : Page, IAddElement, INotifyPropertyChanged
    {
        private IParentPage _parent;
        private static ProgramData _data = new ProgramData();

        public AddGroupedRecipeCategoryPage(IParentPage parent)
        {
            _parent = parent;
            ResetInternalData();
            Loaded += AddGroupedRecipeCategoryPage_Loaded;
            InitializeComponent();
        }

        private void ResetInternalData()
        {
            // This is done to make sure that new instances of the AddRcPage are empty, 
            // while navigating back and forth still retains data
            _data = new ProgramData();
        }

        private void AddGroupedRecipeCategoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadObservableObjects();
        }

        private void LoadObservableObjects()
        {
            RecipeCategories = new ObservableCollection<RecipeCategory>(_data.AllRecipeCategories.OrderBy(x => x.Name));
            // This is done to not lose all created recipe categories when we switch pages (to add new recipe categories for instance)
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static int _minSelectionAmount;
        public int MinSelectionAmount
        {
            get { return _minSelectionAmount; }
            set { _minSelectionAmount = value; OnPropertyChanged(nameof(MinSelectionAmount)); }
        }

        private static int _maxSelectionAmount;
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
        public RecipeCategory SelectedRc
        {
            get { return _selectedRc; }
            set { _selectedRc = value; OnPropertyChanged(nameof(SelectedRc)); }
        }
        #endregion

        public event EventHandler<bool> ItemSuccessfullyAdded;

        public void AddItem(object sender, RoutedEventArgs e)
        {
            var categories = new List<RecipeCategory>();

            foreach (RecipeCategory rc in RecipeCategories)
            {
                categories.Add(rc);
            }

            try
            {
                var selection = new GroupedSelection<RecipeCategory>(categories, MinSelectionAmount, MaxSelectionAmount);
                _parent.Data.AllGroupedRecipeCategories.Add(selection);
                ClearPage();
                ItemSuccessfullyAdded?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                ItemSuccessfullyAdded?.Invoke(this, false);
                MessageBox.Show(ex.Message);
            }
        }

        private void ClearPage()
        {
            _data = new ProgramData();
            RecipeCategories = new ObservableCollection<RecipeCategory>();
            MinSelectionAmount = 0;
            MaxSelectionAmount = 0;
        }

        private void Button_AddNewRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddCategoriesPage(_data, AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category To Group", _parent));
        }

        private void Button_EditRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonRemoveRecipeCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _data.RemoveElement(SelectedRc);

            RecipeCategory selectedRc = SelectedRc;
            ObservableCollection<RecipeCategory> rc = RecipeCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedRc, ref rc);
            SelectedRc = selectedRc;
            RecipeCategories = rc;

            ListView_RecipeCategories.Focus();
        }
    }
}
