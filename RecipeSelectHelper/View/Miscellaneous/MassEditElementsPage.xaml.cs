using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for MassEditElementsPage.xaml
    /// This page can for instance be used for asking the user one-by-one whether each recipe should recieve a new category.
    /// </summary>
    public partial class MassEditElementsPage : Page, INotifyPropertyChanged
    {
        private MainWindow _parent;
        private Func<object, string> _createItemDescription;
        private Action<object> _yesIsClicked;
        private Action<object> _noIsClicked;
        private List<bool> _logOfAnswers;

        public MassEditElementsPage(MainWindow parent, string title, string pageDescription, 
            Func<ProgramData, IList<object>> findCollectionFunc, Func<object, string> createItemDescription, 
            Action<object> yesIsClicked, Action<object> noIsClicked)
        {
            _parent = parent;
            ItemsToEdit = findCollectionFunc(parent.Data);
            _createItemDescription = createItemDescription;
            _yesIsClicked = yesIsClicked;
            _noIsClicked = noIsClicked;
            PageTitle = title;
            PageDescription = pageDescription;
            ItemDescription = createItemDescription(GetNextObject());
            _logOfAnswers = new List<bool>(ItemsToEdit.Count);

            Loaded += MassEditElementsBasePage_Loaded;
            InitializeComponent();
        }

        private void MassEditElementsBasePage_Loaded(object sender, RoutedEventArgs e)
        {
            ButtonYes.Focus();
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string PageTitle { get; }

        public string PageDescription { get; }

        public IList<object> ItemsToEdit { get; }

        private int _currentIndex = 0;
        public int CurrentIndex
        {
            get { return _currentIndex; }
            private set { _currentIndex = value; OnPropertyChanged(nameof(CurrentIndex)); }
        }

        private string _itemDescription;
        public string ItemDescription
        {
            get { return _itemDescription; }
            set { _itemDescription = value; OnPropertyChanged(nameof(ItemDescription)); }
        }

        #endregion

        private object GetNextObject()
        {
            if (ItemsToEdit.Count <= CurrentIndex)
            {
                ApplyAndClosePage();
                return null;
            }
            object newItem = ItemsToEdit[CurrentIndex++];
            ItemDescription = _createItemDescription(newItem);
            return newItem;
        }

        private void ApplyAndClosePage()
        {
            ApplyChanges();
            ClosePage();
        }

        private void ClosePage()
        {
            _parent.ContentControl.NavigationService.GoBack();
        }

        private void ApplyChanges()
        {
            if(_logOfAnswers.Count > ItemsToEdit.Count) throw new ArgumentException();
            for (var i = 0; i < _logOfAnswers.Count; i++)
            {
                bool answer = _logOfAnswers[i];
                if (answer) _yesIsClicked(ItemsToEdit[i]);
                else _noIsClicked(ItemsToEdit[i]);
            }
        }

        private void ButtonYes_OnClick(object sender, RoutedEventArgs e)
        {
            _logOfAnswers.Add(true);
            GetNextObject();
        }

        private void ButtonNo_OnClick(object sender, RoutedEventArgs e)
        {
            _logOfAnswers.Add(false);
            GetNextObject();
        }

        private void ButtonAbort_OnClick(object sender, RoutedEventArgs e)
        {
            ClosePage();
        }
    }
}
