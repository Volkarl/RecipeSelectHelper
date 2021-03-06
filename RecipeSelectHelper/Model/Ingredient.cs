﻿using System;
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
        public Amount AmountNeeded { get; set; }
        [DataMember]
        public Product CorrespondingProduct { get; set; }

        public Amount AmountSatisfied { get; private set; }

        public int PercentageOfTotalIngredientsGathered => AmountNeeded.ToGrams == 0 ? 0 : Convert.ToInt32(((double) AmountSatisfied.ToGrams / AmountNeeded.ToGrams) * 100);

        public int Value => OwnValue.GetValue + CorrespondingProduct.Value;

        private ValueInformation _ownValue = new ValueInformation();
        public ValueInformation OwnValue => _ownValue ?? (_ownValue = new ValueInformation()); //Needed for deserialization

        public AmountNeededValueCalculator OwnValueCalculator { get; private set; }

        public List<BpValueSourceInfo> BpValueLog { get; private set; } // Shows where the bp-values come from

        public List<ProgressInfo> BpCompositionInfo
        {
            get
            {
                if(BpValueLog == null || BpValueLog.IsEmpty()) return new List<ProgressInfo>();
                return new List<ProgressInfo>(BpValueLog.Select(x => new ProgressInfo((int) x.AmountSatisfied,
                    AmountSatisfied.ToGrams, x.Bp.CorrespondingProduct.Name)));
            }
        }

        private Ingredient() { }

        public Ingredient(Amount amountNeeded, Product correspondingProduct)
        {
            if(correspondingProduct == null) throw new ArgumentException();
            AmountNeeded = amountNeeded;
            CorrespondingProduct = correspondingProduct;
            BindEvent();
        }

        [OnDeserialized]
        private void BindEvent(StreamingContext context = default(StreamingContext))
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
            OwnValue.Reset();
            AmountSatisfied = new Amount(MeasurementUnit.Unit.Gram, 0);
            BpValueLog = null;
        }

        public void AggregateBpValues(Dictionary<BoughtProduct, uint> bpAmountsRemaining)
        {
            if (OwnValueCalculator == null) throw new ArgumentNullException(nameof(bpAmountsRemaining));
            // Translated: where is Ingredient supposed to get its value from if it doesn't yet know about any of the Bought Products?

            OptimalValue result = OwnValueCalculator.GetOptimalValueCombination(bpAmountsRemaining, Convert.ToUInt32(AmountNeeded.ToGrams));
            //todo remove this uint shit, but make sure the numbers are not below zero or convert in some safe way.
            RecordResult(result);
        }

        private void RecordResult(OptimalValue result)
        {
            OwnValue.AddValue((int)Math.Round(result.Value), new AggregatedValue(this));
            AmountSatisfied.AddAmount(result.AmountSatisfied); 
            BpValueLog = result.ValueLog;
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
