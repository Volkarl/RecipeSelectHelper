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
            for (int i = 0; i < 9; i++)
            {
                var checkBox = new CheckBox();
                checkBox.Content = "Object " + i;
                checkBox.Margin = new Thickness(4);
                ItemsControl_Ingredients.Items.Add(checkBox);
                var newCheckBox = new CheckBox();
                newCheckBox.Content = checkBox.Content;
                newCheckBox.Margin = checkBox.Margin;
                ItemsControl_Categories.Items.Add(newCheckBox);
            }
        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            if (_parent.ContentControl.NavigationService.CanGoBack)
            {
                _parent.ContentControl.NavigationService.GoBack();
            }
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

        private void Button_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            string name = TextBox_RecipeName.Text;
            string description = TextBox_RecipeDescription.Text;
            string instruction = TextBox_RecipeInstruction.Text;
            var categories = new List<RecipeCategory>();
            var ingredients = new List<Ingredient>();
            var expirationInfo = new ExpirationInfo();

            var recipe = new Recipe(name, description, instruction, ingredients, categories);

            ClearUIElements();


            // REMOVE PRODUCT EXPIRATION AND PUT IT WHERE IT BELONGS (IE NOT IN ADD RECIPE)
            // ADD ERROR HANDLING HERE
            // SYNCHRONIZE THE LABELS IN THE LEFT STACKPANEL WITH THE ELEMENTS IN THE MIDDLE ONE (Might as well do the same with the right ones anyhow)
        }

        private void ClearUIElements()
        {
            throw new NotImplementedException();
        }
    }
}
