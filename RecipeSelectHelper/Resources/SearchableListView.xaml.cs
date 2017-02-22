using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

namespace RecipeSelectHelper.Resources
{
    /// <summary>
    /// Interaction logic for SearchableListView.xaml
    /// </summary>
    public partial class SearchableListView : UserControl
    {
        private Action<object, MouseButtonEventArgs> _onMouseDoubleClick;
        public SearchableListView()
        {
            _onMouseDoubleClick = (x, y) => { };
            Loaded += SearchableListView_Loaded;
            InitializeComponent();
        }

        private void SearchableListView_Loaded(object sender, RoutedEventArgs e)
        {
            InnerListView = ListView_Items;
            InnerTextBox = TextBox_SearchParameter;
        }

        public ListView InnerListView { get; set; }
        public TextBox InnerTextBox { get; set; }

        private bool ItemIsSelected<T>(T item)
        {
            return ListView_Items.SelectedItems.Contains(item);
        }

        public void InitializeSearchableListView<T>(Action<T, MouseButtonEventArgs> onMouseDoubleClick, 
            Func<List<T>> getNewItemsSource, Func<string, List<T>, List<T>> sortItemsSource, string displayMemberPath, Func<T, string, bool> filterFunc) where T : class
        {
            _onMouseDoubleClick = (x,y) => onMouseDoubleClick(x as T, y);
            ListView_Items.DisplayMemberPath = displayMemberPath;
            ListView_Items.ItemsSource = getNewItemsSource();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListView_Items.ItemsSource);
            view.Filter = o => ItemIsSelected(o) || filterFunc(o as T, TextBox_SearchParameter.Text);
            //TextBox_SearchParameter.KeyDown += (x,e) => { RefillItemsSource(e, getNewItemsSource, sortItemsSource); };
        }
        

        //private void RefillItemsSource<T>(KeyEventArgs e, Func<List<T>> getNewItemsSource, Func<string, List<T>, List<T>> sortItemsSource)
        //{
        //    if (e.Key != Key.Enter) return;

        //    IList selectedItems = ListView_Items.SelectedItems ?? new List<T>(); //Saves the currently selected items 

        //    List<T> itemsList = getNewItemsSource();
        //    itemsList = sortItemsSource(TextBox_SearchParameter.Text, itemsList);
        //    itemsList = AddMissingMembers(itemsList, selectedItems as List<T>); //Adds the old selected items onto the back of the now sorted list of items 

        //    ListView_Items.ItemsSource = itemsList;

        //    foreach (object item in selectedItems)
        //    {
        //        ListView_Items.SelectedItems.Add(item); //Marks all selected items as being selected
        //    }
        //}

        //private List<T> AddMissingMembers<T>(List<T> collection1, List<T> collection2)
        //{
        //    collection2 = collection2 ?? new List<T>();
        //    List<T> missingMembers = new List<T>();
        //    foreach (T item in collection2)
        //    {
        //        if (!collection2.Contains(item))
        //        {
        //            missingMembers.Add(item);
        //        }
        //    }
        //    collection1.AddRange(missingMembers);
        //    return collection1;
        //}

        //if (!HasProperty(typeT, propertyToFilterBy)) throw new ArgumentException("Property to sort by is not contained in " + typeT);
        //private bool HasProperty(Type obj, string propertyName)
        //{
        //    return obj.GetProperty(propertyName) != null;
        //}

        private void ListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _onMouseDoubleClick(sender, e); 
        }

        private void TextBox_SearchParameter_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            CollectionViewSource.GetDefaultView(ListView_Items.ItemsSource).Refresh();

            //var selectedItems = ListView_Items.SelectedItems;
            //ListView_Items.SelectedItems.Clear();
            //foreach (var selectedItem in selectedItems)
            //{
            //    ListView_Items.SelectedItems.Add(selectedItem);
            //}
        }
    }
}
