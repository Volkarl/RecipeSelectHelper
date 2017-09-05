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
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for MassAddGroupedCategoriesPage.xaml
    /// This page is used for modifying items where yes-no questions are not enough.
    /// For instance, adding a new GroupedCategory to every recipe.
    /// It works by turning every item into a UiElement (using Func), displaying this, and then turning the UiElement back into a modified item.
    /// </summary>
    public partial class MassAddGroupedCategoriesPage : Page, INotifyPropertyChanged
    {
        public class Result
        {
            public ContentControl ModifiedUi;
            public object ModifiedObject;

            public Result(ContentControl modifiedUi, object modifiedObject)
            {
                ModifiedUi = modifiedUi;
                ModifiedObject = modifiedObject;
            }
        }

        private readonly MainWindow _parent;
        private readonly Func<object, string> _createItemDescription;
        private readonly Func<object, ContentControl> _convertItemToUiElement;
        private readonly Func<ContentControl, object> _convertBack;
        private readonly Action<object, Result> _alterOriginalWithResult;
        private readonly List<ContentControl> _modifiedItems;
        private readonly List<object> _originalItems;
        private readonly Predicate<object> _isNextClickable;

        public MassAddGroupedCategoriesPage(MainWindow parent, string title, string pageDescription, 
            List<object> itemsToEdit, List<object> itemsForCreatingUi, Func<object, string> createItemDescription,
            Func<object, ContentControl> convertItemToUiElement, Func<ContentControl, object> convertBack, Action<object, Result> alterOriginalWithResult,
            Predicate<object> isNextClickable = null)
        {
            _parent = parent;

            if (!itemsToEdit.IsNullOrEmpty()) // If empty, it'll initialize the page and then navigate back
            {
                _originalItems = itemsToEdit;
                ItemsForCreatingUi = itemsForCreatingUi; 
                _modifiedItems = new List<ContentControl>(ItemsForCreatingUi.Count);
                _createItemDescription = createItemDescription;
                _convertItemToUiElement = convertItemToUiElement;
                _convertBack = convertBack;
                _alterOriginalWithResult = alterOriginalWithResult;
                _isNextClickable = isNextClickable ?? (o => true);
                PageTitle = title;
                PageDescription = pageDescription;
            }
            Loaded += MassAddGroupedCategoriesPage_Loaded; ;
            InitializeComponent();
        }

        private void SetNextIsClickable()
        {
            NextIsClickable = _isNextClickable(_convertBack(CurrentUiElement));
        }

        private void MassAddGroupedCategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (ItemsForCreatingUi.IsNullOrEmpty()) ClosePage();
            else
            {
                GetNextObject();
                SetNextIsClickable();
                ContentControl.AddHandler(MouseUpEvent, new MouseButtonEventHandler(OnMouseUp), true);
                // This ensures that the OnMouseUp is executed every mouse click. As such, if the UI Element is modified by mouse, 
                // then we check whether it's still in an acceptable state for clicking next.
            }
        }

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string PageTitle { get; }

        public string PageDescription { get; }

        public IList<object> ItemsForCreatingUi { get; }

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

        private ContentControl _currentUiElement;
        public ContentControl CurrentUiElement
        {
            get { return _currentUiElement; }
            set { _currentUiElement = value; OnPropertyChanged(nameof(CurrentUiElement)); }
        }

        private bool _nextIsClickable;
        public bool NextIsClickable
        {
            get { return _nextIsClickable; }
            set { _nextIsClickable = value; OnPropertyChanged(nameof(NextIsClickable)); }
        }

        #endregion

        private void GetNextObject()
        {
            if (ItemsForCreatingUi.Count <= CurrentIndex)
            {
                ApplyAndClosePage();
                return;
            }
            object newItem = ItemsForCreatingUi[CurrentIndex++];
            CurrentUiElement = _convertItemToUiElement(newItem);
            ItemDescription = _createItemDescription(newItem);
            SetNextIsClickable();
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
            if (_modifiedItems.Count != ItemsForCreatingUi.Count) throw new ArgumentException();
            for (var i = 0; i < _modifiedItems.Count; i++)
            {
                _alterOriginalWithResult(_originalItems[i], new Result(_modifiedItems[i], ItemsForCreatingUi[i]));
            }
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            SetNextIsClickable();
            if (!NextIsClickable) e.Handled = false;
            else
            {
                _modifiedItems.Add(CurrentUiElement);
                GetNextObject();
            }
        }

        private void ButtonAbort_OnClick(object sender, RoutedEventArgs e)
        {
            ClosePage();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            SetNextIsClickable();
        }
    }
}
