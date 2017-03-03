﻿using RecipeSelectHelper.Model;
using RecipeSelectHelper.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.View.BoughtProducts;
using AllCategoriesPage = RecipeSelectHelper.View.Categories.AllCategoriesPage;
using AllRecipesPage = RecipeSelectHelper.View.Recipes.AllRecipesPage;
using AllSortingMethodsPage = RecipeSelectHelper.View.SortingMethods.AllSortingMethodsPage;
using AllStoreProductsPage = RecipeSelectHelper.View.Products.AllStoreProductsPage;
using RankingsViewPage = RecipeSelectHelper.View.Miscellaneous.RankingsViewPage;
using SettingsPage = RecipeSelectHelper.View.Miscellaneous.SettingsPage;

namespace RecipeSelectHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            Loaded += MainWindow_Loaded1;
            DataContext = this;
            InitializeComponent();
        }

        public ProgramData Data { get; set; }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            var xmlReader = new XMLDataHandler();
            Data = xmlReader.FromXML();

            SetPage(new RankingsViewPage(this));
            HighlightButtonBackground(Button_RankRecipes);

            //Data.AllRecipes = new List<Recipe> { new Recipe("Antipasta", categories: (new List<RecipeCategory> { new RecipeCategory("Tomatoes"), new RecipeCategory("Fish") })) };
        }

        public void SetPage(Page newpage)
        {
            this.ContentControl.Content = newpage;
        }

        private void HighlightButtonBackground(Button button)
        {
            ClearOtherButtons();
            button.Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void ClearOtherButtons()
        {
            SolidColorBrush defaultColor = new SolidColorBrush(Colors.LightGray);
            Button_RankRecipes.Background = defaultColor;
            Button_FridgeIngredients.Background = defaultColor;
            Button_AllRecipes.Background = defaultColor;
            Button_AllStoreProducts.Background = defaultColor;
            Button_AllSortingMethods.Background = defaultColor;
            Button_AllCategories.Background = defaultColor;
            Button_Settings.Background = defaultColor;
        }

        private void Button_RankRecipes_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new RankingsViewPage(this));
            HighlightButtonBackground(sender as Button);
        }

        private void Button_FridgeIngredients_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new AllBoughtProductsPage(this));
            HighlightButtonBackground(sender as Button);
        }

        private void Button_AllRecipes_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new AllRecipesPage(this));
            HighlightButtonBackground(sender as Button);
        }

        private void Button_AllStoreProducts_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new AllStoreProductsPage(this));
            HighlightButtonBackground(sender as Button);
        }

        private void Button_AllSortingMethods_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new AllSortingMethodsPage(this));
            HighlightButtonBackground(sender as Button);
        }

        private void Button_AllCategories_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new AllCategoriesPage(this));
            HighlightButtonBackground(sender as Button);
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new SettingsPage(this));
            HighlightButtonBackground(sender as Button);
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var xmlReader = new XMLDataHandler();
            xmlReader.SaveToXML(this.Data);
        }
    }
}
