using System;
using System.Linq;

namespace RecipeSelectHelper.Resources.CustomControls
{
    public class IntegerTextBox : LimitedInputTextBox
    {
        public IntegerTextBox() : base(AllowPositiveAndNegativeIntegers) { }

        private static string AllowPositiveAndNegativeIntegers(string str)
        {
            string minus = str.FirstOrDefault() == '-' ? "-" : "";
            string rest = new string(str.Where(Char.IsDigit).ToArray());
            return minus + rest;
        }
    }
}