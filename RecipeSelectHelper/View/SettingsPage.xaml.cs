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
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private MainWindow _parent;
        public SettingsPage(MainWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private static int _pcClicks = 0;
        private void Button_AddPC_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.AllProductCategories.Add(new ProductCategory("PCat" + _pcClicks++));
        }

        private static int _rcClicks = 0;
        private void Button_AddRC_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Data.AllRecipeCategories.Add(new RecipeCategory("RCat" + _rcClicks++));
        }

        private static int _pClicks = 0;
        private void Button_AddP_OnClick(object sender, RoutedEventArgs e)
        {
            List<ProductCategory> pc = new List<ProductCategory>();
            List<Product> subList = new List<Product>();
            var rand = new Random();
            if (_parent.Data.AllProductCategories.Count > 0)
            {
                pc.Add(_parent.Data.AllProductCategories[rand.Next(0, _parent.Data.AllProductCategories.Count)]);
            }
            if (_parent.Data.AllProducts.Count > 0)
            {
                subList.Add(_parent.Data.AllProducts[rand.Next(0, _parent.Data.AllProducts.Count)]);
            }
            _parent.Data.AllProducts.Add(new Product("Product" + _pClicks++, pc, subList));
        }

        private void Button_AddBP_OnClick(object sender, RoutedEventArgs e)
        {
            if(_parent.Data.AllProducts.Count == 0) return;

            var rand = new Random();
            var p = _parent.Data.AllProducts[rand.Next(0, _parent.Data.AllProducts.Count)];
            var exp = new ExpirationInfo();
            _parent.Data.AllBoughtProducts.Add(new BoughtProduct(p, exp));
        }

        private static int _rClicks = 0;
        private void Button_AddR_OnClick(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            var iList = new List<Ingredient>();
            var rcList = new List<RecipeCategory>();
            if (_parent.Data.AllProducts.Count > 0)
            {
                var corrP = _parent.Data.AllProducts[rand.Next(0, _parent.Data.AllProducts.Count)];
                iList.Add(new Ingredient(_rClicks, corrP));
            }
            if (_parent.Data.AllRecipeCategories.Count > 0)
            {
                rcList.Add(_parent.Data.AllRecipeCategories[rand.Next(0, _parent.Data.AllRecipeCategories.Count)]);
            }
            var r = new Recipe("Recipe" + _rClicks++, "Lorem", "Ipsum", iList, rcList);
            _parent.Data.AllRecipes.Add(r);
        }
    }
}
