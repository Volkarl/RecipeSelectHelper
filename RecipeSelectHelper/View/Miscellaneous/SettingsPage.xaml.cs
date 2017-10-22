using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Properties;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Forms.Clipboard;
using MessageBox = System.Windows.MessageBox;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page, INotifyPropertyChanged
    {
        private IParentPage _parent;

        public SettingsPage(IParentPage parent)
        {
            _parent = parent;
            Loaded += SettingsPage_Loaded;
            InitializeComponent();
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        public string SaveFilePath
        {
            get { return Settings.Default.DataDirectoryPath; }
            set
            {
                if (UtilityMethods.DirectoryPathIsValid(value))
                {
                    Settings.Default.DataDirectoryPath = value;
                    OnPropertyChanged(nameof(SaveFilePath));
                }
            }
        }


        //todo delete this when done testing
        private ObservableCollection<Recipe> _recipes = new ObservableCollection<Recipe>
        {
            new Recipe("Tomato Juice", 1, "Dont drink, danger!", new StringList("Mash tomatoes", "Throw in the trash"), 
                new List<Ingredient>
                {
                    new Ingredient(new Amount(10) , new Product("Tomato", new List<ProductCategory> {new ProductCategory("Vegetable"), new ProductCategory("Pretty disgusting.")})),
                    new Ingredient(new Amount(20), new Product("Water", new List<ProductCategory> {new ProductCategory("Tasteless things")}))
                }, new List<RecipeCategory> {new RecipeCategory("Not ingestible")})
        };
        public ObservableCollection<Recipe> Recipes
        {
            get { return _recipes; }
            set { _recipes = value; OnPropertyChanged(nameof(Recipes)); }
        }


        #endregion

        #region Testing

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
            Product p = new Product("Product" + _pClicks++, pc);
            _parent.Data.AllProducts.Add(p);
            _parent.Data.ProductSubstitutes.AddSubstitutes(p, subList);

        }

        private void Button_AddBP_OnClick(object sender, RoutedEventArgs e)
        {
            if(_parent.Data.AllProducts.Count == 0) return;

            var rand = new Random();
            var p = _parent.Data.AllProducts[rand.Next(0, _parent.Data.AllProducts.Count)];
            uint amount = Convert.ToUInt32(rand.Next(0, 200));
            var exp = new ExpirationInfo();
            _parent.Data.AllBoughtProducts.Add(new BoughtProduct(p, amount, exp));
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
                iList.Add(new Ingredient(new Amount(_rClicks), corrP));
            }
            if (_parent.Data.AllRecipeCategories.Count > 0)
            {
                rcList.Add(_parent.Data.AllRecipeCategories[rand.Next(0, _parent.Data.AllRecipeCategories.Count)]);
            }
            var r = new Recipe("Recipe" + _rClicks++, 1, "Lorem", new StringList("Ipsum"), iList, rcList);
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

        #endregion

        private void ButtonImportFromFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = @"Xml files (*.xml)|*.xml|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ProgramData importedData = XmlDataHandler.FromXml(dialog.FileName);
                _parent.Data.Merge(importedData);
            }
        }

        private void ButtonImportPasteData_OnClick(object sender, RoutedEventArgs e)
        {
            string dataAsString = Clipboard.GetText();
            ProgramData importedData = XmlDataHandler.FromXmlString(dataAsString);
            _parent.Data.Merge(importedData);
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
            ProgramData data = new ProgramData();
            switch (ListViewExportOptions.SelectedIndex)
            {
                case 0: data = _parent.Data;
                    break;
                case 1: data = GetRecipeData(_parent.Data);
                    break;
                case 2: data = GetStoreProductData(_parent.Data);
                    break;
                case 3: data = GetBoughtProductData(_parent.Data);
                    break;
                case 4: data = GetSortingMethodData(_parent.Data);
                    break;
            }
            return data;
        }

        private ProgramData GetSortingMethodData(ProgramData parentData)
        {
            throw new NotImplementedException();
        }

        private ProgramData GetBoughtProductData(ProgramData parentData)
        {
            throw new NotImplementedException();
        }

        private ProgramData GetStoreProductData(ProgramData parentData)
        {
            throw new NotImplementedException();
        }

        private ProgramData GetRecipeData(ProgramData parentData)
        {
            ProgramData data = new ProgramData();
            //foreach (Recipe recipe in parentData.AllRecipes)
            //{
            //    data.AllRecipes.Add(recipe);
            //    data.AllProducts.AddRange(recipe.Ingredients.ConvertAll(x => x.CorrespondingProduct));
            //    data.
            // HERE I WANTED TO USE .UNION TO THEN ENSURE THAT EACH PRODUCT/RECIPECATEGORY AND SUCH WAS ONLY IN THERE ONCE, EVEN IF
            // REFERENCED MULTIPLE TIMES!
            //}

            // FOllow it down the tree somehow? Make a specialized class for this

            data.AllRecipes = parentData.AllRecipes;

            
            throw new NotImplementedException();
        }

        private void ButtonExportSaveAsFile_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog {ShowNewFolderButton = true};
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlDataHandler.SaveToXml(UtilityMethods.AddDefaultFileName(dialog.SelectedPath), GetSelectedData());
            }
        }

        private void ButtonExitWithoutSaving_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.Shutdown(false);
        }

        private void ButtonChangeSaveLocation_OnClick(object sender, RoutedEventArgs e)
        {
            var browser = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.Desktop,
                ShowNewFolderButton = true
            };
            var result = browser.ShowDialog();
            if (result == DialogResult.OK) SaveFilePath = browser.SelectedPath;
        }

        private void ButtonSaveChanges_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SaveChanges();
        }
    }
}
