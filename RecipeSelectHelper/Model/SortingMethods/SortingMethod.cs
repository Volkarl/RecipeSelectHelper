using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using RecipeSelectHelper.Resources;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "SortingMethod")]
    public class SortingMethod : ISortingMethod
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Preference> Preferences { get; set; }

        public event EventHandler<double> ProgressChanged;

        private SortingMethod() { }
        public SortingMethod(string name, List<Preference> preferences)
        {
            if(String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name");
            Name = name;
            //preferences = preferences ?? new List<Preference>();
            //preferences = ApplyPreferenceOptimizations(preferences);
            //preferences = ApplyPreferenceOrderingRules(preferences);
            Preferences = preferences ?? new List<Preference>();
        }

        private List<Preference> ApplyPreferenceOptimizations(List<Preference> preferences)
        {
            // Combine all expirationDatePreferences into one, to avoid errors and unecessary calculations while computing the values. 
            return preferences.CombineElements(x => x is ExpirationDatePreference,
                x => new ExpirationDatePreference(x.Sum(y => ((ExpirationDatePreference) y).Val)));
        }

        private List<Preference> ApplyPreferenceOrderingRules(List<Preference> preferences)
        {
            // AllRecipeIngredientsInFridgePreference MUST be after anything that manipulates ingredientAmounts
            // TODO

            for (var i = preferences.Count - 1; i >= 0; i--)
            {
                if(preferences[i] is ExpirationDatePreference) preferences.MoveElement(i, preferences.LastIndex());
                // Go backwards through preferences and move these to the very back
            }
            return preferences;
        }

        public void Execute(ProgramData data, bool allowSubstitutes)
        {
            if (Preferences == null || data == null) return;

            double percentPerStep = 50 / (double)Preferences.Count;
            double percentageFinished = 0;
            foreach (Preference preference in Preferences)
            {
                preference.Calculate(data);
                percentageFinished += percentPerStep;
                ProgressChanged?.Invoke(this, percentageFinished);
            }

            TransferProductValueToIngredients(data.AllProducts, data.AllBoughtProducts, allowSubstitutes, data.ProductSubstitutes);
            // This is done because Products and BoughtProducts are separate from Ingredients. 
            // To aggregate their value into the Recipe values, the Ingredients are sent information about the BoughtProducts.

            foreach (Recipe recipe in data.AllRecipes)
            {
                recipe.AggregateValue();
                percentageFinished += percentPerStep;
                ProgressChanged?.Invoke(this, percentageFinished);
            }
        }

        private void TransferProductValueToIngredients(List<Product> products, List<BoughtProduct> boughtProducts, bool substitutesEnabled, SubstituteRelationsDictionary substituteRelations)
        {
            Dictionary<Product, AmountNeededValueCalculator> valCalcs = CreateEmptyValueCalculators(products);
            foreach (BoughtProduct bp in boughtProducts) valCalcs[bp.CorrespondingProduct].AddBoughtProduct(bp);
            if (substitutesEnabled) AddSubstituteBpsToValCalcs(valCalcs, products, boughtProducts, substituteRelations);
            foreach (Product p in products) p.TransferValueToCorrespondingIngredients(valCalcs[p]);
            // The result of this is that every ingredient in every recipe now holds a reference to the single 
            // Value Calculator belonging to the Ingredient's Corresponding Product.
            // If substitutes are enabled, every Value Calculator also contains all of its substitutes' bought products.
        }

        private void AddSubstituteBpsToValCalcs(Dictionary<Product, AmountNeededValueCalculator> valCalcs, List<Product> products, 
            List<BoughtProduct> bps, SubstituteRelationsDictionary substituteRelations)
        {
            ILookup<Product, BoughtProduct> bpLookup = bps.ToLookup(x => x.CorrespondingProduct);

            foreach (Product p in products)
            {
                // TODO Perhaps add a progress counter here?
                foreach (Product sub in substituteRelations.FindSubstitutes(p))
                    foreach (BoughtProduct bp in bpLookup[sub]) valCalcs[p].AddBoughtProduct(bp);
            }
        }

        private Dictionary<Product, AmountNeededValueCalculator> CreateEmptyValueCalculators(List<Product> products)
        {
            List<AmountNeededValueCalculator> emptyCalculators = new List<AmountNeededValueCalculator>();
            foreach (Product x in products.Distinct()) emptyCalculators.Add(new AmountNeededValueCalculator(x));
            return emptyCalculators.ToDictionary(x => x.CorrespondingProduct);
        }

    }
}