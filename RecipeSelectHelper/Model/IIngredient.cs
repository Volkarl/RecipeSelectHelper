using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IIngredient
    {
        Amount AmountNeeded { get; set; }
        int Value { get; }
        ValueInformation OwnValue { get; }
        Product CorrespondingProduct { get; set; }
    }
}