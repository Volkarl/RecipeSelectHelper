using System;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.View.Recipes
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
