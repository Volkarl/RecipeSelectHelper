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

namespace RecipeSelectHelper.Resources
{
    /// <summary>
    /// Interaction logic for TextProgressBar.xaml
    /// </summary>
    public partial class TextProgressBar : UserControl
    {
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.RegisterAttached("OuterMaximum", typeof(int), typeof(TextProgressBar), new PropertyMetadata(ChangedMax));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.RegisterAttached("OuterMinimum", typeof(int), typeof(TextProgressBar), new PropertyMetadata(ChangedMin));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached("OuterValue", typeof(int), typeof(TextProgressBar), new PropertyMetadata(ChangedVal));
        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached("OuterText", typeof(string), typeof(TextProgressBar), new PropertyMetadata(ChangedText));

        #region ObservableObjects

        private static void ChangedMax(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TextProgressBar;
            c.ProgressBar.Maximum = (int)e.NewValue;
        }

        private static void ChangedMin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TextProgressBar;
            c.ProgressBar.Minimum = (int)e.NewValue;
        }

        private static void ChangedVal(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TextProgressBar;
            c.ProgressBar.Value = (int)e.NewValue;
        }

        private static void ChangedText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as TextProgressBar;
            c.TextBlock.Text = (string)e.NewValue;
        }

        public int OuterMaximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public int OuterMinimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public int OuterValue
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string OuterText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        public TextProgressBar()
        {
            InitializeComponent();
        }
    }
}
