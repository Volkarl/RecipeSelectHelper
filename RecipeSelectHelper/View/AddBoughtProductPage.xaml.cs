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
    /// Interaction logic for AddBoughtProductPage.xaml
    /// </summary>
    public partial class AddBoughtProductPage : Page, IAddElement
    {
        private MainWindow _parent;

        public AddBoughtProductPage(MainWindow parent)
        {
            _parent = parent;
            this.Loaded += AddBoughtProductPage_Loaded;
            InitializeComponent();
        }

        private void AddBoughtProductPage_Loaded(object sender, RoutedEventArgs e)
        {
            CheckBox_ProductCanExpire.IsChecked = true;
        }

        private void CheckBox_ProductCanExpire_Toggled(object sender, RoutedEventArgs e)
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



        public void AddItem(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Success");
        }
    }
}
