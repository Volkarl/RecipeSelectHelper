using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class FilterGroupedProductCategories
    {
        public List<Boolable<ProductCategory>> GroupedSelection { get; set; }

        public FilterGroupedProductCategories(GroupedSelection<ProductCategory> groupedSelection)
        {
            GroupedSelection = new List<Boolable<ProductCategory>>();
            foreach (var pc in groupedSelection.GroupedItems)
            {
                GroupedSelection.Add(new Boolable<ProductCategory>(pc));
            }
        }
    }
}
