using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RecipeSelectHelper.Resources
{
    [DataContract(Name = "Amount")]
    public class Amount
    {
        public Amount(MeasurementUnit.Unit selectedUnit, int amountInSelectedUnit)
        {
            SelectedUnit = selectedUnit;
            AmountInSelectedUnit = amountInSelectedUnit;
        }

        public int ToGrams => Convert.ToInt32(MeasurementUnit.ConvertToGram(SelectedUnit) * AmountInSelectedUnit);

        [DataMember]
        public MeasurementUnit.Unit SelectedUnit { get; }

        [DataMember]
        public int AmountInSelectedUnit { get; }
    }
}
