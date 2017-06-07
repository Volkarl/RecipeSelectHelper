using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class AmountNeededValueCalculator
    {
        public SortedSet<BpValue> OrderedBpValues { get; } = new SortedSet<BpValue>(new SortByValuePerAmount());
        public Product CorrespondingProduct { get; }

        public AmountNeededValueCalculator(Product correspondingProduct)
        {
            CorrespondingProduct = correspondingProduct;
        }

        public OptimalValue GetOptimalValueCombination(Dictionary<BoughtProduct, uint> bpAmountsRemaining, uint amountNeeded)
        {
            // The idea is that we use up the front (most value per amount) BoughtProducts first

            OptimalValue returnVal = new OptimalValue();
            using (var iterator = OrderedBpValues.GetEnumerator())
            {
                // While I still need some amount, and while there is still some left.
                while (amountNeeded != 0 && iterator.MoveNext())
                {
                    if(iterator.Current == null) throw new ArgumentNullException();
                    uint amount = bpAmountsRemaining[iterator.Current.Bp];
                    if (amount != 0)
                    {
                        uint removalAmount = (amount >= amountNeeded) ? amountNeeded : amount;
                        // Done to avoid removing more than allowed.
                        bpAmountsRemaining[iterator.Current.Bp] -= removalAmount;
                        returnVal.Value += iterator.Current.ValuePerAmount * removalAmount;
                        returnVal.AmountSatisfied += removalAmount;
                        amountNeeded -= removalAmount;
                    }
                }
            }

            throw new NotImplementedException((OrderedBpValues.First().ValuePerAmount > OrderedBpValues.Last().ValuePerAmount) + 
                "MAKE SURE THE FIRST VALUE IS ACTUALLY THE HIGHEST ONE!!! OTHERWISE REVERSE THE SORTING.");
            return returnVal;
        }

        public void AddBoughtProduct(BoughtProduct bp)
        {
            if (!OrderedBpValues.Add(new BpValue(bp)))
                throw new ArgumentException($"Could not be added to ordered values: {bp}");
        }
    }

    public class OptimalValue
    {
        public double Value { get; set; }
        public uint AmountSatisfied { get; set; }

        public OptimalValue() : this(0, 0) { }
        public OptimalValue(double value, uint amountSatisfied)
        {
            Value = value;
            AmountSatisfied = amountSatisfied;
        }
    }

    public class SortByValuePerAmount : IComparer<BpValue>
    {
        public int Compare(BpValue x, BpValue y)
        {
            if(x == null || y == null) throw new ArgumentNullException($"{(x == null ? "x" : "y")} is null.");
            return x.ValuePerAmount.CompareTo(y.ValuePerAmount);
        }
    }
}
