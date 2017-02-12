using System;
using System.Runtime.Serialization;

namespace RecipeSelectHelper.Model.SortingMethods
{
    [KnownType(typeof(Preference))]
    [DataContract(Name = "Preference")]
    public class Preference : IPreference
    {
        [DataMember]
        public Action<ProgramData> CalculationMethod { get; private set; }
        [DataMember]
        public string Description { get; private set; }

        public Preference(Action<ProgramData> calculationMethod, string description = null)
        {
            this.CalculationMethod = calculationMethod;
            Description = description;
        }

        public void Calculate(ProgramData pd)
        {
            CalculationMethod(pd);
        }
    }
}