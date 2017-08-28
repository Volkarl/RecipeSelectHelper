using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RecipeSelectHelper.Resources.Converters
{
    public class NullOrEmptyToCollapsedVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || IsListWithoutElements(value) ? Visibility.Collapsed : Visibility.Visible;
        }

        private bool IsListWithoutElements(object value)
        {
            var list = value as IList;
            return list?.Count == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
