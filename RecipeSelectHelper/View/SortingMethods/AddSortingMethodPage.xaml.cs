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
using RecipeSelectHelper.Resources.CustomControls;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;

namespace RecipeSelectHelper.View.SortingMethods
{
    /// <summary>
    /// Interaction logic for SortingMethodsPage.xaml
    /// </summary>
    public partial class AddSortingMethodPage : Page, INotifyPropertyChanged, IAddElement
    {
        private MainWindow _parent;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AddSortingMethodPage(MainWindow parent)
        {
            _parent = parent;
            LoadObservableObjects();
            Loaded += SortingMethodsPage_Loaded;
            InitializeComponent();
        }

        private void LoadObservableObjects()
        {
            LoadPref1Settings();
            Pref1Value = String.Empty;
            SelectedPreferences = new ObservableCollection<Preference>();
        }

        private void SortingMethodsPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public event EventHandler<bool> ItemSuccessfullyAdded;

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

        private ObservableCollection<Preference> _selectedPreferences;
        public ObservableCollection<Preference> SelectedPreferences
        {
            get { return _selectedPreferences; }
            set { _selectedPreferences = value; OnPropertyChanged(nameof(SelectedPreferences)); }
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
                StackPanel_CurrentPreference.Children.RemoveRange(1, 2);
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
            UIElement ui1 = null;
            UIElement ui2 = null;

            switch (value)
            {
                case PreferenceTopic.ByProductCategory:
                    ui1 = new DockPanelWithLabel("If product is: ",
                        CreateComboBoxToDisplayName(_parent.Data.AllProductCategories));
                    ui2 = new DockPanelWithLabel("Then add value: ", 
                        new IntegerTextBox());
                    break;
                case PreferenceTopic.ByRecipeCategory:
                    ui1 = new DockPanelWithLabel("If recipe is: ",
                        CreateComboBoxToDisplayName(_parent.Data.AllRecipeCategories));
                    ui2 = new DockPanelWithLabel("Then add value: ", 
                        new IntegerTextBox());
                    break;
                case PreferenceTopic.BySpecificIngredients:
                    ui1 = new DockPanelWithLabel("If recipe contains ingredient: ", 
                        CreateComboBoxToDisplayName(_parent.Data.AllProducts));
                    ui2 = new DockPanelWithLabel("Then add value: ", 
                        new IntegerTextBox());
                    break;
                case PreferenceTopic.ByIngredientsOwned:
                    ui1 = new Label {Content = "If recipe contains any ingredient from fridge", Height = 30};
                    ui2 = new DockPanelWithLabel("Then add value: ", 
                        new IntegerTextBox());
                    break;
                case PreferenceTopic.ByExpirationDate:
                    ui1 = new TextBlock {Text = "By default: assigns value between 0 and ≈ 120 to products from fridge depending on how close they are to expiration.", Height = 30, TextWrapping = TextWrapping.Wrap};
                    ui2 = new DockPanelWithLabel("Multiply this value by: ",
                        new IntegerTextBox());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }

            if (ui1 != null)
            {
                StackPanel_CurrentPreference.Children.Add(ui1);
            }
            if (ui2 != null)
            {
                StackPanel_CurrentPreference.Children.Add(ui2);
            }
        }

        private ComboBox CreateComboBoxToDisplayName<T>(IEnumerable<T> items)
        {
            var cbx = new ComboBox
            {
                ItemsSource = new ObservableCollection<T>(items),
                DisplayMemberPath = "Name"
            };
            return cbx;
        }

        #endregion

