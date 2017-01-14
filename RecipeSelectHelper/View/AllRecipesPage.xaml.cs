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

namespace RecipeSelectHelper.View
{
    /// <summary>
    /// Interaction logic for AllRecipesPage.xaml
    /// </summary>
    public partial class AllRecipesPage : Page
    {
        public AllRecipesPage()
        {
            InitializeComponent();
            this.Loaded += RankingsViewPageLoaded;
        }

        private void RankingsViewPageLoaded(object sender, RoutedEventArgs e)
        {
            ListView_SizeChanged(ListView_Recipes, null);
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gridView = listView.View as GridView;
            var remainingWidth = listView.ActualWidth - 5;

            for (Int32 i = 1; i < gridView.Columns.Count; i++)
            {
                remainingWidth -= gridView.Columns[i].ActualWidth;
            }
            gridView.Columns[0].Width = remainingWidth;
        }
    }
}
