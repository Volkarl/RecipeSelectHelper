using System;
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
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class AllCategoriesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public AllCategoriesPage(MainWindow parent)
        {
            this._parent = parent;
            this.Loaded += AllCategoriesPage_Loaded;
            InitializeComponent();
        }

        private void AllCategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
            OrderCategoriesAlphabetically();
        }

        private void OrderCategoriesAlphabetically()
        {
            SortCategory(Categories.AddCategoriesPage.CategoryMode.ProductCategory);
            SortCategory(Categories.AddCategoriesPage.CategoryMode.RecipeCategory);
        }
        private void SortCategory(Categories.AddCategoriesPage.CategoryMode mode)
        {
            if (mode == Categories.AddCategoriesPage.CategoryMode.ProductCategory)
            {
                ProductCategories = new ObservableCollection<ProductCategory>(ProductCategories.OrderBy(x => x.Name));
            }
            if (mode == Categories.AddCategoriesPage.CategoryMode.RecipeCategory)
            {
                RecipeCategories = new ObservableCollection<RecipeCategory>(RecipeCategories.OrderBy(x => x.Name));
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
            _parent.ContentControl.Content = new AddElementBasePage(new Categories.AddCategoriesPage(_parent.Data, Categories.AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category", _parent);
        }

        private void Button_EditProductCategory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProductCategory == null) return;
            MessageBox.Show(SelectedProductCategory.Name + " | OwnValue " + SelectedProductCategory.OwnValue);
        }

        private void Button_RemoveProductCategory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProductCategory == null) return;
            _parent.Data.RemoveElement(SelectedProductCategory); //.AllProductCategories.Remove(SelectedProductCategory);

            ProductCategory selectedPc = SelectedProductCategory;
            ObservableCollection<ProductCategory> pc = ProductCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedPc, ref pc);
            SelectedProductCategory = selectedPc;
            ProductCategories = pc;

            ListView_PC.Focus();
        }

        private void Button_AddRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new Categories.AddCategoriesPage(_parent.Data, Categories.AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category", _parent);
        }

        private void Button_EditRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRecipeCategory == null) return;
            MessageBox.Show(SelectedRecipeCategory.Name + " | OwnValue " + SelectedRecipeCategory.OwnValue);
        }

        private void Button_RemoveRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRecipeCategory == null) return;
            _parent.Data.RemoveElement(SelectedRecipeCategory); //AllRecipeCategories.Remove(SelectedRecipeCategory);

            RecipeCategory selectedRc = SelectedRecipeCategory;
            ObservableCollection<RecipeCategory> rc = RecipeCategories;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedRc, ref rc);
            SelectedRecipeCategory = selectedRc;
            RecipeCategories = rc;

            ListView_RC.Focus();
        }

        private void Button_ViewGroupedCategories_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AllGroupedCategoriesPage(_parent));
        }

        private void Button_EvaluateMissingRecipes_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedRecipeCategory == null || _parent.Data.AllRecipes.IsNullOrEmpty()) return;

            _parent.SetPage(new MassEditElementsPage(
                _parent,
                "Evaluate All Recipes By Category",
                $"Recipe Category: {SelectedRecipeCategory}",
                $"Should the recipe contain the category {SelectedRecipeCategory.Name}?",
                _parent.Data.AllRecipes.ConvertAll(r => (object) r),
                o => 
                {
                    var r = (Recipe) o;
                    return $"{FullItemDescriptor.GetDescription(r)}\n\n" +
                           $"{(r.Categories.Contains(SelectedRecipeCategory) ? "Recipe already contains the category." : "Recipe does not contain the category.")}\n";
                },

                o =>
                {
                    var r = (Recipe) o;
                    if (!r.Categories.Contains(SelectedRecipeCategory)) r.Categories.Add(SelectedRecipeCategory);
                },

                o =>
                {
                    var r = (Recipe) o;
                    if (r.Categories.Contains(SelectedRecipeCategory)) r.Categories.Remove(SelectedRecipeCategory);
                }));
        }

        private void Button_EvaluateMissingProducts_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedProductCategory == null || _parent.Data.AllProducts.IsNullOrEmpty()) return;

            _parent.SetPage(new MassEditElementsPage(
                _parent,
                "Evaluate All Products By Category",
                $"Product Category: {SelectedProductCategory}",
                $"Should the product contain the category {SelectedProductCategory.Name}?",
                _parent.Data.AllProducts.ConvertAll(p => (object)p),
                o =>
                {
                    var p = (Product) o;
                    return $"{FullItemDescriptor.GetDescription(p)}\n\n" +
                           $"{(p.Categories.Contains(SelectedProductCategory) ? "Product already contains the category." : "Product does not contain the category.")}\n";
                },

                o =>
                {
                    var p = (Product) o;
                    if (!p.Categories.Contains(SelectedProductCategory)) p.Categories.Add(SelectedProductCategory);
                },

                o =>
                {
                    var p = (Product) o;
                    if (p.Categories.Contains(SelectedProductCategory)) p.Categories.Remove(SelectedProductCategory);
                }));
        }
    }
}
