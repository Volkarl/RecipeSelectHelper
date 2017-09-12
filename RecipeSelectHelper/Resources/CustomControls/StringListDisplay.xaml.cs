using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;

namespace RecipeSelectHelper.Resources.CustomControls
{
    /// <summary>
    /// Interaction logic for StringListDisplay.xaml
    /// </summary>
    public partial class StringListDisplay : UserControl
    {
        public StringListDisplay()
        {
            InitializeComponent();
        }

        private OnEnterPressed _onEnter;

        enum OnEnterPressed
        {
            AddNew, EditSelected
        }

        #region DependencyProperties

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(StringListDisplay), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string) GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty StringsProperty = DependencyProperty.Register(
            "Strings", typeof(StringList), typeof(StringListDisplay), new PropertyMetadata(default(StringList), RegisterUpdateStringDisplayEvent));

        public StringList Strings
        {
            get { return (StringList)GetValue(StringsProperty); }
            set { SetValue(StringsProperty, value); }
        }

        #endregion

        private static void RegisterUpdateStringDisplayEvent(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Every time a new stringlist is added to the dependency property, this is executed to make sure that the new stringlist also
            // calls the RefreshListView method whenever its collection is changed (for instance when items are added/removed from the stringlist)
            var control = (StringListDisplay) sender;
            var oldCollection = e.OldValue as INotifyCollectionChanged;
            var newCollection = e.NewValue as INotifyCollectionChanged;
            if (oldCollection != null) oldCollection.CollectionChanged -= control.StringsCollectionChanged;
            if (newCollection != null) newCollection.CollectionChanged += control.StringsCollectionChanged;
            control.RefreshListView();
        }

        private void StringsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => RefreshListView();

        private void RefreshListView()
        {
            ListViewOfInstructions.Items.Refresh();
        }

        private string SelectedString
        {
            get { return ListViewOfInstructions.SelectedItem as string; }
            set { ListViewOfInstructions.SelectedItem = value; }
        }

        private int SelectedIndex => ListViewOfInstructions.SelectedIndex;

        private string NewInstruction
        {
            get { return TextBoxNewInstruction.Text; }
            set { TextBoxNewInstruction.Text = value; }
        }

        private Visibility NewInstructionVisibility
        {
            set { TextBoxNewInstruction.Visibility = value; }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            ShowNewInstruction(true);
            _onEnter = OnEnterPressed.AddNew;
        }

        private void ButtonUp_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedString == null || SelectedIndex <= 0) return;
            int oldSelected = SelectedIndex;
            Strings.MoveInDirection(SelectedString, StringList.MoveDirection.Up);
            SelectedString = Strings[oldSelected - 1];
        }

        private void ButtonDown_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedString == null || SelectedIndex >= Strings.LastIndex()) return;
            int oldSelected = SelectedIndex;
            Strings.MoveInDirection(SelectedString, StringList.MoveDirection.Down);
            SelectedString = Strings[oldSelected + 1];
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (SelectedString == null) return;
            NewInstruction = SelectedString;
            ShowNewInstruction(false);
            _onEnter = OnEnterPressed.EditSelected;
        }

        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedString == null) return;
            int oldSelected = SelectedIndex;
            Strings.RemoveAt(SelectedIndex);
            if(oldSelected > 0) SelectedString = Strings[oldSelected - 1];
            RefreshListView();
        }

        private void TextBoxNewInstruction_OnLostFocus(object sender, RoutedEventArgs e)
        {
            HideNewInstruction(false);
        }

        private void HideNewInstruction(bool clear)
        {
            if(clear) NewInstruction = String.Empty;
            NewInstructionVisibility = Visibility.Collapsed;
        }

        private void ShowNewInstruction(bool clearFirst)
        {
            if(clearFirst) NewInstruction = String.Empty;
            NewInstructionVisibility = Visibility.Visible;
            TextBoxNewInstruction.Focus();
        }

        private void TextBoxNewInstruction_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (_onEnter == OnEnterPressed.AddNew)
                {
                    int oldSelected = SelectedString == null ? 0 : SelectedIndex;
                    Strings.Add(NewInstruction);
                    NewInstruction = String.Empty;
                    SelectedString = Strings[oldSelected];
                }
                else
                {
                    int oldSelected = SelectedIndex;
                    Strings[SelectedIndex] = NewInstruction;
                    HideNewInstruction(true);
                    SelectedString = Strings[oldSelected];
                }
                e.Handled = true;
                RefreshListView();
            }
            else if (e.Key == Key.Escape)
            {
                HideNewInstruction(false);
                e.Handled = true;
            }
        }
    }
}
