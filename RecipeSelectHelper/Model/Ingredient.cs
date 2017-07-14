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

        public uint AmountSatisfied { get; private set; }

        public int Value => OwnValue + CorrespondingProduct.Value;

        public int OwnValue { get; set; }

        public AmountNeededValueCalculator OwnValueCalculator { get; private set; }

        private Ingredient() { }

        public Ingredient(uint amountNeeded, Product correspondingProduct)
        {
            if(correspondingProduct == null) throw new ArgumentException();
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
            RebindEvent();
        }

        [OnDeserialized]
        private void RebindEvent(StreamingContext context = default(StreamingContext))
        {
            CorrespondingProduct.TransferValueToIngredients += BoughtProductValueTransfered;
        }

        private void BoughtProductValueTransfered(object sender, AmountNeededValueCalculator e)
        {
            OwnValueCalculator = e; 
        }

        public void Reset()
        {
            OwnValueCalculator = null;
            OwnValue = 0;
            AmountSatisfied = 0;
        }

        public void AggregateBpValues(Dictionary<BoughtProduct, uint> bpAmountsRemaining)
        {
            if (OwnValueCalculator == null) throw new ArgumentNullException(nameof(bpAmountsRemaining));
            // Translated: where is Ingredient supposed to get its value from if it doesn't yet know about any of the Bought Products?

            OptimalValue result = OwnValueCalculator.GetOptimalValueCombination(bpAmountsRemaining, AmountNeeded);
            OwnValue += (int) Math.Round(result.Value);
            AmountSatisfied += result.AmountSatisfied;
        }

        // Deprecated because we want to keep boughtProducts and/or substitutes entirely separate. Even if we use
        // Water in stead of Tomatos to make our Recipe, it doesn't make the Ingredient of the Recipe not-tomatoes.
        //public void AggregateValues(Dictionary<BoughtProduct, uint> bpAmountsRemaining,
        //    SubstituteRelationsDictionary substituteRelations = null)
        //{
        //    // Firstly it tries to find the amount it needs in any of the available boughtProducts, whilst recordning how much it uses up
        //    AggregateBpValues(bpAmountsRemaining);

        //    // Then, if the ingredient has not been satisfied, it fills the rest with the corresponding product or its substitutes
        //    if (AmountSatisfied == 0) return;
        //    if (substituteRelations == null)

        //        AggregateMostValuableSubstituteProduct(substituteRelations);
        //}

        //private void AggregateMostValuableSubstituteProduct(SubstituteRelationsDictionary substituteRelations)
        //{
        //    if(substituteRelations == null) throw new ArgumentNullException(nameof(substituteRelations));

        //    // Remember this is for Product, not BoughtProduct, so the amounts do not matter. Any amount that we can't satisfy 
        //    // with BoughtProducts, we satisfy with the most valuable substitute (including the corresponding product itself)
        //    List<Product> subs = substituteRelations.FindSubstitutes(CorrespondingProduct);
        //    subs.Add(CorrespondingProduct);

        //    Product bestProduct;
        //    int maxVal = subs.Max(x => x.Value);
        //    if (maxVal == CorrespondingProduct.Value) bestProduct = CorrespondingProduct;
        //    else bestProduct = subs.Find(x => x.Value == maxVal);

        //    double decimalAmountRequired = 1 - AmountSatisfied / AmountNeeded;
        //    OwnValue += (int) Math.Round(decimalAmountRequired * bestProduct.Value);
        //}
    }
}
