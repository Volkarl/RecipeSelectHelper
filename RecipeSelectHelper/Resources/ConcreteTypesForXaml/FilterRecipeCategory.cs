using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources.ConcreteTypesForXaml
{
    public class FilterRecipeCategory : Boolable<RecipeCategory>
    {
        public FilterRecipeCategory(RecipeCategory instance) : base(instance)
        {
        }
    }
}
