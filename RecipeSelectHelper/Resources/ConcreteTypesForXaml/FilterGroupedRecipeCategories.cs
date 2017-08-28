using System;
using System.Collections.Generic;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources.ConcreteTypesForXaml
{
    public class FilterGroupedRecipeCategories : List<Boolable<RecipeCategory>>
    {
        public readonly GroupedSelection<RecipeCategory> Parent;

        public FilterGroupedRecipeCategories(GroupedSelection<RecipeCategory> groupedSelection) : base(new List<Boolable<RecipeCategory>>())
        {
            if (groupedSelection == null) throw new ArgumentException();
            Parent = groupedSelection;
            foreach (RecipeCategory pc in groupedSelection.GroupedItems)
            {
                this.Add(new Boolable<RecipeCategory>(pc));
            }
        }

        public List<RecipeCategory> GetCheckedCategories()
        {
            var list = new List<RecipeCategory>();
            foreach (Boolable<RecipeCategory> bp in this)
            {
                if (bp.Bool) list.Add(bp.Instance);
            }
            return list;
        }
    }
}