        private void Button_FinalizePreference_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Preference pref = InterpretPreference(Pref1Choice);
                SelectedPreferences.Add(pref);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Invalid Selection, Error: \n\n" + exception.Message);
            }
        }

        private Preference InterpretPreference(PreferenceTopic choice)
        {
            Preference preferenceMethod = null;
            switch (choice)
            {
                case PreferenceTopic.ByProductCategory:
                {
                    ProductCategory pc;
                    int value;
                    if (GetValuesFromUI(out pc, out value, StackPanel_CurrentPreference))
                    {
                        preferenceMethod = SortByProductCategory(pc, value);
                    }
                    else
                    {
                        throw new NullReferenceException("Invalid input");
                    }
                    break;
                }
                case PreferenceTopic.ByRecipeCategory:
                {
                    RecipeCategory rc;
                    int value;
                    if (GetValuesFromUI(out rc, out value, StackPanel_CurrentPreference))
                    {
                        preferenceMethod = SortByRecipeCategory(rc, value);
                    }
                    break;
                }
                case PreferenceTopic.BySpecificIngredients:
                {
                    Product p;
                    int value;
                    if (GetValuesFromUI(out p, out value, StackPanel_CurrentPreference))
                    {
                        preferenceMethod = SortBySingleIngredient(p, value);
                    }
                    break;
                }
                case PreferenceTopic.ByIngredientsOwned:
                {
                    int value;
                    UnpackValues(out value, StackPanel_CurrentPreference);
                    //if (GetValuesFromUI(out value, StackPanel_CurrentPreference, 1))
                    //{
                    //    preferenceMethod = SortByIngredientsOwned(value);
                    //}
                    preferenceMethod = SortByIngredientsOwned(value);
                    break;
                    }
                case PreferenceTopic.ByExpirationDate:
                {
                    int value;
                    UnpackValues(out value, StackPanel_CurrentPreference);
                    //GetValuesFromUI(out value, StackPanel_CurrentPreference, 1);
                    preferenceMethod = SortByExpirationDate(value);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
            return preferenceMethod;
        }

        private bool UnpackValues(out int value, StackPanel stackPanel) // My naming scheme here is completely off the chain -.-
        {
            var dp = stackPanel.Children[2] as DockPanelWithLabel;
            var itxt = dp.SecondElement as IntegerTextBox;
            return int.TryParse(itxt.Text, out value);
        }

        private bool GetValuesFromUI(out int value, StackPanel stackPanel, int integerTextBoxIndex)
        {
            var itxt = stackPanel.Children[integerTextBoxIndex] as IntegerTextBox;
            return int.TryParse(itxt.Text, out value);
        }

        private bool GetValuesFromUI<T>(out T selectedItem, out int value, StackPanel stackPanel) where T : class
        {
            var dp = stackPanel.Children[1] as DockPanelWithLabel;
            var cbx = dp.SecondElement as ComboBox;
            selectedItem = cbx.SelectedValue as T;
            var dp2 = stackPanel.Children[2] as DockPanelWithLabel;
            var itxt = dp2.SecondElement as IntegerTextBox;
            return int.TryParse(itxt.Text, out value);   //what happens if its empty? Will it raise an exception regardless?
        }

        private Preference SortByExpirationDate(int val)
        {
            return new ExpirationDatePreference(val);
        }

        private Preference SortByIngredientsOwned(int val)
        {
            return new IngredientsOwnedPreference(val);
        }

        private Preference SortBySingleIngredient(Product p, int val)
        {
            return new SingleIngredientPreference(val, p);
        }

        private Preference SortByRecipeCategory(RecipeCategory rc, int val)
        {
            return new RecipeCategoryPreference(val, rc);
        }

        private Preference SortByProductCategory(ProductCategory pc, int val)
        {
            return new ProductCategoryPreference(val, pc);
        }

        private void ComboBox_Pref1_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pref1Choice = ExtensionMethods.GetEnumValueFromDescription<PreferenceTopic>(Pref1Value);
        }

        private void Button_Click_RemovePreference(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var stackPanel = button.Parent as StackPanel;
            var textBlock = stackPanel.Children[0] as TextBlock;
            var itemToRemove = textBlock.DataContext as Preference;
            SelectedPreferences.Remove(itemToRemove);
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            try
            {
                var sm = new SortingMethod(TextBox_SortingMethodName.Text, SelectedPreferences.ToList());
                _parent.Data.AllSortingMethods.Add(sm);
                ClearUI();
                ItemSuccessfullyAdded?.Invoke(this, true);
            }
            catch (ArgumentException ex)
            {
                ItemSuccessfullyAdded?.Invoke(this, false);
                ErrorReporter.EmptyRequiredProperty(ex.Message);
            }
        }

        private void ClearUI()
        {
            SelectedPreferences = new ObservableCollection<Preference>();
            TextBox_SortingMethodName.Text = String.Empty;
        }
    }
}



// ADD TOOLTIPS
// ADD Error when sorting method has no members, mm.