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
                var stackPanel = new StackPanel();
                var checkBox = new CheckBox();
                var checkedContent = new StackPanel();
                var label1 = new Label();
                var textBox = new TextBox();
                var label2 = new Label();

                checkBox.Content = ingredients.Name;
                checkBox.Margin = new Thickness(4);

                label1.Content = "Amount: ";
                textBox.Width = 40;
                label2.Content = " kg.";

                checkedContent.Orientation = Orientation.Horizontal;
                checkedContent.Children.Add(label1);
                checkedContent.Children.Add(textBox);
                checkedContent.Children.Add(label2);
                checkedContent.Visibility = Visibility.Collapsed;

                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(checkBox);
                stackPanel.Children.Add(checkedContent);

                checkBox.Checked += IngredientSelected;
                checkBox.Unchecked += IngredientSelected;

                ItemsControl_Ingredients.Items.Add(stackPanel);
            }
        }

        private void IngredientSelected(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            StackPanel stackPanel = checkBox.Parent as StackPanel;
            StackPanel checkedContent = stackPanel.Children[1] as StackPanel;

            if (checkBox.IsChecked.Value)
            {
                checkedContent.Visibility = Visibility.Visible;
            }
            else
            {
                checkedContent.Visibility = Visibility.Collapsed;
            }

            // Figure out how to do it horizontally? Its not pretty as it looks right now.
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
            List<RecipeCategory> categories = GetCheckedCategories();
            List<Ingredient> ingredients = GetCheckedIngredients();

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

            // ADD ERROR HANDLING IN RECIPE AND OTHER (Name != null) (Need to be unique)
        }

        private List<Ingredient> GetCheckedIngredients()
        {
            var ingredients = new List<Ingredient>();
            var checkedProducts = new List<Product>();
            var indexList = new List<int>();



            // THIS PROBABLY NO LONGER WORKS!!!! BECAUSE CHECKBOX IS ELEMENT 0 INSIDE ONE STACKPANEL

            for (int i = 0; i < ItemsControl_Ingredients.Items.Count; i++)
            {
                if (((CheckBox)(ItemsControl_Ingredients.Items[i])).IsChecked.Value)
                {
                    indexList.Add(i);
                }
            }
            foreach (int index in indexList)
            {
                checkedProducts.Add(_parent.Data.AllProducts[index]);
            }

            foreach (Product product in checkedProducts)
            {
                ingredients.Add(new Ingredient(0, product)); // HERE IT NEEDS TO TAKE THE CORRECT VALUE
            }
            return ingredients;
        }

        private List<RecipeCategory> GetCheckedCategories()
        {
            var categories = new List<RecipeCategory>();
            var indexList = new List<int>();

            for (int i = 0; i < ItemsControl_Categories.Items.Count; i++)
            {
                if (((CheckBox)(ItemsControl_Categories.Items[i])).IsChecked.Value)
                {
                    indexList.Add(i);
                }
            }
            foreach (int index in indexList)
            {
                categories.Add(_parent.Data.AllRecipeCategories[index]);
            }
            return categories;
        }

        private void ClearUIElements()
        {
            TextBox_RecipeName.Text = string.Empty;
            TextBox_RecipeDescription.Text = string.Empty;
            TextBox_RecipeInstruction.Text = string.Empty;
        }

        private void Button_AddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_AddIngredient_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
