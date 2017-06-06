using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "ExpirationDatePreference")]
    public class ExpirationDatePreference : Preference
    {
        [DataMember]
        public int Val { get; set; }

        private static Func<double, double> _valueDecayFormula = x => (-0.4371 * Math.Pow(x, 2)) + (15.851 * x) - 22.42;

        public ExpirationDatePreference(int val)
        {
            Val = val;
            Description += nameof(ExpirationDatePreference) + " | Expiration date value (0 to ≈ 120) multiplied by " + Val;
        }

        public override void Calculate(ProgramData pd, Dictionary<BoughtProduct, uint> amountsInFridge)
        {
            DateTime startTime = DateTime.Now;
            foreach (BoughtProduct bp in pd.AllBoughtProducts)
            {
                int val = CalculateValue(bp.ExpirationData, startTime);
                bp.OwnValue += val;
                bp.CorrespondingProduct.TransferValueToCorrespondingIngredients(val, bp.Amount);
                // For the value of the boughtproducts to be aggregated into the corresponding recipes, the amount/value information is added to 
                // all the ingredients. They then figure out how to combine the different boughtproducts most efficiently, to get the highest value.
                // This also means that if this preference is executed twice, then the same boughtproducts will be used for values twice, 
                // which is usually not good.
            }
        }

        private int CalculateValue(ExpirationInfo exp, DateTime time)
        {
            var decimalExpired = (int)(exp.GetExpiredPercentage(time) * 100);
            if (decimalExpired == 0) return 0;
            double formulaResult = _valueDecayFormula(decimalExpired);
            return Val * Convert.ToInt32(formulaResult);
        }
    }
}