using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "IngredientsOwnedPreference")]
    public class IngredientsOwnedPreference : Preference
    {
        [DataMember]
        public int Val { get; set; }

        public IngredientsOwnedPreference(int val)
        {
            this.Val = val;
            Description += nameof(IngredientsOwnedPreference) + " | Add " + val + " to every owned ingredient";
        }

        public override void Calculate(ProgramData pd)
        {
            pd.AllBoughtProducts.ForEach(y => y.OwnValue.AddValue(Val, this));
        }
    }
}