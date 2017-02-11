using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using RecipeSelectHelper.Model;
using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;

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
                    ui1 = new Label {Content = "If recipe contains any ingredient from fridge"};
                    ui2 = new DockPanelWithLabel("Then add value: ", 
                        new IntegerTextBox());
                    break;
                case PreferenceTopic.ByExpirationDate:
                    ui1 = new Label {Content = "LOTS OF WEIRD STUFF AND CONFIGURATIONS SHOULD BE HERE!"};
                    ui2 = new IntegerTextBox();
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

        private DockPanel CreateDockPanelWithLabel(object labelContent, UIElement secondElement)
        {
            var dockPanel = new DockPanel();
            dockPanel.Children.Add(new Label {Content = labelContent, Width = 200});
            dockPanel.Children.Add(secondElement);
            return dockPanel;
        }

        private UniformGrid CreateUniformGridWithLabel(object labelContent, UIElement secondElement)
        {
            var uniformGrid = new UniformGrid
            {
                Rows = 1,
                Columns = 2
            };
            uniformGrid.Children.Add(new Label {Content = labelContent});
            uniformGrid.Children.Add(secondElement);
            return uniformGrid;
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
                        //var dp = StackPanel_CurrentPreference.Children[1] as DockPanelWithLabel;
                        //var cbx = dp.SecondElement as ComboBox;
                        //var pc = cbx.SelectedValue as ProductCategory;
                        //var itxt = StackPanel_CurrentPreference.Children[2] as IntegerTextBox;
                        //int value;
                        //if (int.TryParse(itxt.Text, out value))
                        //{
                        //    preferenceMethod = SortByProductCategory(pc, value);
                        //}
                        //else
                        //{
                        //    throw new NullReferenceException("Invalid input");
                        //}
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
                    GetValuesFromUI(out value, StackPanel_CurrentPreference, 1);
                    break;
                }
                case PreferenceTopic.ByExpirationDate:
                {
                    preferenceMethod = SortByExpirationDate();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(choice), choice, null);
            }
            return preferenceMethod;
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
            var itxt = StackPanel_CurrentPreference.Children[2] as IntegerTextBox;
            return int.TryParse(itxt.Text, out value);   //what happens if its empty? Will it raise an exception regardless?
        }

        private Preference SortByExpirationDate()
        {
            var method = new Action<ProgramData>(x => MessageBox.Show("No one knows"));
            return new Preference(method);
        }

        private Preference SortByIngredientsOwned(int val)
        {
            var method = new Action<ProgramData>(x => x.AllBoughtProducts.ForEach(y => y.Value += val));
            return new Preference(method);
        }

        private Preference SortBySingleIngredient(Product p, int val)
        {
            var method = new Action<ProgramData>(x => x.AllProducts.Find(y => y.Equals(p)).Value += val);
            return new Preference(method);
        }

        private Preference SortByRecipeCategory(RecipeCategory rc, int val)
        {
            var method = new Action<ProgramData>(x => x.AllRecipeCategories.Find(y => y.Equals(rc)).Value += val);
            return new Preference(method);
        }

        private Preference SortByProductCategory(ProductCategory pc, int val)
        {
            var method = new Action<ProgramData>(x => x.AllProductCategories.Find(y => y.Equals(pc)).Value += val);
            return new Preference(method);
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
