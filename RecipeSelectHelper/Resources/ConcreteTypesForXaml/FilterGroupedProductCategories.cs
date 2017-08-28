using System;
using System.Collections.Generic;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources.ConcreteTypesForXaml
{
    public class FilterGroupedProductCategories : List<Boolable<ProductCategory>>
    {
        //public List<Boolable<ProductCategory>> GroupedSelection { get; set; }

        public FilterGroupedProductCategories(GroupedSelection<ProductCategory> groupedSelection) : base(new List<Boolable<ProductCategory>>())
        {
            if(groupedSelection == null) throw new ArgumentException();
            Parent = groupedSelection;
            //GroupedSelection = new List<Boolable<ProductCategory>>();
            foreach (ProductCategory pc in groupedSelection.GroupedItems)
            {
                this.Add(new Boolable<ProductCategory>(pc));
            }
        }

        public readonly GroupedSelection<ProductCategory> Parent;

        public List<ProductCategory> GetCheckedCategories()
        {
            var list = new List<ProductCategory>();
            foreach (Boolable<ProductCategory> bp in this)
            {
                if(bp.Bool) list.Add(bp.Instance);
            }
            return list;
        }
    }
}
