using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources.CustomControls
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
