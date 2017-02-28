using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for AddGroupedRecipeCategoryPage.xaml
    /// </summary>
    public partial class AddGroupedRecipeCategoryPage : Page, IAddElement, INotifyPropertyChanged
    {
        private MainWindow _parent;
        
        public AddGroupedRecipeCategoryPage(MainWindow parent)
        {
            _parent = parent;
            LoadObservableObjects();
            InitializeComponent();
        }

        private void LoadObservableObjects()
        {
            MinSelectionAmount = 0;
            MaxSelectionAmount = 3;
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _minSelectionAmount;
        public int MinSelectionAmount
        {
            get { return _minSelectionAmount; }
            set { _minSelectionAmount = value; OnPropertyChanged(nameof(MinSelectionAmount)); }
        }

        private int _maxSelectionAmount;
        private IAddElement _addElementImplementation;

        public int MaxSelectionAmount
        {
            get { return _maxSelectionAmount; }
            set { _maxSelectionAmount = value; OnPropertyChanged(nameof(MaxSelectionAmount)); }
        }

        public void AddItem(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
