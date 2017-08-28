using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RecipeSelectHelper.Resources.Converters
{
    public class NullOrEmptyToHiddenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null || IsListWithoutElements(value) ? Visibility.Hidden : Visibility.Visible;
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
