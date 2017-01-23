namespace RecipeSelectHelper.Model
{
    public interface IBoughtProduct
    {
        int ID { get; set; }
        int Value { get; set; }
        Product CorrespondingProduct { get; set; }
        ExpirationInfo ExpirationData { get; set; }
    }
}