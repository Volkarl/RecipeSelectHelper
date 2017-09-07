using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources.CustomControls
{
    /// <summary>
    /// Interaction logic for InputSlider.xaml
    /// </summary>
    public partial class InputSlider : UserControl
    {
        public InputSlider()
        {
            InitializeComponent();
        }

        #region DependencyProperty

        public static readonly DependencyProperty OuterMinimumProperty = DependencyProperty.Register(
            "OuterMinimum", typeof(int), typeof(InputSlider), new PropertyMetadata(default(int)));

        public int OuterMinimum
        {
            get { return (int) GetValue(OuterMinimumProperty); }
            set { SetValue(OuterMinimumProperty, value); }
        }

        public static readonly DependencyProperty OuterMaximumProperty = DependencyProperty.Register(
            "OuterMaximum", typeof(int), typeof(InputSlider), new PropertyMetadata(default(int)));

        public int OuterMaximum
        {
            get { return (int) GetValue(OuterMaximumProperty); }
            set { SetValue(OuterMaximumProperty, value); }
        }

        public static readonly DependencyProperty OuterValueProperty = DependencyProperty.Register(
            "OuterValue", typeof(int), typeof(InputSlider), new PropertyMetadata(default(int)));

        public int OuterValue
        {
            get { return (int) GetValue(OuterValueProperty); }
            set { SetValue(OuterValueProperty, value); }
        }

        #endregion
    }
}
