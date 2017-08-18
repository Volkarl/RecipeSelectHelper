using System;
using System.Windows;

namespace RecipeSelectHelper.Resources
{
    public interface IAddElement
    {
        event EventHandler<bool> ItemSuccessfullyAdded;
        void AddItem(object sender, RoutedEventArgs e);
    }
}
