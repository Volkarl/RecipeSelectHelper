using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model.SortingMethods
{
    //[KnownType(typeof(Preference))]    // Try removing this!
    [KnownType(typeof(ExpirationDatePreference))]
    [KnownType(typeof(IngredientsOwnedPreference))]
    [KnownType(typeof(ProductCategoryPreference))]
    [KnownType(typeof(RecipeCategoryPreference))]
    [KnownType(typeof(SingleIngredientPreference))]
    [DataContract(Name = "Preference")]
    public abstract class Preference : IPreference
    {
        [DataMember]
        public string Description { get; set; }

        protected Preference(string description = null)
        {
            Description = description ?? string.Empty;
        }

        public abstract void Calculate(ProgramData pd);
    }
}