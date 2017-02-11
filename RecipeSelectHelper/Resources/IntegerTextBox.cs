using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources
{
    public class IntegerTextBox : TextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            Text = new String(Text.Where(c => Char.IsDigit(c)).ToArray());
            this.SelectionStart = Text.Length;

            VerticalContentAlignment = VerticalAlignment.Center;
        }
    }
}
