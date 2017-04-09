using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    public class AmountPerValueCalculator
    {
        public uint AmountNeeded { get; }
        public List<Tuple<int, uint>> OrderedValueAmounts;

        public AmountPerValueCalculator(uint amountNeeded)
        {
            AmountNeeded = amountNeeded;
            OrderedValueAmounts = new List<Tuple<int, uint>>();
        }

        public void AddAmountWithValue(int totalValue, uint totalAmount)
        {
            if (OrderedValueAmounts.IsEmpty())
            {
                OrderedValueAmounts.Add(new Tuple<int, uint>(totalValue, totalAmount));
            }

            int i = 0;
            while (totalValue <= OrderedValueAmounts[i].Item1 && i < OrderedValueAmounts.Count)
            {
                // Find the index where the new tuple belongs
                i++;
            }
            OrderedValueAmounts.Insert(i, new Tuple<int, uint>(totalValue, totalAmount));
        }

        public int GetOptimalValueCombination()
        {
            int optimalValue = 0;
            long amountNeeded = AmountNeeded;
            foreach (Tuple<int, uint> valueAmount in OrderedValueAmounts)
            {
                amountNeeded -= valueAmount.Item2;
                if (amountNeeded > 0)
                {
                    // If we need more, then add the value and move on
                    optimalValue += valueAmount.Item1;
                }
                else if (amountNeeded == 0)
                {
                    // If we need no more, then add the value and break the loop, as we need not check more
                    optimalValue += valueAmount.Item1;
                    break;
                }
                else
                {
                    // If we've overshot the necessary amount, then we figure out how large a percentage we didn't overshoot and add only that value
                    uint overshotAmount = (uint) - amountNeeded;
                    uint nonOvershotAmount = valueAmount.Item2 - overshotAmount;
                    double percentage = nonOvershotAmount / (double) valueAmount.Item2;
                    optimalValue += Convert.ToInt32(valueAmount.Item1 * percentage);
                    break;
                }
            }
            return optimalValue;
        }
    }
}
