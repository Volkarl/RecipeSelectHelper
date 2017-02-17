using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Recipe")]
    public class Recipe : IRecipe
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Instruction { get; set; }
        //[DataMember]
        //public int ID { get; private set; } 

        public int Value { get; set; } = 0;

        public int OwnValue { get; set; } = 0;

        [DataMember]
        public List<Ingredient> Ingredients { get; set; }
        [DataMember]
        public List<RecipeCategory> Categories { get; set; }

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

        //private static int _recipeCreatedNumber = 0;
        // Counter starts at 0 at every new program execution, it needs to get this from settings.settings? 

        public Recipe(string name, string description = null, string instruction = null, List<Ingredient> ingredients = null, List<RecipeCategory> categories = null)
        {
            this.Name = name;
            //this.ID = _recipeCreatedNumber++;
            this.Description = description ?? String.Empty;
            this.Instruction = instruction ?? String.Empty;
            this.Ingredients = ingredients ?? new List<Ingredient>();
            this.Categories = categories ?? new List<RecipeCategory>();
            this.Value = 0;
        }

        public void AggregateValue()    //combine this with value prop
        {
            int val = OwnValue;
            foreach (RecipeCategory recipeCategory in Categories)
            {
                val += recipeCategory.Value;
            }
            foreach (Ingredient ingredient in Ingredients)
            {
                val += ingredient.Value;
            }
            this.Value = val;
        }
    }
}
