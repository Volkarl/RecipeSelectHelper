using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "SingleIngredientPreference")]
    public class SingleIngredientPreference : Preference
    {
        [DataMember]
        public int Val { get; set; }
        [DataMember]
        public Product Product { get; set; }

        public SingleIngredientPreference(int val, Product product)
        {
            Val = val;
            Product = product;
            Description += nameof(SingleIngredientPreference) + " | Add " + val + " to product: " + product.Name;
        }

        public override void Calculate(ProgramData pd)
        {
            if (Product == null) return;
            pd.AllProducts.Find(x => x.Equals(Product)).OwnValue.AddValue(Val, this);
            //pd.ProductSubstitutes.FindSubstitutes(Product).ForEach(x => x.OwnValue.AddValue(Val, this));
            // When I look at substitutes again, I may wish to use this like once more. 
            // Atm though, substitutes are only handled by the sortingmethod itself, not by the preferences. 
            // In any case, I'd need an allowSubs bool as input.
        }
    }
}
