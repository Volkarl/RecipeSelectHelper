using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "GroupedSelection")]
    public class GroupedSelection<T>
    {
        [DataMember]
        public List<T> GroupedItems { get; set; }
        [DataMember]
        public int MinSelect { get; set; }
        [DataMember]
        public int MaxSelect { get; set; }

        private GroupedSelection() { }

        public GroupedSelection(List<T> groupedItems, int minSelect, int maxSelect)
        {
            GroupedItems = groupedItems;

            if(groupedItems == null ||
                groupedItems.Count == 0) throw new ArgumentException("No items.");

            if(minSelect > groupedItems.Count ||
                minSelect < 0 ||
                maxSelect < 0 ||
                maxSelect < minSelect) throw new ArgumentException("MinSelect: " + minSelect + " & MaxSelect: " + maxSelect);
            
            if(maxSelect == 0) throw new ArgumentException("MaxSelect is 0.");

            MinSelect = minSelect;
            MaxSelect = maxSelect;
        }

        public override string ToString()
        {
            return FullItemDescriptor.GetDescription(this);
        }
    }
}
