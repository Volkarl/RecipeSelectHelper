using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    public class Recipe : IRecipe
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public int ID { get; private set; }
        public int Value { get; set; }
        public List<IIngredient> Ingredients { get; set; }
        public List<IRecipeCategory> Categories { get; set; }

        public string CategoriesAsString
        {
            get
            {
                if (Categories == null)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Join(", ", Categories.ConvertAll(x => x.Name));
                }
            }
        }

        private static int _recipeCreatedNumber = 0;

        public Recipe(string name, string description = null, string instruction = null, List<IIngredient> ingredients = null, List<IRecipeCategory> categories = null)
        {
            this.Name = name;
            this.ID = _recipeCreatedNumber++;
            this.Description = description;
            this.Ingredients = ingredients;
            Categories = categories;
        }
    }
}
