using RecipeSelectHelper.Model;
using System;
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

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for AddElementBasePage.xaml
    /// </summary>
    public partial class AddElementBasePage : Page
    {
        private Page _content;
        private string _title;
        private MainWindow _parent;

        public AddElementBasePage(Page content, string title, MainWindow parent)
        {
            _content = content;
            _title = title;
            _parent = parent;

            Loaded += AddElementBasePage_Loaded;
            InitializeComponent();
        }

        private void AddElementBasePage_Loaded(object sender, RoutedEventArgs e)
        {
            var addItemPage = _content as IAddElement;
            if (addItemPage != null)
            {
                this.Button_Add.Click -= addItemPage.AddItem; 
                // This is done to remove old (duplicate) subscriptions caused by clicking the BACK-button and navigating to a new version of the page.
                this.Button_Add.Click += addItemPage.AddItem;
            }

            this.TextBlock_PageTitle.Text = _title;
            this.ContentControl_AddNewItem.Content = _content;
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (_parent.ContentControl.NavigationService.CanGoBack)
            {
                _parent.ContentControl.NavigationService.GoBack();
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
