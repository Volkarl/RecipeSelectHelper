using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "GroupedRecipeCategory")]
    public class GroupedRecipeCategory
    {
        [DataMember]
        public List<int> SelectedIndex { get; set; }
        [DataMember]
        public GroupedSelection<RecipeCategory> CorrespondingGroupedSelection { get; set; }

        public GroupedRecipeCategory(GroupedSelection<RecipeCategory> correspondingGroupedRC)
        {
            SelectedIndex = new List<int>();
            if(correspondingGroupedRC == null) throw new ArgumentException();
            CorrespondingGroupedSelection = correspondingGroupedRC;
        }

        public void SelectItem(int index)
        {
            if (SelectedIndex.Contains(index)) return;
            if(index >= CorrespondingGroupedSelection.GroupedItems.Count ||
               index < 0) throw new ArgumentException("Invalid index");
            SelectedIndex.Add(index);
        }

        public void DeselectItem(int index)
        {
            SelectedIndex.Remove(index);
        }

        public List<RecipeCategory> GetSelectedItems()
        {
            SelectionIsValid();
            List<RecipeCategory> selectedItems = new List<RecipeCategory>();
            foreach (int i in SelectedIndex)
            {
                selectedItems.Add(CorrespondingGroupedSelection.GroupedItems[i]);
            }
            return selectedItems;
        }

        public void SelectionIsValid()
        {
            if (SelectedIndex.Count < CorrespondingGroupedSelection.MinSelect ||
                SelectedIndex.Count > CorrespondingGroupedSelection.MaxSelect) throw new ArgumentException("Selected Items: " +
                SelectedIndex.Count + " | MinSelect: " + CorrespondingGroupedSelection.MinSelect + " | MaxSelect: " + CorrespondingGroupedSelection.MaxSelect);
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
