using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class BpValueInformation
    {
        public double ValuePerAmount { get; set; }
        public BoughtProduct Product { get; set; }
        public int PercentageOfAmountNeeded { get; set; }

        private int _amountNeeded;

        public BpValueInformation(BoughtProduct product, int amountNeeded)
        {
            if(product == null) throw new ArgumentException();
            Product = product;
            ValuePerAmount = (double) Product.OwnValue / Product.Amount;
            _amountNeeded = amountNeeded;
            PercentageOfAmountNeeded = (int)((Product.Amount / (double) amountNeeded) * 100);
        }
    }
}
