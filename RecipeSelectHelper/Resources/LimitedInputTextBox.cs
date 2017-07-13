using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources
{
    public class LimitedInputTextBox : TextBox
    {
        private readonly Func<char, bool> _allowedChars;

        public LimitedInputTextBox(Func<char, bool> allowedChars)
        {
            _allowedChars = allowedChars;
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            Text = new String(Text.Where(_allowedChars).ToArray());
            this.SelectionStart = Text.Length;
        }
    }
}
