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
        public Amount(int amountInGrams) : this(MeasurementUnit.Unit.Gram, amountInGrams) { }

        public Amount(MeasurementUnit.Unit selectedUnit, int amountInSelectedUnit)
        {
            SelectedUnit = selectedUnit;
            ToGrams = Convert.ToInt32(MeasurementUnit.ConvertToGram(selectedUnit) * amountInSelectedUnit);
        }

        [DataMember]
        public MeasurementUnit.Unit SelectedUnit { get; private set; }

        [DataMember]
        public int ToGrams { get; private set; }

        public int ToSelectedUnit => Convert.ToInt32(MeasurementUnit.GetConversionRate(MeasurementUnit.Unit.Gram, SelectedUnit) * ToGrams);

        public void AddAmount(Amount amountToAdd)
        {
            AddAmount(amountToAdd.ToGrams);
        }

        public void AddAmount(int amountToAddInGrams)
        {
            ToGrams += amountToAddInGrams;
        }
    }
}
