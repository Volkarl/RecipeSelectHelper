using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public class RecipeCategory : IRecipeCategory
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public RecipeCategory(string name)
        {
            Name = name;
        }
    }
}
