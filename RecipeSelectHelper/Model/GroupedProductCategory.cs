using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "GroupedProductCategory")]
    public class GroupedProductCategory
    {
        [DataMember]
        public int MinSelect { get; set; }
        [DataMember]
        public int MaxSelect { get; set; }
        [DataMember]
        public List<Boolable<ProductCategory>> GroupedPc { get; set; }
        [DataMember]
        public GroupedSelection<ProductCategory> CorrespondingGroupedSelection { get; set; }

        private GroupedProductCategory() { }

        public GroupedProductCategory(GroupedSelection<ProductCategory> correspondingGroupedPc) : this(correspondingGroupedPc.GroupedItems, correspondingGroupedPc.MinSelect, correspondingGroupedPc.MaxSelect)
        {
            if (correspondingGroupedPc == null) throw new ArgumentException();
            CorrespondingGroupedSelection = correspondingGroupedPc;
        }

        private GroupedProductCategory(List<ProductCategory> correspondingGroupedPc, int minSelect, int maxSelect)
        {
            if (correspondingGroupedPc == null) throw new ArgumentException();
            MinSelect = minSelect;
            MaxSelect = maxSelect;
            GroupedPc = new List<Boolable<ProductCategory>>();
            foreach (ProductCategory pc in correspondingGroupedPc)
            {
                GroupedPc.Add(new Boolable<ProductCategory>(pc));
            }
        }

        public List<ProductCategory> GetCurrentSelectedItems()
        {
            return GroupedPc.Where(pc => pc.Bool).Select(bpc => bpc.Instance).ToList();

            //List<ProductCategory> selectedItems = new List<ProductCategory>();
            //foreach (Boolable<ProductCategory> pcBoolable in GroupedPc)
            //{
            //    if (pcBoolable.Bool) selectedItems.Add(pcBoolable.Instance);
            //}

            //return selectedItems;
        }

        public bool SelectionIsValid()
        {
            string error;
            return SelectionIsValid(out error);
        }

        public bool SelectionIsValid(out string error)
        {
            List<ProductCategory> selectedItems = GetCurrentSelectedItems();

            bool tooMany = selectedItems.Count > MaxSelect;
            bool tooFew = selectedItems.Count < MinSelect;

            error = null;
            if (!tooFew && !tooMany) return true;
            if (tooMany)
            {
                error = $"Too many items selected, only {MaxSelect} are allowed";
            }
            else if (tooFew)
            {
                error = $"Too few items selected, {MaxSelect} are required";
            }
            return false;
        }

        public List<ProductCategory> GetFinalizedSelection()
        {
            string error;
            if (!SelectionIsValid(out error)) throw new ArgumentException(error);
            return GetCurrentSelectedItems();
        }

        public override string ToString()
        {
            string s = String.Empty;
            foreach (ProductCategory selectedPc in GetCurrentSelectedItems())
            {
                s += selectedPc.Name + ", ";
            }
            s = s.TrimEnd(' ', ',');
            s += " | ";
            return s + $"{MinSelect}-{MaxSelect}";
        }
    }
}
