using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "GroupedProductCategory")]
    public class GroupedProductCategory
    {
        [DataMember]
        public List<int> SelectedIndex { get; set; }
        [DataMember]
        public GroupedSelection<ProductCategory> CorrespondingGroupedSelection { get; set; }

        public GroupedProductCategory(GroupedSelection<ProductCategory> correspondingGroupedPC)
        {
            SelectedIndex = new List<int>();
            if (correspondingGroupedPC == null) throw new ArgumentException();
            CorrespondingGroupedSelection = correspondingGroupedPC;
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

        public List<ProductCategory> GetSelectedItems()
        {
            if (SelectedIndex.Count < CorrespondingGroupedSelection.MinSelect ||
                SelectedIndex.Count > CorrespondingGroupedSelection.MaxSelect) throw new ArgumentException("Selected Items: " +
                    SelectedIndex.Count + " | MinSelect: " + CorrespondingGroupedSelection.MinSelect + " | MaxSelect: " + CorrespondingGroupedSelection.MaxSelect);

            var selectedItems = new List<ProductCategory>();
            foreach (int i in SelectedIndex)
            {
                selectedItems.Add(CorrespondingGroupedSelection.GroupedItems[i]);
            }
            return selectedItems;
        }

        public override string ToString()
        {
            string s = String.Empty;
            foreach (int i in SelectedIndex)
            {
                s += CorrespondingGroupedSelection.GroupedItems[i] + ", ";
            }
            s = s.TrimEnd(' ', ',');
            s += " | ";
            return s + $"{CorrespondingGroupedSelection.MinSelect}-{CorrespondingGroupedSelection.MaxSelect}";
        }
    }
}
