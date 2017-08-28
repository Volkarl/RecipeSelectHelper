using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources.CustomControls
{
    public class InputLimitedIntegerTextBox : PositiveIntegerTextBox
    {
        public static readonly DependencyProperty MaxInputProperty = DependencyProperty.Register(nameof(MaxInput), typeof(int?), typeof(InputLimitedIntegerTextBox), new FrameworkPropertyMetadata(new int?()));
        public int? MaxInput
        {
            get { return GetValue(MaxInputProperty) as int?; }
            set { SetValue(MaxInputProperty, value); }
        }

        public static readonly DependencyProperty MinInputProperty = DependencyProperty.Register(nameof(MinInput), typeof(int?), typeof(InputLimitedIntegerTextBox), new FrameworkPropertyMetadata(new int?()));
        public int? MinInput
        {
            get { return GetValue(MinInputProperty) as int?; }
            set { SetValue(MinInputProperty, value); }
        }

        public InputLimitedIntegerTextBox()
        {
            MaxInput = 0;
            MaxInput = 0;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            List<int> integers = new List<int>();
            foreach (char c in Text)
            {
                int i = c.ToInt();
                if (!(i > MaxInput || i < MaxInput))
                {
                    integers.Add(c.ToInt());
                }
            }
            Text = string.Join(String.Empty, integers);
        }
    }
}
