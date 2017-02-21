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

        public void InitializeSearchableListView<T>(Action<T, MouseButtonEventArgs> onMouseDoubleClick, 
            Func<List<T>> getNewItemsSource, Func<string, List<T>, List<T>> sortItemsSource, string displayMemberPath) where T : class
        {
            _onMouseDoubleClick = (x,y) => onMouseDoubleClick(x as T, y);
            ListView_Items.ItemsSource = getNewItemsSource();
            ListView_Items.KeyDown += (x,e) => { RefillItemsSource(e, getNewItemsSource, sortItemsSource); };
            ListView_Items.DisplayMemberPath = displayMemberPath;
        }

        private void RefillItemsSource<T>(KeyEventArgs e, Func<List<T>> getNewItemsSource, Func<string, List<T>, List<T>> sortItemsSource)
        {
            if (e.Key == Key.Enter)
            {
                var itemsList = getNewItemsSource();
                itemsList = sortItemsSource(TextBox_SearchParameter.Text, itemsList);
                ListView_Items.ItemsSource = itemsList;
            }
        }

        //if (!HasProperty(typeT, propertyToFilterBy)) throw new ArgumentException("Property to sort by is not contained in " + typeT);
        //private bool HasProperty(Type obj, string propertyName)
        //{
        //    return obj.GetProperty(propertyName) != null;
        //}

        private void ListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _onMouseDoubleClick(sender, e); 
        }
    }
}
