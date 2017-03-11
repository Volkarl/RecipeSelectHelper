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

        public void InitializeSearchableListView<T>(List<T> itemsSource, string displayMemberPath, 
            Func<T, string, bool> filterFunc, Action<T> onMouseDoubleClick = null) where T : class
        {
            if (onMouseDoubleClick == null)
            {
                _onMouseDoubleClick = (sender,e) => DisplayContent<T>(sender as ListViewItem);
            }
            else
            {
                _onMouseDoubleClick = (sender,e) => DisplayContent(sender as ListViewItem, onMouseDoubleClick);
            }
            ListView_Items.DisplayMemberPath = displayMemberPath;
            ListView_Items.ItemsSource = itemsSource;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListView_Items.ItemsSource);
            view.Filter = o => ItemIsSelected(o) || filterFunc(o as T, TextBox_SearchParameter.Text);
        }

        private void DisplayContent<T>(ListViewItem sender, Action<T> onMouseDoubleClick) where T : class 
        {
            if (sender == null || onMouseDoubleClick == null) return;
            onMouseDoubleClick(sender.Content as T);
        }

        private void DisplayContent<T>(ListViewItem sender) where T : class 
        {
            DisplayContent<T>(sender, x => MessageBox.Show(x.ToString()));
        }

        private void ListView_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _onMouseDoubleClick(sender, e); 
        }

        private void TextBox_SearchParameter_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) CollectionViewSource.GetDefaultView(ListView_Items.ItemsSource).Refresh();

            // Do I want to do something akin putting the selected itmes to the bottom?
            // If so, then I need the user to give the sort functions to the listview too.
            // Atm. it is dependant on the order of items never changing. 
        }

        public void ClearSelection()
        {
            InnerListView.UnselectAll();
        }

        public void DisplaySelectedItems()
        {
            string str = String.Empty;
            foreach (object item in InnerListView.SelectedItems)
            {
                str += item + "\n";
            }
            str = str.TrimEnd('\n');
            MessageBox.Show(str);
        }
    }
}
