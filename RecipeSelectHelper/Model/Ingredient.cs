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
    [DataContract(Name = "Ingredient")]
    public class Ingredient : IIngredient
    {
        [DataMember]
        public uint AmountNeeded { get; set; }
        [DataMember]
        public Product CorrespondingProduct { get; set; }

        public int Value => OwnValue + CorrespondingProduct.Value;

        public int OwnValue => _ownValueCalculator.GetOptimalValueCombination();

        private AmountPerValueCalculator _ownValueCalculator;

        public Ingredient(uint amountNeeded, Product correspondingProduct)
        {
            if(correspondingProduct == null) throw new ArgumentException();
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
            CorrespondingProduct.IncreaseIngredientValue += IngredientValueIncreased;
            _ownValueCalculator = new AmountPerValueCalculator(amountNeeded);
        }

        private void IngredientValueIncreased(object sender, Tuple<int,uint> e)
        {
            _ownValueCalculator.AddAmountWithValue(e.Item1, e.Item2);
        }
    }
}
