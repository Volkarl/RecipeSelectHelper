using System.Collections.Generic;

namespace RecipeSelectHelper.Model
{
    public interface IRecipe
    {
        string Name { get; set; }
        string Description { get; set; }
        string Instruction { get; set; }
        int ID { get; }
        int Value { get; set; }
        List<IIngredient> Ingredients { get; set; }
        List<IRecipeCategory> Categories { get; set; }
        string CategoriesAsString { get; }
    }
}