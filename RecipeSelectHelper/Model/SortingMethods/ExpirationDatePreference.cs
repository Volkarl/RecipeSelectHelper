using System;
using System.Runtime.Serialization;
using System.Windows;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [DataContract(Name = "ExpirationDatePreference")]
    public class ExpirationDatePreference : Preference
    {
        [DataMember]
        public int Val { get; set; }

        private static Func<double, double> _valueFormula = x => (-0.4371 * Math.Pow(x, 2)) + (15.851 * x) - 22.42;

        public ExpirationDatePreference(int val, string description = null) : base(description)
        {
            Val = val;
            Description += nameof(ExpirationDatePreference) + " | Expiration date value (0 to ≈ 120) multiplied by " + Val;
        }

        public override void Calculate(ProgramData pd)
        {
            DateTime now = DateTime.Now;
            pd.AllBoughtProducts.ForEach(x => x.Value += CalculateValue(x.ExpirationData, now));
        }

        private int CalculateValue(ExpirationInfo exp, DateTime time)
        {
            int decimalExpired = (int)(exp.GetExpiredPercentage(time) * 100);
            double formulaResult = _valueFormula(decimalExpired);
            return Val * Convert.ToInt32(formulaResult);
        }
    }
}