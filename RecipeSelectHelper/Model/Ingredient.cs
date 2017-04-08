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
        public uint AmountNeeded { get; set; }
        [DataMember]
        public Product CorrespondingProduct { get; set; }

        public int Value => OwnValue + CorrespondingProduct.Value;

        public int OwnValue { get; set; } = 0;

        public Ingredient(uint amountNeeded, Product correspondingProduct)
        {
            if(correspondingProduct == null) throw new ArgumentException();
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
            CorrespondingProduct.IncreaseIngredientValue += CorrespondingProduct_IncreaseIngredientValue;
        }

        private void CorrespondingProduct_IncreaseIngredientValue(object sender, Tuple<int,uint> e)
        {
            // Implement the amountNeeded-vs.amountFulfilled-thingy
            OwnValue += e;
        }
    }
}
