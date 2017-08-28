using RecipeSelectHelper.Model;
using RecipeSelectHelper.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecipeSelectHelper.Properties;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.View.BoughtProducts;
using RecipeSelectHelper.View.Miscellaneous;
using AllCategoriesPage = RecipeSelectHelper.View.Categories.AllCategoriesPage;
using AllRecipesPage = RecipeSelectHelper.View.Recipes.AllRecipesPage;
using AllSortingMethodsPage = RecipeSelectHelper.View.SortingMethods.AllSortingMethodsPage;
using AllStoreProductsPage = RecipeSelectHelper.View.Products.AllStoreProductsPage;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;
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

        public ProgramData Data { get; set; }
        public bool SaveChangesOnExit = true;
        private readonly List<string> _navigationHistory = new List<string>();
        public string NavigationHistory => string.Join(" > ", _navigationHistory);

        public MainWindow()
        {
            Loaded += MainWindow_Loaded1;
            InitializeComponent();
        }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            string path = GetSettingsFilePath();
            Data = File.Exists(path) ? XmlDataHandler.FromXml(path) : new ProgramData();
            if (Data.CompatibilityVersion != ProgramData.ProgramVersion) throw new ArgumentException("Loaded data is of wrong program version");
                //FixCompatibilityErrors(Data.CompatibilityVersion);
            SetPage(new RankingsViewPage(this));
            HighlightButtonBackground(Button_RankRecipes);
        }

        private string GetSettingsFilePath()
        {
            if (!UtilityMethods.DirectoryPathIsValid(Settings.Default.DataDirectoryPath))
            {
                Settings.Default.DataDirectoryPath = UtilityMethods.GetExeDirectoryPath();
                Settings.Default.Save();
            }
            return UtilityMethods.AddDefaultFileName(Settings.Default.DataDirectoryPath);
        }

        public void SetPage(AddElementBasePage newpage)
        {
            SetPage(newpage, newpage.ContentPageTitle);
        }

        public void SetPage(Page newpage, string title = null)
        {
            ContentControl.Content = newpage;
            AddNavigationPath(title ?? newpage.Title);
        }

        public void NavigatePageBack()
        {
            if (ContentControl.NavigationService.CanGoBack)
            {
                ContentControl.NavigationService.GoBack();
                RemovePreviousNavigationPath();
            }
        }

        private void RemovePreviousNavigationPath()
        {
            _navigationHistory.RemoveAt(_navigationHistory.Count - 1);
            OnPropertyChanged(nameof(NavigationHistory));
        }

        public void SetRootPage(Page newpage, Button highlightSpecificButton = null)
        {
            ClearNavigationPath();
            SetPage(newpage);
            if (highlightSpecificButton != null)
            {
                HighlightButtonBackground(highlightSpecificButton);
            }
            else
            {
                HighlightButtonBackground(newpage);
            }
        }

        private void AddNavigationPath(string path)
        {
            _navigationHistory.Add(path);
            OnPropertyChanged(nameof(NavigationHistory));
        }

        private void ClearNavigationPath()
        {
            _navigationHistory.Clear();
            OnPropertyChanged(nameof(NavigationHistory));
        }

        private void HighlightButtonBackground(Page page)
        {
            Button buttonToHighlight;
            if (page is RankingsViewPage) buttonToHighlight = Button_RankRecipes;
            else if (page is AllBoughtProductsPage) buttonToHighlight = Button_FridgeIngredients;
            else if (page is AllRecipesPage) buttonToHighlight = Button_AllRecipes;
            else if (page is AllStoreProductsPage) buttonToHighlight = Button_AllStoreProducts;
            else if (page is AllSortingMethodsPage) buttonToHighlight = Button_AllSortingMethods;
            else if (page is AllCategoriesPage) buttonToHighlight = Button_AllCategories;
            else if (page is SettingsPage) buttonToHighlight = Button_Settings;
            else throw new ArgumentException();
            HighlightButtonBackground(buttonToHighlight);
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
            SetRootPage(new RankingsViewPage(this), sender as Button);
        }

        private void Button_FridgeIngredients_Click(object sender, RoutedEventArgs e)
        {
            SetRootPage(new AllBoughtProductsPage(this), sender as Button);
        }

        private void Button_AllRecipes_Click(object sender, RoutedEventArgs e)
        {
            SetRootPage(new AllRecipesPage(this), sender as Button);
        }

        private void Button_AllStoreProducts_Click(object sender, RoutedEventArgs e)
        {
            SetRootPage(new AllStoreProductsPage(this), sender as Button);
        }

        private void Button_AllSortingMethods_Click(object sender, RoutedEventArgs e)
        {
            SetRootPage(new AllSortingMethodsPage(this), sender as Button);
        }

        private void Button_AllCategories_Click(object sender, RoutedEventArgs e)
        {
            SetRootPage(new AllCategoriesPage(this), sender as Button);
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            SetRootPage(new SettingsPage(this), sender as Button);
        }

        public void SaveChanges()
        {
            string path = UtilityMethods.AddDefaultFileName(Settings.Default.DataDirectoryPath);
            Settings.Default.Save();
            try
            {
                XmlDataHandler.SaveToXml(path, Data);
            }
            catch (Exception ex)
            {
                string defaultPath = UtilityMethods.AddDefaultFileName(UtilityMethods.GetExeDirectoryPath());
                XmlDataHandler.SaveToXml(defaultPath, Data);
                MessageBox.Show("Invalid save path: data instead saved at " + defaultPath, ex.Message);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(SaveChangesOnExit) SaveChanges();
            //if (!SaveChangesOnExit) return;

            //try
            //{
            //    SaveChanges();
            //}
            //catch (Exception)
            //{
            //    var result = MessageBox.Show(this, "Error occured while saving changes: continue closing the program?",
            //        "Error", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            //    e.Cancel = (result == MessageBoxResult.No);
            //}
        }
    }
}
