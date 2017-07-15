using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IProductCategory
    {
        string Name { get; set; }
        ValueInformation OwnValue { get; }
    }
}