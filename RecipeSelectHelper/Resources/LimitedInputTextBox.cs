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
        private readonly Func<string, string> _allowedStringFunc;

        public LimitedInputTextBox(Func<char, bool> allowedCharacters) : this(s => new string(s.Where(allowedCharacters).ToArray())) { }

        /// <summary>
        /// Use for context-specific string filtering, for instance: allowing a single '-' character at the front of the string
        /// </summary>
        /// <param name="allowedStringFunc"></param>
        public LimitedInputTextBox(Func<string, string> allowedStringFunc)
        {
            _allowedStringFunc = allowedStringFunc ?? (s => s);
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            string unmodifiedText = Text;
            Text = _allowedStringFunc(Text);

            if (Text != unmodifiedText) 
                this.SelectionStart = Text.Length;
            // If something was modified as a result of the method, then place text selection on the far right.
        }
    }
}
