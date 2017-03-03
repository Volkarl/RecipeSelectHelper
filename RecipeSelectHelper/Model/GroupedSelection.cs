using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Recipe")]
    public class GroupedSelection<T>
    {
        [DataMember]
        public List<T> GroupedItems { get; set; }
        public List<int> SelectedIndex { get; set; }
        [DataMember]
        public int MinSelect { get; set; }
        [DataMember]
        public int MaxSelect { get; set; }

        public GroupedSelection(List<T> groupedItems, int minSelect, int maxSelect)
        {
            GroupedItems = groupedItems;
            SelectedIndex = new List<int>(groupedItems.Count);

            if(minSelect > groupedItems.Count ||
                minSelect < 0 ||
                maxSelect < 0 ||
                maxSelect < minSelect) throw new ArgumentException("MinSelect: " + minSelect + " & MaxSelect: " + maxSelect);

            MinSelect = minSelect;
            MaxSelect = maxSelect;
        }

        public void SelectItem(int index)
        {
            if (SelectedIndex.Contains(index)) return;
            SelectedIndex.Add(index);
        }

        public void DeselectItem(int index)
        {
            SelectedIndex.Remove(index);
        }

        public List<T> GetSelectedItems()
        {
            if(SelectedIndex.Count < MinSelect ||
                SelectedIndex.Count > MaxSelect) throw new ArgumentException("Selected Items: " + 
                    SelectedIndex.Count + "MinSelect: " + MinSelect +  "MaxSelect: " + MaxSelect);
            
            var selectedItems = new List<T>();
            foreach (int i in SelectedIndex)
            {
                selectedItems.Add(GroupedItems[i]);
            }
            return selectedItems;
        }

        public override string ToString()
        {
            string s = String.Empty;
            foreach (T item in GroupedItems)
            {
                s += item + ", ";
            }
            s = s.TrimEnd(' ', ',');
            s += " | ";
            return s + $"{MinSelect}-{MaxSelect}";
        }
    }
}
