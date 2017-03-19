using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        public static void HeaderFillRemainingSpace(ListView listView, int headerIndex)
        {
            if(listView == null) throw new ArgumentException(nameof(listView) + " Invalid");
            GridView gridView = listView.View as GridView;
            if (gridView == null) throw new ArgumentException("Inner " + nameof(gridView) + " Invalid");
            if (headerIndex > gridView.Columns.Count) throw new ArgumentException(nameof(headerIndex) + " Invalid");
            
            var remainingWidth = listView.ActualWidth - 10;

            for (Int32 i = 1; i < gridView.Columns.Count; i++)
            {
                remainingWidth -= gridView.Columns[i].ActualWidth;
            }

            remainingWidth = remainingWidth >= 0 ? remainingWidth : 0;

            gridView.Columns[headerIndex].Width = remainingWidth;
        }
    }
}
