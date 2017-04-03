using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class FilterRecipeCategory : Boolable<RecipeCategory>
    {
        public FilterRecipeCategory(RecipeCategory instance) : base(instance)
        {
        }
    }
}
