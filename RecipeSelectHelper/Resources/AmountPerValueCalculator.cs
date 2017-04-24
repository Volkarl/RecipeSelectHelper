using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class AmountPerValueCalculator
    {
        public uint AmountNeeded { get; }
        public List<Tuple<int, BoughtProduct>> OrderedValueProduct;

        public AmountPerValueCalculator(uint amountNeeded)
        {
            AmountNeeded = amountNeeded;
            OrderedValueProduct = new List<Tuple<int, BoughtProduct>>();
        }

        public void AddAmountWithValue(int totalValue, BoughtProduct bp)
        {
            if (OrderedValueProduct.IsEmpty())
            {
                OrderedValueProduct.Add(new Tuple<int, BoughtProduct>(totalValue, bp));
                return;
            }

            int i = 0;
            while (totalValue <= OrderedValueProduct[i].Item1 && i < OrderedValueProduct.Count)
            {
                // Find the index where the new tuple belongs
                i++;
            }
            OrderedValueProduct.Insert(i, new Tuple<int, BoughtProduct>(totalValue, bp));
            // The most optimal boughtProducts are, as such, inserted leftmost
            //TODO
            throw new NotImplementedException("REVERSE THE INSERTIONS, I WANT OPTIMAL ONES LEFTMOST"); 
        }

        public int GetOptimalValueCombination()
        {
            int optimalValue = 0;
            long amountNeeded = AmountNeeded;
            foreach (Tuple<int, BoughtProduct> valueBp in OrderedValueProduct)
            {
                amountNeeded -= valueBp.Item2.Amount;
                if (amountNeeded > 0)
                {
                    // If we need more, then add the value and move on
                    optimalValue += valueBp.Item1;
                }
                else if (amountNeeded == 0)
                {
                    // If we need no more, then add the value and break the loop, as we need not check more
                    optimalValue += valueBp.Item1;
                    break;
                }
                else
                {
                    // If we've overshot the necessary amount, then we figure out how large a percentage we didn't overshoot and add only that value
                    uint overshotAmount = (uint) - amountNeeded;
                    uint nonOvershotAmount = valueBp.Item2 - overshotAmount;
                    double percentage = nonOvershotAmount / (double) valueBp.Item2;
                    optimalValue += Convert.ToInt32(valueBp.Item1 * percentage);
                    break;
                }
            }
            return optimalValue;
        }
    }
}
