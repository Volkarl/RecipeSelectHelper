using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Miscellaneous
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

        private void Button_All_OnClick(object sender, RoutedEventArgs e)
        {
            Button_AddPC_OnClick(null, null);
            Button_AddRC_OnClick(null, null);
            Button_AddP_OnClick(null, null);
            Button_AddBP_OnClick(null, null);
            Button_AddR_OnClick(null, null);
        }

        private void ButtonExportAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportAllRecipes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportAllProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportAllBoughtProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportAllSortingMethods_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportSelectedRecipes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportSelectedProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportSelectedBoughtProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExportSelectedSortingMethods_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonImportFromFile_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonImportPasteData_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExpanderExport_OnExpanded(object sender, RoutedEventArgs e)
        {
            ListViewExportOptions.Focus();
        }

        private void ButtonExportAsText_OnClick(object sender, RoutedEventArgs e)
        {
            ProgramData data = GetSelectedData();
            string dataAsXml = XmlDataHandler.XmlAsString(data);
            var popup = new CopyableTextWindow(dataAsXml);
            popup.ShowDialog();
        }

        private ProgramData GetSelectedData()
        {
            return _parent.Data;
        }

        private void ButtonExportSaveAsFile_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
