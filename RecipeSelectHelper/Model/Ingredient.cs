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

        public int OwnValue
        {
            get
            {
                if (_ownValueCalculator == null) _ownValueCalculator = new AmountPerValueCalculator(AmountNeeded);
                return _ownValueCalculator.GetOptimalValueCombination();
            }
        }

        private AmountPerValueCalculator _ownValueCalculator;

        public Ingredient(uint amountNeeded, Product correspondingProduct)
        {
            if(correspondingProduct == null) throw new ArgumentException();
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
            CorrespondingProduct.IncreaseIngredientValue += IngredientValueIncreased;
        }

        private void IngredientValueIncreased(object sender, Tuple<int,BoughtProduct> e)
        {
            if(_ownValueCalculator == null) _ownValueCalculator = new AmountPerValueCalculator(AmountNeeded);
            // It is done this way, because the constructor is not invoked during serialization.
            _ownValueCalculator.AddAmountWithValue(e.Item1, e.Item2);
        }
    }
}
