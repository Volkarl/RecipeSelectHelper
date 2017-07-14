using System.Collections.Generic;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IRecipe
    {
        string Name { get; set; }
        string Description { get; set; }
        string Instruction { get; set; }
        //int ID { get; }             //Remove?
        int Value { get; }
        ValueInformation OwnValue { get; }
        List<Ingredient> Ingredients { get; set; }
        List<RecipeCategory> Categories { get; set; }
        string CategoriesAsString { get; }

        void AggregateValue();
    }
}