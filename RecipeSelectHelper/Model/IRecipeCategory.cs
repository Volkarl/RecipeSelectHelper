using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    public interface IRecipeCategory
    {
        string Name { get; set; }
        ValueInformation OwnValue { get; }
    }
}