using System;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class RecipeWithPercentageScore
    {
        public int MaxValue { get; private set; }
        public double PercentageValue { get; private set; }
        public Recipe CorrespondinRecipe { get; set; }

        public RecipeWithPercentageScore(Recipe recipe, int maxValue = 0)
        {
            if (maxValue == 0) maxValue = 1; // To avoid dividing by zero
            CorrespondinRecipe = recipe;
            MaxValue = maxValue;
            PercentageValue = (CorrespondinRecipe.Value / MaxValue) * 100;
        }
    }
}
