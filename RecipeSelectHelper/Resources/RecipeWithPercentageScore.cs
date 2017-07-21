using System;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class RecipeWithPercentageScore
    {
        public int MaxValue { get; }
        public double PercentageValue { get; }
        public Recipe CorrespondingRecipe { get; set; }

        public RecipeWithPercentageScore(Recipe recipe, int maxValue = 1)
        {
            CorrespondingRecipe = recipe;
            MaxValue = maxValue == 0 ? 1 : maxValue; // To avoid dividing by zero
            PercentageValue = (CorrespondingRecipe.Value / MaxValue) * 100;
        }
    }
}
