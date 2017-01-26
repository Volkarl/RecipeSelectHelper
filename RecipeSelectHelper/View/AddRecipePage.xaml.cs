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
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page
    {
        private MainWindow _parent;
        public AddRecipePage(MainWindow parent)
        {
            _parent = parent;
            this.Loaded += AddRecipePage_Loaded;
            InitializeComponent();
        }

        private void AddRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            AddChildrenToWrapPanels();
        }

        private void AddChildrenToWrapPanels()
        {
            foreach (RecipeCategory category in _parent.Data.AllRecipeCategories)
            {
                var checkBox = new CheckBox();
                checkBox.Content = category.Name;
                checkBox.Margin = new Thickness(4);
                ItemsControl_Categories.Items.Add(checkBox);
            }

            foreach (Product ingredients in _parent.Data.AllProducts)
            {
                var checkBox = new CheckBox();
                checkBox.Content = ingredients.Name;
                checkBox.Margin = new Thickness(4);
                ItemsControl_Ingredients.Items.Add(checkBox);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (_parent.ContentControl.NavigationService.CanGoBack)
            {
                _parent.ContentControl.NavigationService.GoBack();
            }
        }

        private void Button_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            string name = TextBox_RecipeName.Text;
            string description = TextBox_RecipeDescription.Text;
            string instruction = TextBox_RecipeInstruction.Text;
            var categories = new List<RecipeCategory>();
            var ingredients = new List<Ingredient>();

            try
            {
                var recipe = new Recipe(name, description, instruction, ingredients, categories);
                _parent.Data.AllRecipes.Add(recipe);
                ClearUIElements();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // REMOVE PRODUCT EXPIRATION AND PUT IT WHERE IT BELONGS (IE NOT IN ADD RECIPE)
            // ADD ERROR HANDLING IN RECIPE (Name != null) (Need to be unique)
            // SYNCHRONIZE THE LABELS IN THE LEFT STACKPANEL WITH THE ELEMENTS IN THE MIDDLE ONE (Might as well do the same with the right ones anyhow)
        }

        private void ClearUIElements()
        {
            TextBox_RecipeName.Text = string.Empty;
            TextBox_RecipeDescription.Text = string.Empty;
            TextBox_RecipeInstruction.Text = string.Empty;
        }
    }
}
