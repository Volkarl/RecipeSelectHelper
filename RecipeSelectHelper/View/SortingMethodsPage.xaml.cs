using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            this._parent = parent;
            DataContext = this;
            this.Loaded += SortingMethodsPage_Loaded;
            InitializeComponent();
        }

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
    }
}
