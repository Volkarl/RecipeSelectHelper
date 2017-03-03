using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class RecipeWithPercentageScore
    {
        public int MaxValue { get; private set; }
        public double PercentageValue { get; private set; }
        public Recipe CorrespondinRecipe { get; set; }

        public RecipeWithPercentageScore(Recipe recipe)
        {
            CorrespondinRecipe = recipe;
            MaxValue = 0;
            PercentageValue = 0;
        }
        public RecipeWithPercentageScore(Recipe recipe, int maxValue)
        {
            CorrespondinRecipe = recipe;
            MaxValue = maxValue;
            PercentageValue = (CorrespondinRecipe.Value / MaxValue) * 100;
        }
    }
}
