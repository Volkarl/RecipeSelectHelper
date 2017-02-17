using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RecipeSelectHelper.Model
{
    [DataContract(Name = "Ingredient")]
    public class Ingredient : IIngredient
    {
        [DataMember]
        public int AmountNeeded { get; set; }
        [DataMember]
        public Product CorrespondingProduct { get; set; }

        public int Value => OwnValue + CorrespondingProduct.AggregatedValue;

        public int OwnValue { get; set; } = 0;

        public Ingredient(int amountNeeded, Product correspondingProduct)
        {
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
        }
    }
}
