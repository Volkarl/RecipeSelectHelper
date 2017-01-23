using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Ingredient")]
    public class Ingredient : IIngredient
    {
        [DataMember]
        public int AmountNeeded { get; set; }
        [DataMember]
        public Product CorrespondingProduct { get; set; }
        [DataMember]
        public int Value { get; set; }

        public Ingredient(int amountNeeded, Product correspondingProduct)
        {
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
        }
    }
}
