using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "GroupedRecipeCategory")]
    public class GroupedRecipeCategory
    {
        [DataMember]
        public int MinSelect { get; set; }
        [DataMember]
        public int MaxSelect { get; set; }
        [DataMember]
        public List<Boolable<RecipeCategory>> GroupedRc { get; set; }

        public GroupedRecipeCategory(GroupedSelection<RecipeCategory> correspondingGroupedRc) : this(correspondingGroupedRc.GroupedItems, correspondingGroupedRc.MinSelect, correspondingGroupedRc.MaxSelect)
        {
            if(correspondingGroupedRc == null) throw new ArgumentException();
        }

        private GroupedRecipeCategory(List<RecipeCategory> correspondingGroupedRc, int minSelect, int maxSelect)
        {
            if(correspondingGroupedRc == null) throw new ArgumentException();
            MinSelect = minSelect;
            MaxSelect = maxSelect;
            GroupedRc = new List<Boolable<RecipeCategory>>();
            foreach (RecipeCategory rc in correspondingGroupedRc)
            {
                GroupedRc.Add(new Boolable<RecipeCategory>(rc));
            }
        }

        public List<RecipeCategory> GetCurrentSelectedItems()
        {
            List<RecipeCategory> selectedItems = new List<RecipeCategory>();
            foreach (Boolable<RecipeCategory> rcBoolable in GroupedRc)
            {
                if(rcBoolable.Bool) selectedItems.Add(rcBoolable.Instance);
            }

            return selectedItems;
        }

        public bool SelectionIsValid(out string error)
        {
            List<RecipeCategory> selectedItems = GetCurrentSelectedItems();

            bool tooMany = selectedItems.Count > MaxSelect;
            bool tooFew = selectedItems.Count < MinSelect;

            error = null;
            if (!tooFew && !tooMany) return true;
            if (tooMany)
            {
                error = $"Too many items selected, only {MaxSelect} are allowed";
            }
            else if(tooFew)
            {
                error = $"Too few items selected, {MaxSelect} are required";
            }
            return false;
        }

        public List<RecipeCategory> GetFinalizedSelection()
        {
            string error;
            if(!SelectionIsValid(out error)) throw new ArgumentException(error);
            return GetCurrentSelectedItems();
        }

        public override string ToString()
        {
            string s = String.Empty;
            foreach (RecipeCategory selectedRc in GetCurrentSelectedItems())
            {
                s += selectedRc.Name + ", ";
            }
            s = s.TrimEnd(' ', ',');
            s += " | ";
            return s + $"{MinSelect}-{MaxSelect}";
        }
    }
}
