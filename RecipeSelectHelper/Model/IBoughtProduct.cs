using RecipeSelectHelper.Model.SortingMethods;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IBoughtProduct
    {
        ValueInformation OwnValue { get; }
        Product CorrespondingProduct { get; set; }
        ExpirationInfo ExpirationData { get; set; }
        uint Amount { get; set; }
    }
}