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
using System.Windows.Shapes;

namespace RecipeSelectHelper.View.Miscellaneous
{
    /// <summary>
    /// Interaction logic for CopyableTextWindow.xaml
    /// </summary>
    public partial class CopyableTextWindow : Window
    {
        private string _inputText;

        public CopyableTextWindow()
        {
            InitializeComponent();
        }

        public CopyableTextWindow(string text)
        {
            Loaded += CopyableTextWindow_Loaded;
            _inputText = text;
            InitializeComponent();
        }

        private void CopyableTextWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxTextToCopy.Text = _inputText;
            TextBoxTextToCopy.Focus();
            TextBoxTextToCopy.SelectAll();
        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            string text = TextBoxTextToCopy.Text ?? String.Empty;
            Clipboard.SetDataObject(text);
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
