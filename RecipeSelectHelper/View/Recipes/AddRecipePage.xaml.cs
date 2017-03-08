using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.View.Categories;
using RecipeSelectHelper.View.Products;

namespace RecipeSelectHelper.View.Recipes
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        public AddRecipePage(MainWindow parent)
        {
            _parent = parent;
            Loaded += AddRecipePage_Loaded;
            InitializeComponent();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<GroupedRecipeCategory> _groupedRecipeCategories;
        public ObservableCollection<GroupedRecipeCategory> GroupedRecipeCategories
        {
            get { return _groupedRecipeCategories; }
            set { _groupedRecipeCategories = value; OnPropertyChanged(nameof(GroupedRecipeCategories)); }
        }

        private ObservableCollection<Boolable<RecipeCategory>> _recipeCategories;
        public ObservableCollection<Boolable<RecipeCategory>> RecipeCategories
        {
            get { return _recipeCategories; }
            set { _recipeCategories = value; OnPropertyChanged(nameof(RecipeCategories)); }
        }

        private ObservableCollection<BoolableWithValue<Product, int>> _ingredients;
        public ObservableCollection<BoolableWithValue<Product, int>> Ingredients
        {
            get { return _ingredients; }
            set { _ingredients = value; OnPropertyChanged(nameof(Ingredients)); }
        }

        #endregion


        private void AddRecipePage_Loaded(object sender, RoutedEventArgs e)
        {
            AddChildrenToWrapPanels();

            InitializeObservableObjects();
        }

        private void InitializeObservableObjects()
        {
            GroupedRecipeCategories = new ObservableCollection<GroupedRecipeCategory>(_parent.Data.AllGroupedRecipeCategories.ConvertAll(x => new GroupedRecipeCategory(x)));
            RecipeCategories = new ObservableCollection<Boolable<RecipeCategory>>(_parent.Data.AllRecipeCategories.ConvertAll(x => new Boolable<RecipeCategory>(x)));
            Ingredients = new ObservableCollection<BoolableWithValue<Product, int>>(_parent.Data.AllProducts.ConvertAll(x => new BoolableWithValue<Product, int>(x)));
        }

        private void AddChildrenToWrapPanels()
        {
            //StackPanel_GroupedCategories.Children.Clear();
            //ItemsControl_Categories.Items.Clear();
            //ItemsControl_Ingredients.Items.Clear();

            //foreach (GroupedSelection<RecipeCategory> groupedSelection in _parent.Data.AllGroupedRecipeCategories)
            //{
            //    var groupedRc = new GroupedRecipeCategory(groupedSelection);
            //    var wrapPanel = new WrapPanel();
            //    var label = new Label
            //    {
            //        Content =
            //        $"Select {groupedRc.CorrespondingGroupedSelection.MinSelect} to {groupedRc.CorrespondingGroupedSelection.MaxSelect} types: "
            //    };
            //    wrapPanel.Children.Add(label);
            //    for (int i = 0; i < groupedRc.CorrespondingGroupedSelection.GroupedItems.Count; i++)
            //    {
            //        RecipeCategory rc = groupedRc.CorrespondingGroupedSelection.GroupedItems[i];
            //        var checkBox = new CheckBox();
            //        checkBox.Content = rc.Name;
            //        checkBox.Margin = new Thickness(4);
            //        var i1 = i;
            //        checkBox.Checked += (sender, args) => groupedRc.SelectItem(i1);
            //        checkBox.Unchecked += (sender, args) => groupedRc.DeselectItem(i1);
            //        wrapPanel.Children.Add(checkBox);
            //    }
            //    _displayedGroupedRc.Add(groupedRc);
            //    StackPanel_GroupedCategories.Children.Add(wrapPanel);
            //}

            //foreach (RecipeCategory category in _parent.Data.AllRecipeCategories)
            //{
            //    var checkBox = new CheckBox();
            //    checkBox.Content = category.Name;
            //    checkBox.Margin = new Thickness(4);
            //    ItemsControl_Categories.Items.Add(checkBox);
            //}

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
            InitializeObservableObjects();
        }

        private void Button_AddCategory_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddCategoriesPage(_parent.Data, AddCategoriesPage.CategoryMode.RecipeCategory), "Add New Recipe Category", _parent));
        }

        private void Button_AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddStoreProductPage(_parent), "Add New Store Product", _parent));
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            string name = TextBox_RecipeName.Text;
            string description = TextBox_RecipeDescription.Text;
            string instruction = TextBox_RecipeInstruction.Text;
            List<GroupedRecipeCategory> groupedRc = GroupedRecipeCategories.ToList();



            List<RecipeCategory> categories = RecipeCategories.Where(x => x.Bool).ToList().ConvertAll(y => y.Instance); //GetCheckedCategories();
            List<Ingredient> ingredients = GetCheckedIngredients();

            string error;
            var valid = new ValidityChecker(_parent.Data);
            if (valid.RecipeNameIsValid(name, out error) &&
                valid.DescriptionIsValid(name, out error) &&
                valid.InstructionIsValid(name, out error) &&
                valid.GroupedRcAreValid(groupedRc, out error) &&
                valid.CategoriesAreValid(categories, out error) &&
                valid.IngredientsAreValid(ingredients, out error))
            {
                try
                {
                    var recipe = new Recipe(name, description, instruction, ingredients, categories, groupedRc);
                    _parent.Data.AllRecipes.Add(recipe);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ClearUIElements();
            }
            else
            {
                MessageBox.Show(error);
            }
        }

        private void Button_AddGroupedCategory_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SetPage(new AddElementBasePage(new AddGroupedRecipeCategoryPage(_parent), "Add New Recipe Types", _parent));
        }
    }
}
