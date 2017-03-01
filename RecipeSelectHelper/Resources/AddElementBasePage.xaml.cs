using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    /// <summary>
    /// Interaction logic for AddElementBasePage.xaml
    /// </summary>
    public partial class AddElementBasePage : Page
    {
        private Page _content;
        private string _title;
        private MainWindow _parent;
        private object _finalizeButtonContent;

        // Rename this class maybe. Its no longer only for adding elements, but also for editing elements. 
        // Anything with a back button and a finalize button!
        public AddElementBasePage(Page content, string title, MainWindow parent, string contentOfFinalizeButton = "Add")
        {
            _content = content;
            _title = title;
            _parent = parent;
            _finalizeButtonContent = contentOfFinalizeButton;

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
            this.Button_Add.Content = _finalizeButtonContent;
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
