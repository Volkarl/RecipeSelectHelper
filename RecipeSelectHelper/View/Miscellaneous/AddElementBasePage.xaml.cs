using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for AddElementBasePage.xaml
    /// </summary>
    public partial class AddElementBasePage : Page, INotifyPropertyChanged
    {
        private IAddElement _content;
        public string ContentPageTitle;
        private IParentPage _parent;
        private object _finalizeButtonContent;

        #region ObservableObjects

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool? _itemSuccessfullyAdded = null;
        public bool? ItemSuccessfullyAdded
        {
            get { return _itemSuccessfullyAdded; }
            set { _itemSuccessfullyAdded = value; OnPropertyChanged(nameof(ItemSuccessfullyAdded)); }
        }

        #endregion

        public AddElementBasePage(IParentPage parent) : this(null, "Page Not Implemented", parent, string.Empty) { }

        // Rename this class maybe. Its no longer only for adding elements, but also for editing elements. 
        // Anything with a back button and a finalize button!
        public AddElementBasePage(IAddElement content, string title, IParentPage parent, string contentOfFinalizeButton = "Add")
        {
            _content = content;
            ContentPageTitle = title;
            _parent = parent;
            _finalizeButtonContent = contentOfFinalizeButton;

            Loaded += AddElementBasePage_Loaded;
            InitializeComponent();
        }

        private void AddElementBasePage_Loaded(object sender, RoutedEventArgs e)
        {
            var addItemPage = _content;
            if (addItemPage != null)
            {
                Button_Add.Click -= addItemPage.AddItem; 
                Button_Add.Click += addItemPage.AddItem;
                addItemPage.ItemSuccessfullyAdded -= OnItemSuccessfullyAdded;
                addItemPage.ItemSuccessfullyAdded += OnItemSuccessfullyAdded;
                // This is done to remove old (duplicate) subscriptions caused by clicking the BACK-button and navigating to a new version of the page.
            }

            this.TextBlock_PageTitle.Text = ContentPageTitle;
            this.Button_Add.Content = _finalizeButtonContent;
            this.ContentControl_AddNewItem.Content = _content;
        }

        private void OnItemSuccessfullyAdded(object sender, bool b) => ItemSuccessfullyAdded = b;

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            _parent.NavigatePageBack();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
        }

        //TOdo cant I just delete the above?
    }
}
