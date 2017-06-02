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

        public SortingMethod(string name, List<Preference> preferences)
        {
            if(String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException();
            Name = name;
            preferences = preferences ?? new List<Preference>();
            preferences = ApplyPreferenceOptimizations(preferences);
            preferences = ApplyPreferenceOrderingRules(preferences);
            Preferences = preferences;
        }

        private List<Preference> ApplyPreferenceOptimizations(List<Preference> preferences)
        {
            // Combine all expirationDatePreferences into one, to avoid errors and unecessary calculations while computing the values. 
            List<ExpirationDatePreference> expirPrefs = preferences.FindAll(x => x is ExpirationDatePreference).ConvertAll(y => y as ExpirationDatePreference);
            int combinedVal = expirPrefs.Sum(x => x.Val);
            var combinedExpirPref = new ExpirationDatePreference(combinedVal);
            preferences.RemoveElements(expirPrefs.ConvertAll(x => x as Preference));
            preferences.Add(combinedExpirPref);
            return preferences;
        }

        private List<Preference> ApplyPreferenceOrderingRules(List<Preference> preferences)
        {
            // AllRecipeIngredientsInFridgePreference MUST be after anything that manipulates ingredientAmounts
            // TODO

            for (var i = preferences.Count - 1; i >= 0; i--)
            {
                if(preferences[i] is ExpirationDatePreference) preferences.MoveElement(i, preferences.Count);
                // Go backwards through preferences and move these to the very back
            }
            return preferences;
        }

        public void Execute(ProgramData data)
        {
            if (Preferences == null || data == null) return;

            Dictionary<BoughtProduct, uint> TempAmount = CreateTempAmountDict(data.AllBoughtProducts);

            double percentPerStep = 50 / (double)Preferences.Count;
            double percentageFinished = 0;
            foreach (Preference preference in Preferences)
            {
                preference.Calculate(data);
                percentageFinished += percentPerStep;
                ProgressChanged?.Invoke(this, percentageFinished);
            }
            foreach (Recipe recipe in data.AllRecipes)
            {
                recipe.AggregateValue();
                percentageFinished += percentPerStep;
                ProgressChanged?.Invoke(this, percentageFinished);
            }
        }

        private Dictionary<BoughtProduct, uint> CreateTempAmountDict(List<BoughtProduct> boughtProducts)
        {
            return boughtProducts.ToDictionary(x => x, x => x.Amount); 
            //TODO: If there are ever issues, check if it truly passes amount by value, and not reference
        }
    }
}