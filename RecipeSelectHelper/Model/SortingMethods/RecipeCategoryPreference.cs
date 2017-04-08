using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "RecipeCategoryPreference")]
    public class RecipeCategoryPreference : Preference
    {
        [DataMember]
        public int Val { get; set; }
        [DataMember]
        public RecipeCategory RecipeCategory { get; set; }

        public RecipeCategoryPreference(int val, RecipeCategory productCategory)
        {
            Val = val;
            RecipeCategory = productCategory;
            Description += nameof(RecipeCategoryPreference) + " | Add " + val + " to all recipes of category: " + RecipeCategory.Name;
        }

        public override void Calculate(ProgramData pd)
        {
            if (RecipeCategory == null) return;
            pd.AllRecipeCategories.Find(y => y.Equals(RecipeCategory)).OwnValue += Val;
        }
    }
}
