namespace RecipeSelectHelper.Model
{
    public interface IIngredient
    {
        int AmountNeeded { get; set; }
        int Value { get; set; }
        Product CorrespondingProduct { get; set; }
    }
}