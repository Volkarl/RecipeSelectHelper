using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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
            Pref2Settings = new ObservableCollection<string>();
            Pref3Settings = new ObservableCollection<string>();
            Pref1Value = String.Empty;
            Pref2Value = String.Empty;
            Pref3Value = String.Empty;
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

        private ObservableCollection<string> _pref2Settings;
        public ObservableCollection<string> Pref2Settings
        {
            get { return _pref2Settings; }
            set { _pref2Settings = value; OnPropertyChanged(nameof(Pref2Settings)); }
        }

        private string _pref2Value;
        public string Pref2Value
        {
            get { return _pref2Value; }
            set { _pref2Value = value; OnPropertyChanged(nameof(Pref2Value)); }
        }

        private ObservableCollection<string> _pref3Settings;
        public ObservableCollection<string> Pref3Settings
        {
            get { return _pref3Settings; }
            set { _pref3Settings = value; OnPropertyChanged(nameof(Pref3Settings)); }
        }

        private string _pref3Value;
        public string Pref3Value
        {
            get { return _pref3Value; }
            set { _pref3Value = value; OnPropertyChanged(nameof(Pref3Value)); }
        }

        #endregion

        #region PreferenceChoices

        private enum Pref1Choices
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

        private Pref1Choices _pref1Choice;
        private Pref1Choices Pref1Choice
        {
            get { return _pref1Choice; }
            set { _pref1Choice = value;
                LoadPref2Settings(value);
            }
        }
        // Missing two here and so and so forth
        #endregion

        #region LoadPreferenceChoices

        private void LoadPref1Settings()
        {
            var s = new List<string>
            {
                Pref1Choices.ByProductCategory.GetDescription(),
                Pref1Choices.ByRecipeCategory.GetDescription(),
                Pref1Choices.BySpecificIngredients.GetDescription(),
                Pref1Choices.ByIngredientsOwned.GetDescription(),
                Pref1Choices.ByExpirationDate.GetDescription()
            };
            Pref1Settings = new ObservableCollection<string>(s);
        }

        private void LoadPref2Settings(Pref1Choices value)
        {
            switch (value)
            {
                case Pref1Choices.ByProductCategory:
                    break;
                case Pref1Choices.ByRecipeCategory:
                    break;
                case Pref1Choices.BySpecificIngredients:
                    break;
                case Pref1Choices.ByIngredientsOwned:
                    break;
                case Pref1Choices.ByExpirationDate:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        #endregion

        private void SortingMethodsPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Button_AddNewPreference_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_FinalizePreference_Click(object sender, RoutedEventArgs e)
        {
//            StackPanel_SelectedPreferences.Children.Add(sortingMethod);
        }

        private void ComboBox_Pref1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _pref1Choice = ExtensionMethods.GetEnumValueFromDescription<Pref1Choices>(Pref1Value);
        }

        private void ComboBox_Pref2_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ComboBox_Pref3_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
