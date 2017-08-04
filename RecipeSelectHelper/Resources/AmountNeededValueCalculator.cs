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
                    BpValue current = iterator.Current;
                    uint amount = bpAmountsRemaining[current.Bp];
                    if (amount != 0)
                    {
                        uint removalAmount = (amount >= amountNeeded) ? amountNeeded : amount;
                        // Done to avoid removing more than allowed.
                        bpAmountsRemaining[current.Bp] -= removalAmount;
                        returnVal.AddValue(current.Bp, current.ValuePerAmount * removalAmount, removalAmount);
                        amountNeeded -= removalAmount;
                    }
                }
            }
            return returnVal;
        }

        public void AddBoughtProduct(BoughtProduct bp)
        {
            if (!OrderedBpValues.Add(new BpValue(bp)))
            {

                var v = OrderedBpValues.Comparer.Compare(new BpValue(bp), OrderedBpValues.First());
                throw new ArgumentException($"Error: Duplicate BoughtProduct was not added to ordered values: {bp}" +
                                            $"{OrderedBpValues.Comparer}");
            }
        }
    }

    public class OptimalValue
    {
        public double Value { get; private set; }
        public uint AmountSatisfied { get; private set; }
        public List<BpValueSourceInfo> ValueLog { get; } = new List<BpValueSourceInfo>();

        public OptimalValue() : this(0, 0) { }
        public OptimalValue(double value, uint amountSatisfied)
        {
            Value = value;
            AmountSatisfied = amountSatisfied;
        }

        public void AddValue(BoughtProduct source, double totalValueToAdd, uint amountToAdd)
        {
            ValueLog.Add(new BpValueSourceInfo(source, totalValueToAdd, amountToAdd));
            Value += totalValueToAdd;
            AmountSatisfied += amountToAdd;
        }
    }

    public class BpValueSourceInfo
    {
        public BpValueSourceInfo(BoughtProduct bp, double valueAdded, uint amountSatisfied)
        {
            Bp = bp;
            ValueAdded = valueAdded;
            AmountSatisfied = amountSatisfied;
        }

        public BoughtProduct Bp { get; }
        public double ValueAdded { get; }
        public uint AmountSatisfied { get; }
    }

    public class SortByValuePerAmount : IComparer<BpValue>
    {
        public int Compare(BpValue x, BpValue y)
        {
            if (x == null || y == null) throw new ArgumentNullException($"{(x == null ? "x" : "y")} is null.");
            if (x == y) return 0; // Reference equal objects are equal
            var comparison = x.ValuePerAmount.CompareTo(y.ValuePerAmount) * (-1);
            return comparison == 0 ? 1 : comparison;
            // If both values are equal, one is placed below the other.
        }
    }
}
