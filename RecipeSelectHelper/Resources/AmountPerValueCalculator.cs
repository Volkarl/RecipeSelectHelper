using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class AmountPerValueCalculator
    {
        public int AmountNeeded { get; }
        public BpValueInformation OrderedValueProducts { get; set; }

        public AmountPerValueCalculator(int amountNeeded)
        {
            AmountNeeded = amountNeeded;
            OrderedValueProducts = new List<Tuple<int, BoughtProduct>>();
        }

        public void AddAmountWithValue(int totalValue, BoughtProduct bp)
        {
            if (OrderedValueProducts.IsEmpty())
            {
                OrderedValueProducts.Add(new Tuple<int, BoughtProduct>(totalValue, bp));
                return;
            }

            int i = 0;
            int maxIndex = OrderedValueProducts.Count - 1;
            while (totalValue <= OrderedValueProducts[i].Item1 && i < maxIndex)
            {
                // Find the index where the new tuple belongs
                i++;
            }

            OrderedValueProducts.Insert(i, new Tuple<int, BoughtProduct>(totalValue, bp));
            // The boughtProducts that are the most valuable per weight are, as such, inserted leftmost
        }

        // HERE ADD PROPERTIES FOR OPTIMAL-COMBINATIONS

        public int OptimalValue => GetOptimalValueCombination();

        public int PercentageTotalIngredientsGathered => sfsfPercentageTotalIngredientsGathered();

        public int sfsfPercentageTotalIngredientsGathered()
        {
            // (int) (amountneeded / (double) optimalCombinationAmount (which is max 100 and min 0) * 100)
        }

        public int GetOptimalValueCombination()
        {
            int optimalValue = 0;
            long amountNeeded = AmountNeeded;
            foreach (Tuple<int, BoughtProduct> valueBp in OrderedValueProducts)
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
                    uint overshotAmount = (uint) -amountNeeded;
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
