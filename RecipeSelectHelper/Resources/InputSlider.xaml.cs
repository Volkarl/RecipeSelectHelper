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

namespace RecipeSelectHelper.Resources
{
    /// <summary>
    /// Interaction logic for InputSlider.xaml
    /// </summary>
    public partial class InputSlider : UserControl
    {
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("OuterMaximum", typeof(int), typeof(InputSlider), new PropertyMetadata(ChangedMax));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("OuterMinimum", typeof(int), typeof(InputSlider), new PropertyMetadata(ChangedMin));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("OuterValue", typeof(int), typeof(InputSlider), new PropertyMetadata(ChangedVal));

        public InputSlider()
        {
            InitializeComponent();
        }

        #region ObservableObjects

        private static void ChangedMax(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as InputSlider;
            c.Slider.Maximum = (int)e.NewValue;
        }

        private static void ChangedMin(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as InputSlider;
            c.Slider.Minimum = (int)e.NewValue;
        }

        private static void ChangedVal(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as InputSlider;
            c.Slider.Value = (int)e.NewValue;
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

        #endregion
    }
}
