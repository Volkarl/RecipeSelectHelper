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
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.View.Miscellaneous;
using AddElementBasePage = RecipeSelectHelper.View.Miscellaneous.AddElementBasePage;

namespace RecipeSelectHelper.View.Categories
{
    /// <summary>
    /// Interaction logic for AllGroupedCategoriesPage.xaml
    /// </summary>
    public partial class AllGroupedCategoriesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;
        public AllGroupedCategoriesPage(MainWindow parent)
        {
            _parent = parent;
            Loaded += AllGroupedCategoriesPage_Loaded;
            InitializeComponent();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<GroupedSelection<ProductCategory>> _groupedPc;
        public ObservableCollection<GroupedSelection<ProductCategory>> GroupedPc
        {
            get { return _groupedPc; }
            set { _groupedPc = value; OnPropertyChanged(nameof(GroupedPc)); }
        }

        private GroupedSelection<ProductCategory> _selectedGroupedPc;
        public GroupedSelection<ProductCategory> SelectedGroupedPC
        {
            get { return _selectedGroupedPc; }
            set { _selectedGroupedPc = value; OnPropertyChanged(nameof(SelectedGroupedPC)); }
        }

        private ObservableCollection<GroupedSelection<RecipeCategory>> _groupedRc;
        public ObservableCollection<GroupedSelection<RecipeCategory>> GroupedRc
        {
            get { return _groupedRc; }
            set { _groupedRc = value; OnPropertyChanged(nameof(GroupedRc)); }
        }

        private GroupedSelection<RecipeCategory> _selectedGroupedRc;
        public GroupedSelection<RecipeCategory> SelectedGroupedRc
        {
            get { return _selectedGroupedRc; }
            set { _selectedGroupedRc = value; OnPropertyChanged(nameof(SelectedGroupedRc)); }
        }

        #endregion

        private void AllGroupedCategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeObservableObjects();
        }

        private void InitializeObservableObjects()
        {
            GroupedPc = new ObservableCollection<GroupedSelection<ProductCategory>>(_parent.Data.AllGroupedProductCategories.OrderBy(x => x.MinSelect));
            GroupedRc = new ObservableCollection<GroupedSelection<RecipeCategory>>(_parent.Data.AllGroupedRecipeCategories.OrderBy(x => x.MinSelect));
            SelectedGroupedPC = null;
            SelectedGroupedRc = null;
        }

        private void Button_AddGroupedPC_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddGroupedProductCategoryPage(_parent), "Add New Grouped Product Categories", _parent);
        }

        private void Button_EditGroupedPC_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_RemoveGroupedPC_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.RemoveElement(SelectedGroupedPC);

            GroupedSelection<ProductCategory> selectedGpc = SelectedGroupedPC;
            ObservableCollection<GroupedSelection<ProductCategory>> gpc = GroupedPc;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedGpc, ref gpc);
            SelectedGroupedPC = selectedGpc;
            GroupedPc = gpc;

            SearchableListView_GroupedPC.Focus();
        }

        private void Button_AddGroupedRC_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddGroupedRecipeCategoryPage(_parent), "Add New Grouped Recipe Categories", _parent);
        }

        private void Button_EditGroupedRC_OnClick(object sender, RoutedEventArgs e)
        {
        }

        private void Button_RemoveGroupedRC_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.RemoveElement(SelectedGroupedRc);

            GroupedSelection<RecipeCategory> selectedGrc = SelectedGroupedRc;
            ObservableCollection<GroupedSelection<RecipeCategory>> grc = GroupedRc;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedGrc, ref grc);
            SelectedGroupedRc = selectedGrc;
            GroupedRc = grc;

            SearchableListView_GroupedRC.Focus();
        }

        private void Button_ViewCategories_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AllCategoriesPage(_parent));
        }

        private void Button_EvaluateMissingRecipes_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_EvaluateMissingProducts_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();

            //Todo Find some way to allow the user to re-select the contents of a grouped recipe category, perhaps I need a new type of massedit page?!
            //< ItemsControl ItemTemplate="{StaticResource DataTemplateGroupedRecipeCategory}" ItemsSource="{Binding GroupedRecipeCategories}" >


            //if (SelectedGroupedPC == null || _parent.Data.AllProducts.IsNullOrEmpty()) return;

            //_parent.SetPage(new MassEditElementsPage(
            //    _parent,
            //    "Evaluate All Products By Type",
            //    $"Product type: {FullItemDescriptor.GetDescription(SelectedGroupedPC)}",
            //    "Should the product contain the type?", 
            //    _parent.Data.AllProducts,
            //    o =>
            //    {
            //        var p = (Product) o;
            //        return $"{FullItemDescriptor.GetDescription(p)}\n\n" +
            //               $"{(p.GroupedCategories.Contains(SelectedGroupedPC))}"
            //    }
            //    ));




            //if (SelectedProductCategory == null || _parent.Data.AllProducts.IsNullOrEmpty()) return;

            //_parent.SetPage(new MassEditElementsPage(
            //    _parent,
            //    "Evaluate All Products By Category",
            //    $"Product Category: {SelectedProductCategory}",
            //    $"Should the product contain the category {SelectedProductCategory.Name}?",
            //    _parent.Data.AllProducts.ConvertAll(p => (object)p),
            //    o =>
            //    {
            //        var p = (Product)o;
            //        return $"{FullItemDescriptor.GetDescription(p)}\n\n" +
            //               $"{(p.Categories.Contains(SelectedProductCategory) ? "Product already contains the category." : "Product does not contain the category.")}\n";
            //    },

            //    o =>
            //    {
            //        var p = (Product)o;
            //        if (!p.Categories.Contains(SelectedProductCategory)) p.Categories.Add(SelectedProductCategory);
            //    },

            //    o =>
            //    {
            //        var p = (Product)o;
            //        if (p.Categories.Contains(SelectedProductCategory)) p.Categories.Remove(SelectedProductCategory);
            //    }));
        }
    }
}
