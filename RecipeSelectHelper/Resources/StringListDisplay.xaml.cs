using System;
using System.Collections.Generic;
using System.Linq;
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
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;

namespace RecipeSelectHelper.Resources
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

        public string Title // Make attached property
        {
            get { throw new NotImplementedException(); }
        }

        public StringList StringList // Make attached
        {
            get { throw new NotImplementedException(); }
        }

        private void ButtonAdd_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonUp_OnClick(object sender, RoutedEventArgs e)
        {
            StringList.MoveInDirection(SelectedString, StringList.MoveDirection.Up);
        }

        private void ButtonDown_OnClick(object sender, RoutedEventArgs e)
        {
            StringList.MoveInDirection(SelectedString, StringList.MoveDirection.Down);
        }

        private void ButtonEdit_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ButtonRemove_OnClick(object sender, RoutedEventArgs e)
        {
            StringList.Remove(SelectedString);
        }
    }
}
