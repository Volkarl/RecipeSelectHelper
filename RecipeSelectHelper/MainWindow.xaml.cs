using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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

namespace RecipeSelectHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Show();
            ListView_SizeChanged(ListView_Recipes, null);

        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;
            var remainingWidth = listView.ActualWidth - SystemParameters.VerticalScrollBarWidth;

            for (Int32 i = 1; i < gridView.Columns.Count; i++)
            {
                remainingWidth -= gridView.Columns[i].ActualWidth;
            }
            gridView.Columns[0].Width = remainingWidth;
        }
    }
}
