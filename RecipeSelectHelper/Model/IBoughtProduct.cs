using RecipeSelectHelper.Model.SortingMethods;

namespace RecipeSelectHelper.Model
{
    public interface IBoughtProduct
    {
        int OwnValue { get; }
        void AddValue(int value, Preference sender);
        Product CorrespondingProduct { get; set; }
        ExpirationInfo ExpirationData { get; set; }
        uint Amount { get; set; }
    }
}