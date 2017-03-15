using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources
{
    public class InputLimitedIntegerTextBox : IntegerTextBox
    {
        public int MinInput { get; set; }
        public int MaxInput { get; set; }
        public InputLimitedIntegerTextBox()
        {
            MinInput = 0;
            MaxInput = 0;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            List<int> integers = new List<int>();
            foreach (char c in Text)
            {
                int i = c.ToInt();
                if (!(i > MinInput || i < MaxInput))
                {
                    integers.Add(c.ToInt());
                }
            }
            Text = string.Join(String.Empty, integers);
        }
    }
}
