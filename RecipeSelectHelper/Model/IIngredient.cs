namespace RecipeSelectHelper.Model
{
    public interface IIngredient
    {
        uint AmountNeeded { get; set; }
        int Value { get; }
        int OwnValue { get; }
        Product CorrespondingProduct { get; set; }
    }
}