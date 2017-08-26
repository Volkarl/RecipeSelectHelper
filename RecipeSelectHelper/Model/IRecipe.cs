using System.Collections.Generic;
using RecipeSelectHelper.Resources;
using RecipeSelectHelper.Resources.ConcreteTypesForXaml;

namespace RecipeSelectHelper.Model
{
    public interface IRecipe
    {
        string Name { get; set; }
        string Description { get; set; }
        StringList Instructions { get; set; }
        int Value { get; }
        ValueInformation OwnValue { get; }
        List<Ingredient> Ingredients { get; set; }
        List<RecipeCategory> Categories { get; set; }
        string CategoriesAsString { get; }

        void AggregateValue();
    }
}