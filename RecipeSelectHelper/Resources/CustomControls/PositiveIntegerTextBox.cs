using System;

namespace RecipeSelectHelper.Resources.CustomControls
{
    public class PositiveIntegerTextBox : LimitedInputTextBox
    {
        public PositiveIntegerTextBox() : base(Char.IsDigit) { }
    }
}