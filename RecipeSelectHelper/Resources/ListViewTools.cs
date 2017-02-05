using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Resources
{
    public static class ListViewTools
    {
        public static void RemoveElementAndSelectPrevious<T>(ref T selectedElement, ref ObservableCollection<T> collection)
        {
            if (selectedElement != null && collection != null)
            {
                int indexOfSelection = collection.IndexOf(selectedElement);
                if (indexOfSelection > 0)
                {
                    selectedElement = collection[indexOfSelection - 1];
                }
                else
                {
                    selectedElement = default(T);
                }
                collection.RemoveAt(indexOfSelection);
            }
        }
    }
}
