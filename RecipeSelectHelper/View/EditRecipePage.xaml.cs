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
    /// Interaction logic for EditRecipePage.xaml
    /// </summary>
    public partial class EditRecipePage : Page, IAddElement //rename the interface and its method
    {
        private Recipe _recipeToEdit;

        public EditRecipePage(Recipe recipeToEdit)
        {
            _recipeToEdit = recipeToEdit;
            this.Loaded += EditRecipePage_Loaded;
            InitializeComponent();
        }

        private void EditRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
            // Same as from addstoreproductpage
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            //_recipeToEdit.Categories = 
            // And so on...
        }
    }
}
