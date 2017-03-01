using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Recipes
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page, IAddElement
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
            ItemsControl_Categories.Items.Clear();
            ItemsControl_Ingredients.Items.Clear();

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
                var textBox = new IntegerTextBox();
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
        }

        // REPLACE WITH A LISTVIEW AND A SEARCH BAR AT SOME POINT!

        private List<Ingredient> GetCheckedIngredients()
        {
            var ingredients = new List<Ingredient>();
            var checkedProducts = new List<Product>();
            var indexList = new List<int>();
            var ingredientAmountsNeeded = new List<int>();

            for (int i = 0; i < ItemsControl_Ingredients.Items.Count; i++)
            {
                StackPanel content = ItemsControl_Ingredients.Items[i] as StackPanel;
                if (((CheckBox)(content.Children[0])).IsChecked.Value)
                {
                    indexList.Add(i);
                    var checkedContent = content.Children[1] as StackPanel;
                    IntegerTextBox amountNeededTextBox = checkedContent.Children[1] as IntegerTextBox;
                    int amountNeeded = Int32.Parse(amountNeededTextBox.Text);
                    ingredientAmountsNeeded.Add(amountNeeded);
                }
            }
            foreach (int index in indexList)
            {
                checkedProducts.Add(_parent.Data.AllProducts[index]);
            }

            for (int i = 0; i < checkedProducts.Count; i++)
            {
                ingredients.Add(new Ingredient(ingredientAmountsNeeded[i], checkedProducts[i]));
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
            AddChildrenToWrapPanels();
        }

        private void Button_AddCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new Resources.AddElementBasePage(new Categories.AddCategoriesPage(_parent, Categories.AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category", _parent));
        }

        private void Button_AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new Resources.AddElementBasePage(new Products.AddStoreProductPage(_parent), "Add New Store Product", _parent));
        }

        public void AddItem(object sender, RoutedEventArgs e)
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

            // ADD ERROR HANDLING IN RECIPE AND OTHER (Name != null or empty) (Need to be unique)
        }
    }
}
