using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;
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
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class AllCategoriesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        #region ObservableObjects

        private ObservableCollection<ProductCategory> _productCategories;
        public ObservableCollection<ProductCategory> ProductCategories
        {
            get { return _productCategories; }
            set { _productCategories = value; OnPropertyChanged(nameof(ProductCategories)); }
        }

        private ProductCategory _selectedProductCategory;
        public ProductCategory SelectedProductCategory
        {
            get { return _selectedProductCategory; }
            set { _selectedProductCategory = value; OnPropertyChanged(nameof(SelectedProductCategory)); }
        }

        private ObservableCollection<RecipeCategory> _recipeCategories;
        public ObservableCollection<RecipeCategory> RecipeCategories
        {
            get { return _recipeCategories; }
            set { _recipeCategories = value; OnPropertyChanged(nameof(RecipeCategories)); }
        }

        private RecipeCategory _selectedRecipeCategory;
        public RecipeCategory SelectedRecipeCategory
        {
            get { return _selectedRecipeCategory; }
            set { _selectedRecipeCategory = value; OnPropertyChanged(nameof(SelectedRecipeCategory)); }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AllCategoriesPage(MainWindow parent)
        {
            this._parent = parent;
            DataContext = this;

            this.Loaded += AllCategoriesPage_Loaded;
            InitializeComponent();
        }

        private void AllCategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            SortCategory();
        }

        private void SortCategory()
        {
            SortCategory(AddCategoriesPage.CategoryMode.ProductCategory);
            SortCategory(AddCategoriesPage.CategoryMode.RecipeCategory);
        }
        private void SortCategory(AddCategoriesPage.CategoryMode mode)
        {
            if (mode == AddCategoriesPage.CategoryMode.ProductCategory)
            {
                ProductCategories.OrderBy(x => x.Name);
            }
            if (mode == AddCategoriesPage.CategoryMode.RecipeCategory)
            {
                RecipeCategories.OrderBy(x => x.Name);
            }
        }

        private void InitializeObservableObjects()
        {
            ProductCategories = new ObservableCollection<ProductCategory>(_parent.Data.AllProductCategories);
            RecipeCategories = new ObservableCollection<RecipeCategory>(_parent.Data.AllRecipeCategories);
            SelectedProductCategory = null;
            SelectedRecipeCategory = null;
        }

        private void Button_AddProductCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_parent, AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category", _parent);
        }

        private void Button_EditProductCategory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProductCategory != null)
            {
                MessageBox.Show(SelectedProductCategory.Name + " | Value " + SelectedProductCategory.Value);
            }
        }

        private void Button_RemoveProductCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.Data.AllProductCategories.Remove(SelectedProductCategory);

            ProductCategory selectedPC = SelectedProductCategory;
            ObservableCollection<ProductCategory> PC = ProductCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedPC, ref PC);
            SelectedProductCategory = selectedPC;
            ProductCategories = PC;

            ListView_PC.Focus();
        }

        private void Button_AddRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(_parent, AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category", _parent);
        }

        private void Button_EditRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRecipeCategory != null)
            {
                MessageBox.Show(SelectedRecipeCategory.Name + " | Value " + SelectedRecipeCategory.Value);
            }
        }

        private void Button_RemoveRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.Data.AllRecipeCategories.Remove(SelectedRecipeCategory);

            RecipeCategory selectedRC = SelectedRecipeCategory;
            ObservableCollection<RecipeCategory> RC = RecipeCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedRC, ref RC);
            SelectedRecipeCategory = selectedRC;
            RecipeCategories = RC;

            ListView_RC.Focus();
        }
    }
}
