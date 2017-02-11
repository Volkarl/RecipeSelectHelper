using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for SortingMethodsPage.xaml
    /// </summary>
    public partial class SortingMethodsPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SortingMethodsPage(MainWindow parent)
        {
            _parent = parent;
            DataContext = this;
            LoadObservableObjects();
            Loaded += SortingMethodsPage_Loaded;
            InitializeComponent();
        }

        private void LoadObservableObjects()
        {
            LoadPref1Settings();
            Pref1Value = String.Empty;
        }

        private void SortingMethodsPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        #region ObservableObjects

        private ObservableCollection<string> _pref1Settings;
        public ObservableCollection<string> Pref1Settings
        {
            get { return _pref1Settings; }
            set { _pref1Settings = value; OnPropertyChanged(nameof(Pref1Settings)); }
        }

        private string _pref1Value;
        public string Pref1Value
        {
            get { return _pref1Value; }
            set { _pref1Value = value; OnPropertyChanged(nameof(Pref1Value)); }
        }

        #endregion

        #region PreferenceChoices

        private enum PreferenceTopic
        {
            [Description("Product Category")]
            ByProductCategory,
            [Description("Recipe Category")]
            ByRecipeCategory,
            [Description("Specific Ingredients")]
            BySpecificIngredients,
            [Description("Ingredients Owned")]
            ByIngredientsOwned,
            [Description("Uses Expiring Food First")]
            ByExpirationDate,
        }

        private PreferenceTopic _pref1Choice;
        private PreferenceTopic Pref1Choice
        {
            get { return _pref1Choice; }
            set { _pref1Choice = value;
                UniformGrid_SelectedPreference.Children.RemoveRange(1, 2);
                LoadPref2Settings(value);
            }
        }

        #endregion

        #region LoadPreferenceChoices

        private void LoadPref1Settings()
        {
            var s = new List<string>
            {
                PreferenceTopic.ByProductCategory.GetDescription(),
                PreferenceTopic.ByRecipeCategory.GetDescription(),
                PreferenceTopic.BySpecificIngredients.GetDescription(),
                PreferenceTopic.ByIngredientsOwned.GetDescription(),
                PreferenceTopic.ByExpirationDate.GetDescription()
            };
            Pref1Settings = new ObservableCollection<string>(s);
        }

        private void LoadPref2Settings(PreferenceTopic value)
        {
            var ui1 = new UIElement();
            var ui2 = new UIElement();
            switch (value)
            {
                case PreferenceTopic.ByProductCategory:
                    ui1 = CreateComboBox(_parent.Data.AllProductCategories.ConvertAll(x => x.Name));
                    ui2 = new IntegerTextBox();
                    break;
                case PreferenceTopic.ByRecipeCategory:
                    ui1 = CreateComboBox(_parent.Data.AllRecipeCategories.ConvertAll(x => x.Name));
                    ui2 = new IntegerTextBox();
                    break;
                case PreferenceTopic.BySpecificIngredients:
                    ui1 = CreateComboBox(_parent.Data.AllProducts.ConvertAll(x => x.Name));
                    ui2 = new IntegerTextBox();
                    break;
                case PreferenceTopic.ByIngredientsOwned:
                    ui1 = new IntegerTextBox();
                    break;
                case PreferenceTopic.ByExpirationDate:
                    ui1 = new IntegerTextBox();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }

            UniformGrid_SelectedPreference.Children.Add(ui1);
            UniformGrid_SelectedPreference.Children.Add(ui2);
        }

        private ComboBox CreateComboBox(IEnumerable<String> items)
        {
            var cbx = new ComboBox
            {
                ItemsSource = new ObservableCollection<string>(items)
            };
            return cbx;
        }

        #endregion

        private void Button_FinalizePreference_Click(object sender, RoutedEventArgs e)
        {
            Preference pref = null;
            try
            {
                pref = InterpretPreference(Pref1Choice);
                AddPreference(pref);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            var label = new Label();
            label.Content = pref.ToString();
            var button = new Button();
            button.Content = "x";
            var stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            stack.Children.Add(label);
            stack.Children.Add(button);
            StackPanel_SelectedPreferences.Children.Add(stack);
        }

        private Preference InterpretPreference(PreferenceTopic choice)
        {
            Preference preferenceMethod = null;
            switch (choice)
            {
                case PreferenceTopic.ByProductCategory:
                    preferenceMethod = SortByProductCategory();
                    break;
                case PreferenceTopic.ByRecipeCategory:
                    preferenceMethod = SortByRecipeCategory();
                    break;
                case PreferenceTopic.BySpecificIngredients:
                    preferenceMethod = SortBySingleIngredient();
                    break;
                case PreferenceTopic.ByIngredientsOwned:
                    preferenceMethod = SortByIngredientsOwned();
                    break;
                case PreferenceTopic.ByExpirationDate:
                    preferenceMethod = SortByExpirationDate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
            throw new NotImplementedException();
        }

        private Preference SortByExpirationDate()
        {
            throw new NotImplementedException();
        }

        private Preference SortByIngredientsOwned()
        {
            throw new NotImplementedException();
        }

        private Preference SortBySingleIngredient()
        {
            throw new NotImplementedException();
        }

        private Preference SortByRecipeCategory()
        {
            throw new NotImplementedException();
        }

        private Preference SortByProductCategory()
        {
            throw new NotImplementedException();
        }

        private void AddPreference(Preference preference)
        {
        }

        private void ComboBox_Pref1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pref1Choice = ExtensionMethods.GetEnumValueFromDescription<PreferenceTopic>(Pref1Value);
        }
    }
}
