using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
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
            Preferences = preferences ?? new List<Preference>();
            ApplyPreferenceOrderingRules(Preferences);
        }

        private void ApplyPreferenceOrderingRules(List<Preference> preferences)
        {
            // If there are multiple ExpirationDatePreferences, then those have to be combined, using their Values, otherwise they doubleoverride the ingredientAmounts
            // AllRecipeIngredientsInFridgePreference MUST be after anything that manipulates ingredientAmounts, USE MoveElement() extension

            throw new NotImplementedException();
        }

        public void Execute(ProgramData data)
        {
            if (Preferences == null || data == null) return;
            double percentPerPreference = 100 / (double)Preferences.Count;
            double percentageFinished = 0;
            foreach (Preference preference in Preferences)
            {
                preference.Calculate(data);
                percentageFinished += percentPerPreference;
                ProgressChanged?.Invoke(this, percentageFinished);
            }
            data.AllRecipes.ForEach(x => x.AggregateValue()); 
        }
    }
}