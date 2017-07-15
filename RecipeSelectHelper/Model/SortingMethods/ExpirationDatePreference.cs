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

        private static readonly Func<double, double> ValueDecayParabola = x => (-0.4371 * Math.Pow(x, 2)) + (15.851 * x) - 22.42;

        private static double root1 = 1.47437;
        private static double root2 = 34.7896;
        private static double maxValue = 121.285;

        private double ValueDecayFormula(double decimalExpired)
        {
            if (decimalExpired < root1) return 0;
            if (decimalExpired > root2) return maxValue;
            return ValueDecayParabola(decimalExpired);
        }

        public ExpirationDatePreference(int val)
        {
            Val = val;
            Description += nameof(ExpirationDatePreference) + " | Expiration date value (0 to ≈ 120) multiplied by " + Val;
        }

        public override void Calculate(ProgramData pd)
        {
            DateTime startTime = DateTime.Now;
            foreach (BoughtProduct bp in pd.AllBoughtProducts)
            {
                if(!bp.ExpirationData.HasValue) continue;
                int val = CalcExpirationValue(bp.ExpirationData, startTime);
                bp.OwnValue.AddValue(val, this);
            }
        }

        private int CalcExpirationValue(ExpirationInfo exp, DateTime time)
        {
            var decimalExpired = (int)(exp.GetExpiredPercentage(time) * 100);
            if (decimalExpired == 0) return 0;
            double formulaResult = ValueDecayFormula(decimalExpired);
            return Val * Convert.ToInt32(formulaResult);
        }
    }
}