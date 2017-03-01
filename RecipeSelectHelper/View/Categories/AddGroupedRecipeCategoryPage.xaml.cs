using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AddGroupedRecipeCategoryPage.xaml
    /// </summary>
    public partial class AddGroupedRecipeCategoryPage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        
        public AddGroupedRecipeCategoryPage(MainWindow parent)
        {
            _parent = parent;
            LoadObservableObjects();
            Loaded += AddGroupedRecipeCategoryPage_Loaded;
            InitializeComponent();
        }

        private void AddGroupedRecipeCategoryPage_Loaded(object sender, RoutedEventArgs e)
        {
            SearchableListView_RecipeCategories.InitializeSearchableListView(
                _parent.Data.AllRecipeCategories, 
                "Name",
                (sp, searchParameter) => sp.Name.ToLower().Contains(searchParameter.ToLower()));
        }

        private void LoadObservableObjects()
        {
            MinSelectionAmount = 0;
            MaxSelectionAmount = 3;
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _minSelectionAmount;
        public int MinSelectionAmount
        {
            get { return _minSelectionAmount; }
            set { _minSelectionAmount = value; OnPropertyChanged(nameof(MinSelectionAmount)); }
        }

        private int _maxSelectionAmount;

        public int MaxSelectionAmount
        {
            get { return _maxSelectionAmount; }
            set { _maxSelectionAmount = value; OnPropertyChanged(nameof(MaxSelectionAmount)); }
        }

        #endregion

        public void AddItem(object sender, RoutedEventArgs e)
        {
            List<RecipeCategory> categories = new List<RecipeCategory>();
            foreach (object item in SearchableListView_RecipeCategories.InnerListView.SelectedItems)
            {
                categories.Add(item as RecipeCategory);
            }

            var groupedCategories = new GroupedSelection<RecipeCategory>(categories, MinSelectionAmount, MaxSelectionAmount);

            //_parent.Data.AllProducts.Add(product);

            ClearUI();
        }

        private void ClearUI()
        {
            SearchableListView_RecipeCategories.ClearSelection();
        }
    }
}
