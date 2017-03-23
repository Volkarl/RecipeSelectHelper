using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Properties;
using RecipeSelectHelper.Resources;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Forms.Clipboard;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public SettingsPage(MainWindow parent)
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

        #endregion

        private void ButtonImportFromFile_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonImportPasteData_OnClick(object sender, RoutedEventArgs e)
        {
            string dataAsString = Clipboard.GetText();
            ProgramData importedData = XmlDataHandler.FromXmlString(dataAsString);
            var merge = MergePage.TryMerge(_parent.Data, importedData);
            _parent.ContentControl.Content = new AddElementBasePage(conflictPage, "Resolve conflicts")

            // when showdialog returns -> check property on window
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

        private void ButtonExitWithoutSaving_OnClick(object sender, RoutedEventArgs e)
        {
            _parent.SaveChangesOnExit = false;
            Application.Current.Shutdown();
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
    }
}
