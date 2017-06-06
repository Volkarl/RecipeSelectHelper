using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecipeSelectHelper.Model;

namespace RecipeSelectHelper.Resources
{
    public class BpValue
    {
        public double ValuePerAmount { get; set; }
        public BoughtProduct Bp { get; set; }

        public BpValue(BoughtProduct product)
        {
            if(product == null) throw new ArgumentException();
            Bp = product;
            ValuePerAmount = (double) Bp.OwnValue / Bp.Amount;
            PercentageOfAmountNeeded = (int)((Bp.Amount / (double) amountLeft) * 100);
        }
    }
}
