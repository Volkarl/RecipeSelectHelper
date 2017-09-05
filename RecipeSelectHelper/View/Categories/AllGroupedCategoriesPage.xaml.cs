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
using System.Windows.Controls.Primitives;
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

        enum SelectedListView
        {
            GroupedProductCategory,
            GroupedRecipeCategory,
            None
        }
        
        //private SelectedListView ActiveSelection
        //{
        //    get
        //    {
        //        var pc = SearchableListView_GroupedPC.GetValue(Selector.IsSelectionActiveProperty);
        //        if(pc != null && pc.Equals(true)) return SelectedListView.GroupedProductCategory;
        //        var rc = SearchableListView_GroupedRC.GetValue(Selector.IsSelectionActiveProperty);
        //        if(rc != null && rc.Equals(true)) return SelectedListView.GroupedRecipeCategory;
        //        return SelectedListView.None;
        //    }
        //}

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
            _parent.SetPage(new AddElementBasePage(new AddGroupedProductCategoryPage(_parent), "Add New Product Type", _parent));
        }

        private void Button_EditGroupedPC_OnClick(object sender, RoutedEventArgs e)
        {
            //switch (ActiveSelection)
            //{
            //    case SelectedListView.GroupedProductCategory:
            //        throw new NotImplementedException();
            //        break;
            //    case SelectedListView.GroupedRecipeCategory:
            //        throw new NotImplementedException();
            //        break;
            //    case SelectedListView.None:
            //        throw new NotImplementedException();
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        private void Button_RemoveGroupedPC_OnClick(object sender, RoutedEventArgs e)
        {
            PcRemoveElement();
            //switch (ActiveSelection)
            //{
            //    case SelectedListView.GroupedProductCategory:
            //        PcRemoveElement();
            //        break;
            //    case SelectedListView.GroupedRecipeCategory:
            //        RcRemoveElement();
            //        break;
            //    case SelectedListView.None:
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException();
            //}
        }

        private void RcRemoveElement()
        {
            _parent.Data.RemoveElement(SelectedGroupedRc);

            GroupedSelection<RecipeCategory> selectedGrc = SelectedGroupedRc;
            ObservableCollection<GroupedSelection<RecipeCategory>> grc = GroupedRc;
            ListViewTools.RemoveElementAndSelectPrevious(ref selectedGrc, ref grc);
            SelectedGroupedRc = selectedGrc;
            GroupedRc = grc;

            SearchableListView_GroupedRC.Focus();
        }

        private void PcRemoveElement()
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
            _parent.SetPage(new AddElementBasePage(new AddGroupedRecipeCategoryPage(_parent), "Add New Recipe Type", _parent));
        }

        private void Button_EditGroupedRC_OnClick(object sender, RoutedEventArgs e)
        {
            //switch here as well
        }

        private void Button_RemoveGroupedRC_OnClick(object sender, RoutedEventArgs e)
        {
            RcRemoveElement();
        }

        private void Button_ViewCategories_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AllCategoriesPage(_parent), addNavigationEvent:false);
        }

        private void Button_EvaluateMissingRecipes_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Button_EvaluateMissingProducts_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedGroupedPC == null || _parent.Data.AllProducts.IsNullOrEmpty()) return;

            //rename to massedit, not massadd
            _parent.SetPage(new MassAddGroupedCategoriesPage(
                _parent,
                "Evaluate All Products By Type",
                $"Product type: {FullItemDescriptor.GetDescription(SelectedGroupedPC)}",
                _parent.Data.AllProducts.ConvertAll(p => (object) p),
                _parent.Data.AllProducts.ConvertAll(p => (object) new Product(p.Name, p.Categories, p.GroupedCategories.ConvertAll(CloneGpc))),
                // The above is done to have a product item that can be modified in the UI without it changing the original item until the changes are applied.
                o =>
                {
                    var p = (Product) o;
                    return $"{FullItemDescriptor.GetDescription(p)}\n\n" +
                           $"Product {(p.GroupedCategories.Any(gpc => gpc.CorrespondingGroupedSelection == SelectedGroupedPC) ? "already contains" : "does not contain")} type.";
                },
                o =>
                {
                    var p = (Product) o;
                    GroupedProductCategory gpc = p?.GroupedCategories?.Find(x => x.CorrespondingGroupedSelection == SelectedGroupedPC) ?? new GroupedProductCategory(SelectedGroupedPC);
                    return new ContentControl { ContentTemplate = Application.Current.Resources["DataTemplateGroupedProductCategory"] as DataTemplate, Content = gpc };
                },
                ui => ui.Content as GroupedProductCategory,
                (o, r) =>
                {
                    var p = (Product) o;
                    var gpc = p.GroupedCategories.Find(x => x.CorrespondingGroupedSelection == SelectedGroupedPC);
                    var editedGpc = (GroupedProductCategory) r.ModifiedUi.Content;
                    if (gpc == null) p.GroupedCategories.Add(editedGpc);
                    else gpc.GroupedPc = editedGpc.GroupedPc;
                },
                o => ((GroupedProductCategory) o).SelectionIsValid()
                ));
        }

        private GroupedProductCategory CloneGpc(GroupedProductCategory input)
        {
            var result = new GroupedProductCategory(input.CorrespondingGroupedSelection);
            for (var i = 0; i < input.GroupedPc.Count; i++)
                result.GroupedPc[i].Bool = input.GroupedPc[i].Bool;
            return result;
        }
    }
}
