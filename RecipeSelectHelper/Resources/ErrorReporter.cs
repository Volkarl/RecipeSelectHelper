using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Resources
{
    public static class ErrorReporter
    {
        private static readonly string _error = "Error!";

        public static void EmptyRequiredProperty(string propertyName)
        {
            MessageBox.Show(propertyName + " cannot be empty.", _error);
        }
    }
}
