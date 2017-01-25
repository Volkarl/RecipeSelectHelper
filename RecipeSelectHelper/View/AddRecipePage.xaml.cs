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
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page
    {
        private MainWindow _parent;
        public AddRecipePage(MainWindow parent)
        {
            _parent = parent;

            // I Will only load new data straight into the parent's data. It's the other page's individual jobs to check for changes?

            this.Loaded += AddRecipePage_Loaded;
            InitializeComponent();
        }

        private void AddRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            AddChildrenToWrapPanels();
            CheckBox_ProductCanExpire.IsChecked = true;
        }

        private void AddChildrenToWrapPanels()
        {
            // this.StackPanel_ProductInfoTextBox.Children   (as expander . children. wrappanel. elements?)

        }

        private void Button_AddProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (_parent.ContentControl.NavigationService.CanGoBack)
            {
                _parent.ContentControl.NavigationService.GoBack();
            }
        }

        private void CheckBox_ProductCanExpire_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckBox_ProductCanExpire.IsChecked.Value)
            {
                StackPanel_ExpirationInfo.Visibility = Visibility.Visible;
            }
            else
            {
                StackPanel_ExpirationInfo.Visibility = Visibility.Collapsed;
            }
        }
    }
}
