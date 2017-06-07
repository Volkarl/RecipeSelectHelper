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
            if (maxValue == 0) maxValue = 1; // To avoid dividing by zero
            CorrespondingRecipe = recipe;
            MaxValue = maxValue;
            PercentageValue = (CorrespondingRecipe.Value / MaxValue) * 100;
        }
    }
}
