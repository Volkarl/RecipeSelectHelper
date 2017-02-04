using System;
using System.Collections.Generic;
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

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CategoriesPage(MainWindow parent)
        {
            this._parent = parent;
            DataContext = this;
            InitializeComponent();
        }

        private void Button_AddProductCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(AddCategoriesPage.CategoryMode.ProductCategory), "Add New Product Category", _parent);
        }

        private void Button_EditProductCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_RemoveProductCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_AddRecipeCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.ContentControl.Content = new AddElementBasePage(new AddCategoriesPage(AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category", _parent);
        }

        private void Button_EditRecipeCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_RemoveRecipeCategory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
