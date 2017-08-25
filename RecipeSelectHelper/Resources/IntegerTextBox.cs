using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RecipeSelectHelper.Resources
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