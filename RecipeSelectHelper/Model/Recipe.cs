using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Recipe")]
    public class Recipe : IRecipe
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Instruction { get; set; }

        public int Value { get; private set; }

        public int OwnValue { get; set; }

        [DataMember]
        public List<Ingredient> Ingredients { get; set; }
        [DataMember]
        public List<RecipeCategory> Categories { get; set; }
        [DataMember]
        public List<GroupedRecipeCategory> GroupedCategories { get; set; }

        public string CategoriesAsString
        {
            get
            {
                if (Categories == null)
                {
                    return string.Empty;
                }
                return string.Join(", ", Categories.ConvertAll(x => x.Name));
            }
        }

        public string GroupedCategoriesAsString
        {
            get
            {
                if (GroupedCategories == null || GroupedCategories.IsEmpty()) return String.Empty;
                List<List<Boolable<RecipeCategory>>> grcs = GroupedCategories.ConvertAll(x => x.GroupedRc);
                var grcNames = new List<String>();
                foreach (List<Boolable<RecipeCategory>> groupedRc in grcs)
                {
                    foreach (Boolable<RecipeCategory> boolableRc in groupedRc)
                    {
                        if (boolableRc.Bool) grcNames.Add(boolableRc.Instance.Name);
                    }
                }
                return string.Join(", ", grcNames);
            }
        }

        private Recipe() { }

        public Recipe(string name, string description = null, string instruction = null, List<Ingredient> ingredients = null, List<RecipeCategory> categories = null, List<GroupedRecipeCategory> groupedCategories = null)
        {
            this.Name = name;
            this.Description = description ?? String.Empty;
            this.Instruction = instruction ?? String.Empty;
            this.Ingredients = ingredients ?? new List<Ingredient>();
            this.Categories = categories ?? new List<RecipeCategory>();
            this.GroupedCategories = groupedCategories ?? new List<GroupedRecipeCategory>();
            this.Value = 0;

            // Lots of exceptins here if something is wrong. Check also if selections are correct in groupedcategories.
        }

        public void AggregateValue()    
        {
            // This is not combined with value property because I don't want to recalculate until I click "sort".
            int val = OwnValue;
            foreach (RecipeCategory recipeCategory in Categories)
            {
                val += recipeCategory.OwnValue;
            }

            Dictionary<BoughtProduct, uint> bpAmountsRemaining = CreateDictForBpAmounts();
            // This dictionary will be used to keep track of how much and which boughtProducts our ingredients use 

            foreach (Ingredient ingredient in Ingredients)
            {
                ingredient.AggregateBpValues(bpAmountsRemaining);
                val += ingredient.Value;
            }
            this.Value = val;
        }

        private Dictionary<BoughtProduct, uint> CreateDictForBpAmounts()
        {
            Dictionary<BoughtProduct, uint> bpAmountsRemaining = new Dictionary<BoughtProduct, uint>();
            List<AmountNeededValueCalculator> addedCalculators = new List<AmountNeededValueCalculator>();
            foreach (Ingredient ingredient in Ingredients)
            {
                if (!addedCalculators.Contains(ingredient.OwnValueCalculator))
                {
                    ingredient.OwnValueCalculator.OrderedBpValues.ToList().ConvertAll(x => x.Bp).ForEach(y => bpAmountsRemaining.Add(y, y.Amount));
                    addedCalculators.Add(ingredient.OwnValueCalculator);
                }
            }
            return bpAmountsRemaining;
        }

        public void Clean()
        {
            OwnValue = 0;
            Ingredients.ForEach(x => x.Clean());
        }
    }
}
