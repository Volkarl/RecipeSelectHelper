namespace RecipeSelectHelper.Model
{
    public interface IBoughtProduct
    {
        int OwnValue { get; set; }
        Product CorrespondingProduct { get; set; }
        ExpirationInfo ExpirationData { get; set; }
        uint Amount { get; set; }
    }
}