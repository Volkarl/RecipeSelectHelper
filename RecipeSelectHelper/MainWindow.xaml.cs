using RecipeSelectHelper.Model;
using RecipeSelectHelper.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace RecipeSelectHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Loaded += MainWindow_Loaded1;
            InitializeComponent();
            SetPage(new RankingsViewPage());
        }

        public ProgramData Data { get; set; }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            Data = new ProgramData();
            Data.Load();
        }

        private void SetPage(Page newpage)
        {
            this.ContentControl.Content = newpage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetPage(new RankingsViewPage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetPage(new FridgePage());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetPage(new AllRecipesPage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SetPage(new AllStoreProductsPage());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            SetPage(new SortingMethodsPage());
        }
    }
}
