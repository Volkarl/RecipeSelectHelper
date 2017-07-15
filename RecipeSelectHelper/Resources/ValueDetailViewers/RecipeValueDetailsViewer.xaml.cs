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
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources.ValueDetailViewers
{
    /// <summary>
    /// Interaction logic for RecipeValueDetailsViewer.xaml
    /// </summary>
    public partial class RecipeValueDetailsViewer : UserControl
    {
        public RecipeValueDetailsViewer()
        {
            InitializeComponent();
        }

        public Recipe MyRecipe { get; } = new Recipe("Hot Water", "Water that has been boiled", "Add water over heat and stirr.", categories:new List<RecipeCategory> {new RecipeCategory("Watery food")});

    }
}
