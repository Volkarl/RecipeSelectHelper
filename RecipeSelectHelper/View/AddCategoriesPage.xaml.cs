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
    /// Interaction logic for AddCategoriesPage.xaml
    /// </summary>
    public partial class AddCategoriesPage : Page, IAddElement
    {
        public enum CategoryMode
        {
            ProductCategory, RecipeCategory
        }
        private CategoryMode _mode;

        public AddCategoriesPage(CategoryMode mode)
        {
            _mode = mode;
            InitializeComponent();
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
