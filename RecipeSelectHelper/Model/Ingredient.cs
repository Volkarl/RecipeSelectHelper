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
                if (OwnValueCalculator == null) throw new ArgumentException("The CorrespondingProduct has not sent information about the fitting BoughtProducts.");
                // This means: where is Ingredient supposed to get its value from if it doesn't yet know about any of the Bought Products?
                return OwnValueCalculator.GetOptimalValueCombination();
            }
        }

        public AmountNeededValueCalculator OwnValueCalculator { get; private set; }

        public Ingredient(uint amountNeeded, Product correspondingProduct)
        {
            if(correspondingProduct == null) throw new ArgumentException();
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
            CorrespondingProduct.TransferValueToIngredients += BoughtProductValueTransfered;
        }

        private void BoughtProductValueTransfered(object sender, AmountNeededValueCalculator e)
        {
            OwnValueCalculator = e; 
        }

        public void Clean()
        {
            OwnValueCalculator = null;
            OwnValue = 0;
        }

        public void AggregateValue(Dictionary<BoughtProduct, uint> bpAmountsRemaining)
        {
            OwnValue = OwnValueCalculator.GetOptimalValueCombination(bpAmountsRemaining);
            // TODO CHANGE IT SO THAT AGGREGATE/OWNVALUE/VALUE is like it is for recipes
        }
    }
}
