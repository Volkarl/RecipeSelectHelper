using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IIngredient
    {
        uint AmountNeeded { get; set; }
        int Value { get; }
        ValueInformation OwnValue { get; }
        Product CorrespondingProduct { get; set; }
    }
}