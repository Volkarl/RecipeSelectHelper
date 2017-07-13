using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources
{
    public class IntegerTextBox : PositiveIntegerTextBox
    {
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            bool isNegative = Text.FirstOrDefault() == '-';
            base.OnTextChanged(e);
            Text = '-' + Text;

            // TODO UNTESTED!
        }
    }
}
